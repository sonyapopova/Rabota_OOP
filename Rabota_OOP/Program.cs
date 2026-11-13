using System;
using System.Collections.Generic;
using System.IO;
using Marketplace.DataAccess;
using Marketplace.Domain;
using Marketplace.Services;

namespace Marketplace.Presentation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ProfileAdd profileService = new ProfileAdd();
            PaymentService paymentService = new PaymentService();
            List<Channel> channels = CreateChannels();

            UserProfile studentProfile = RunProfileScenario(profileService, channels);
            RunPaymentScenarios(profileService, paymentService, studentProfile, channels);
            RunCatalogScenario();
            RunPropertiesScenario();
            RunCheckoutScenario();
            RunFileFormatCheck();
        }

        private static List<Channel> CreateChannels()
        {
            return new List<Channel>
            {
                new Channel("Телефон"),
                new Channel("Email"),
                new Channel("Telegram")
            };
        }

        private static UserProfile RunProfileScenario(ProfileAdd profileService, List<Channel> channels)
        {
            User student = new User("Акакий", "Акакиевич", "Добрый", "akakii_dev", "parolnagorshkesidelkorol");
            UserProfile studentProfile = profileService.CreateProfile(student);

            Contact phoneContact = new Contact("+88005553555", channels[0].ID);
            Contact emailContact = new Contact("akakii@gmail.com", channels[1].ID);
            Contact tgContact = new Contact("@a_kak_tg", channels[2].ID);

            profileService.AddContactToProfile(studentProfile, phoneContact);
            profileService.AddContactToProfile(studentProfile, emailContact);
            profileService.AddContactToProfile(studentProfile, tgContact);

            profileService.PrintProfileSummary(studentProfile, channels);

            Console.WriteLine($"\nУдаляем контакт Telegram (ID: {tgContact.ContactId})...");
            profileService.RemoveContactFromProfile(studentProfile, tgContact.ContactId);

            profileService.PrintProfileSummary(studentProfile, channels);

            return studentProfile;
        }

        private static void RunPaymentScenarios(
            ProfileAdd profileService,
            PaymentService paymentService,
            UserProfile studentProfile,
            List<Channel> channels)
        {
            Console.WriteLine("\n=== Сценарий 1: успешная оплата ===");

            Cart cart = new Cart(studentProfile.User.UserId);
            Product notebook = new Product { Id = 1, Title = "Ноутбук", Price = 1000 };
            Product mouse = new Product { Id = 2, Title = "Мышь", Price = 50 };
            cart.Add(new CartItem(notebook.Id, 1, notebook.Price));
            cart.Add(new CartItem(mouse.Id, 2, mouse.Price));

            Console.WriteLine($"Позиций в корзине: {cart.PositionsCount}, сумма: {cart.TotalPrice:F2} котокоинов");

            studentProfile.PersonalDiscount = 0.90m;
            studentProfile.BonusPaymentLimit = 0.30m;

            profileService.TopUpBalance(studentProfile, 2000m);
            Console.WriteLine($"После пополнения: {studentProfile.MoneyBalance:F2} котокоинов, бонусов: {studentProfile.BonusBalance:F2}");

            bool paid = paymentService.PayForPurchase(studentProfile, cart, out string paymentMessage);
            Console.WriteLine(paymentMessage);
            Console.WriteLine($"Состояние корзины после оплаты: товаров {cart.PositionsCount}, итоговая цена {cart.TotalPrice:F2} котокоинов");
            Console.WriteLine($"Проверка корзины: товаров {cart.PositionsCount}, TotalPrice = {cart.TotalPrice:F2} — " +
                $"{(cart.IsEmpty && cart.TotalPrice == 0m ? "корзина пуста" : "корзина не пуста")}");

            profileService.PrintProfileSummary(studentProfile, channels);

            Console.WriteLine("\n=== Проверка владельца корзины ===");

            Cart foreignCart = new Cart(studentProfile.User.UserId + 1);
            foreignCart.Add(new CartItem(4, 1, 1000));

            try
            {
                paymentService.PayForPurchase(studentProfile, foreignCart, out _);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Оплата чужой корзины отклонена: {ex.Message}");
            }

            Console.WriteLine("\n=== Сценарий 2: оплата невозможна, затем пополнение и повтор ===");

            Cart expensiveCart = new Cart(studentProfile.User.UserId);
            expensiveCart.Add(new CartItem(3, 1, 50000));

            Console.WriteLine($"Корзина: товаров {expensiveCart.PositionsCount}, итоговая цена {expensiveCart.TotalPrice:F2} котокоинов");
            Console.WriteLine($"Денег на балансе: {studentProfile.MoneyBalance:F2} котокоинов, бонусов: {studentProfile.BonusBalance:F2}");

            bool failed = paymentService.PayForPurchase(studentProfile, expensiveCart, out string failedMessage);
            Console.WriteLine($"Результат оплаты: {failed}");
            Console.WriteLine(failedMessage);
            Console.WriteLine($"Корзина не изменилась: товаров {expensiveCart.PositionsCount}, итоговая цена {expensiveCart.TotalPrice:F2} котокоинов");
            Console.WriteLine($"Баланс не изменился: денег {studentProfile.MoneyBalance:F2} котокоинов, бонусов {studentProfile.BonusBalance:F2}");

            decimal needMoney = paymentService.CalculateMoneyPayment(studentProfile, expensiveCart);
            decimal missingMoney = needMoney - studentProfile.MoneyBalance;
            Console.WriteLine($"\nПополняем на недостающую сумму: {missingMoney:F2} котокоинов");
            profileService.TopUpBalance(studentProfile, missingMoney);
            Console.WriteLine($"После пополнения: денег {studentProfile.MoneyBalance:F2} котокоинов, бонусов {studentProfile.BonusBalance:F2}");

            bool paidAfterTopUp = paymentService.PayForPurchase(studentProfile, expensiveCart, out string successMessage);
            Console.WriteLine($"Результат оплаты: {paidAfterTopUp}");
            Console.WriteLine(successMessage);
            Console.WriteLine($"Корзина после оплаты: товаров {expensiveCart.PositionsCount}, итоговая цена {expensiveCart.TotalPrice:F2} котокоинов");
            Console.WriteLine($"Проверка корзины: товаров {expensiveCart.PositionsCount}, TotalPrice = {expensiveCart.TotalPrice:F2} — " +
                $"{(expensiveCart.IsEmpty && expensiveCart.TotalPrice == 0m ? "корзина пуста" : "корзина не пуста")}");

            profileService.PrintProfileSummary(studentProfile, channels);
        }

        private static void RunCatalogScenario()
        {
            Console.WriteLine("\n=== Задание 3: каталог, DataAccess и отчёты ===");

            Catalog catalog = new Catalog();

            try
            {
                catalog.Add(new Product { Id = 4, Title = "Некорректный товар", Price = -5 });
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Данные отклонены: {ex.Message}");
            }

            try
            {
                Product priceCheck = new Product { Id = 5, Title = "Проверка цены", Price = 100 };
                priceCheck.Price = -100;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Price = -100 отклонён: {ex.Message}");
            }

            Product infoCheck = new Product { Id = 6, Title = "Проверка GetInfo", Price = 20 };
            infoCheck.Article = "ART-6";
            infoCheck.ChangeStatus("Под заказ");
            Console.WriteLine(infoCheck.GetInfo());
            Console.WriteLine($"Бюджетный: {infoCheck.IsBudget}, статус: {infoCheck.Status}");

            try
            {
                infoCheck.ChangeStatus("Продано");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Статус отклонён: {ex.Message}");
            }

            catalog.Add(new Product { Id = 1, Title = "Кружка", Price = 450 });
            catalog.Add(new Product { Id = 2, Title = "Термос", Price = 1200 });
            catalog.Add(new Product { Id = 3, Title = "Набор стикеров", Price = 90 });

            try
            {
                catalog.Add(new Product { Id = 2, Title = "КРУЖКА", Price = 500 });
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Дубликат отклонён: {ex.Message}");
            }

            Console.WriteLine($"Товаров в каталоге: {catalog.GetCount()}");

            Product? foundProduct = catalog.Find("термос");
            Console.WriteLine(foundProduct is null
                ? "Товар «Термос» не найден."
                : $"Найден товар: {foundProduct.Title} — {foundProduct.Price:F2} котокоинов");

            bool removed = catalog.Remove("Набор стикеров");
            Console.WriteLine($"Удаление товара «Набор стикеров»: {removed}");

            CatalogPrinter printer = new CatalogPrinter();
            Console.WriteLine("\nТовары каталога:");
            printer.PrintAll(catalog.GetProducts());

            Console.WriteLine("\nДва самых дорогих товара:");
            printer.PrintTop(catalog.GetProducts(), 2);

            CatalogReport report = new CatalogReport();
            Console.WriteLine($"\nСредняя цена: {report.GetAveragePrice(catalog.GetProducts()):F2} котокоинов");
            Console.WriteLine(report.GetSummary(catalog.GetProducts()));

            // DIP: работаем через интерфейс, а не через конкретный класс.
            string productsFile = Path.Combine(Path.GetTempPath(), "marketplace_products.csv");
            IProductRepository repository = new FileProductRepository(productsFile);
            repository.Save(catalog.GetProducts());

            List<Product> loadedProducts = repository.Load();
            Console.WriteLine($"\nЗагружено товаров из файла: {loadedProducts.Count}");
            printer.PrintAll(loadedProducts);

            // При сборке с -p:DefineConstants=NEGATIVE_CHECKS строка ниже
            // даёт CS1061: у IReadOnlyList<Product> нет метода Clear.
#if NEGATIVE_CHECKS
            catalog.GetProducts().Clear();
#endif
        }

        private static void RunPropertiesScenario()
        {
            Console.WriteLine("\n=== Задание 4: свойства Product, CustomerProfile и Discount ===");

            Product budgetProduct = new Product { Id = 7, Title = "Блокнот", Price = 20 };
            budgetProduct.Article = "ART-7";
            Console.WriteLine(budgetProduct.GetInfo());
            Console.WriteLine(budgetProduct.GetInfo(false));

            // Иллюстрация попытки вызвать сеттер для неизменяемого свойства:
            // budgetProduct.Title = "Другой блокнот";  // CS8852: свойство можно назначить только в инициализаторе объекта или в аксессоре init
            // budgetProduct.Status = "Продан";         // CS0272: аксессор set свойства "Product.Status" недоступен

            try
            {
                budgetProduct.Article = " ";
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Пустой артикул отклонён: {ex.Message}");
            }

            CustomerProfile firstBuyer = new CustomerProfile
            {
                FullName = "Анна Петрова",
                Phone = "+7 900 000-00-00",
                Email = "anna@example.com",
                Rating = 5
            };

            CustomerProfile secondBuyer = new CustomerProfile
            {
                FullName = "Иван Сергеев",
                Email = "ivan@example.com",
                Rating = 2
            };

            Console.WriteLine(firstBuyer.GetInfo());
            Console.WriteLine(firstBuyer.GetInfo(true));
            Console.WriteLine(firstBuyer.GetContactInfo(true));
            Console.WriteLine($"{secondBuyer.FullName}: доверенный покупатель — {secondBuyer.IsTrusted}");

            try
            {
                secondBuyer.Rating = 7;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Рейтинг 7 отклонён: {ex.Message}");
            }

            try
            {
                secondBuyer.Email = "ivan.example.com";
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Email без @ отклонён: {ex.Message}");
            }

            Discount discount = new Discount
            {
                Name = "Осенняя распродажа",
                Percent = 15m,
                MinSum = 1000,
                ValidUntil = DateTime.Now.AddDays(7)
            };

            Console.WriteLine($"Скидка «{discount.Name}» активна: {discount.IsActive}");
            Console.WriteLine($"Заказ на 900 котокоинов (меньше MinSum): {discount.Calculate(900)}");
            Console.WriteLine($"Заказ на 2000 котокоинов со скидкой 15%: {discount.Calculate(2000)}");
            Console.WriteLine($"Заказ на 2000 котокоинов со скидкой 25%: {discount.Calculate(2000, 25m)}");

            try
            {
                discount.Percent = 120m;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Percent = 120 отклонён: {ex.Message}");
            }

            try
            {
                discount.MinSum = -1;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"MinSum = -1 отклонён: {ex.Message}");
            }
        }

        private static void RunCheckoutScenario()
        {
            Console.WriteLine("\n=== Задание 4, участник 4: readonly struct CartItem и CheckoutService ===");

            List<CartItem> items = new List<CartItem>
            {
                new CartItem(1, 1, 1000),
                new CartItem(2, 2, 50),
                new CartItem(3, 3, 90)
            };

            CheckoutService checkoutService = new CheckoutService();

            Console.WriteLine($"Сумма по позициям: {checkoutService.GetTotal(items)} котокоинов");
            Console.WriteLine(checkoutService.GetReport(items));
            Console.WriteLine(checkoutService.GetReport(items, true));

            IReadOnlyList<CartItem> cheaper = checkoutService.FindCheaperThan(items, 500);
            Console.WriteLine($"Позиций дешевле 500 котокоинов: {cheaper.Count}");
            foreach (CartItem item in cheaper)
                Console.WriteLine($"  Товар №{item.ProductId}: {item.PricePerUnit} котокоинов за единицу");

            // Иллюстрация неизменяемости readonly struct:
            // items[0].Quantity = 5;  // CS0200: свойство "CartItem.Quantity" доступно только для чтения

            try
            {
                _ = new CartItem(3, 0, 90);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Количество 0 отклонено: {ex.Message}");
            }

            try
            {
                _ = new CartItem(3, 1, -10);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Отрицательная цена отклонена: {ex.Message}");
            }
        }

        private static void RunFileFormatCheck()
        {
            string filePath = Path.Combine(Path.GetTempPath(), "marketplace_products_semicolon.txt");
            IProductRepository repository = new FileProductRepository(filePath);
            Product product = new Product { Id = 99, Title = "Кружка; большая", Price = 500 };

            repository.Save(new[] { product });
            List<Product> loaded = repository.Load();

            Console.WriteLine(loaded.Count == 1 && loaded[0].Title == product.Title
                ? $"\nПроверка ';' в названии: загружено «{loaded[0].Title}»"
                : "\nПроверка ';' в названии: товар загружен неверно.");
        }
    }
}
