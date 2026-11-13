using System;
using System.Collections.Generic;
using System.IO;
using Marketplace.DataAccess;
using Marketplace.Domain;

namespace Marketplace.Presentation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ProfileAdd profileService = new ProfileAdd();
            PaymentService paymentService = new PaymentService();

            
            List<Channel> channels = new List<Channel>
            {
                new Channel("Телефон"),
                new Channel("Email"),
                new Channel("Telegram")
            };

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

            Console.WriteLine("\n=== Сценарий 1: успешная оплата ===");

            Cart cart = new Cart(student.UserId);
            cart.Add(new CartItem(new Product(1, "Ноутбук", 1000m), 1));
            cart.Add(new CartItem(new Product(2, "Мышь", 50m), 2));

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

            Cart foreignCart = new Cart(student.UserId + 1);
            foreignCart.Add(new CartItem(new Product(4, "Наушники", 1000m), 1));

            try
            {
                paymentService.PayForPurchase(studentProfile, foreignCart, out _);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Оплата чужой корзины отклонена: {ex.Message}");
            }

            Console.WriteLine("\n=== Сценарий 2: оплата невозможна, затем пополнение и повтор ===");

            Cart expensiveCart = new Cart(student.UserId);
            expensiveCart.Add(new CartItem(new Product(3, "Телевизор", 50000m), 1));

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

            Console.WriteLine("\n=== Задание 3: каталог, DataAccess и отчёты ===");

            Catalog catalog = new Catalog();

            try
            {
                catalog.Add(new Product(4, "Некорректный товар", -5m));
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Данные отклонены: {ex.Message}");
            }

            try
            {
                Product priceCheck = new Product(5, "Проверка цены", 100m);
                priceCheck.SetPrice(-100);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"SetPrice(-100) отклонён: {ex.Message}");
            }

            catalog.Add(new Product(1, "Кружка", 450m));
            catalog.Add(new Product(2, "Термос", 1200m));
            catalog.Add(new Product(3, "Набор стикеров", 90m));

            Console.WriteLine($"Товаров в каталоге: {catalog.GetCount()}");

            Product? foundProduct = catalog.Find("термос");
            Console.WriteLine(foundProduct is null
                ? "Товар «Термос» не найден."
                : $"Найден товар: {foundProduct.GetTitle()} — {foundProduct.GetPrice():F2} котокоинов");

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

            // Снаружи нельзя изменить внутреннюю коллекцию каталога:
            // catalog.GetProducts().Clear(); // не компилируется: у IReadOnlyList<T> нет Clear.
        }
    }
}
