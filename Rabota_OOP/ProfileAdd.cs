using System;
using System.Collections.Generic;
using System.Linq;

namespace classiki
{
    public class ProfileAdd
    {

        //Рома ОТСЮДА
        public void TopUpBalance(UserProfile profile, decimal amount)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            if (amount <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(amount),
                    "Сумма пополнения должна быть больше нуля.");

            //Пополняем денежный баланс
            profile.MoneyBalance += amount;

            //Начисляем 10% от суммы пополнения бонусами
            profile.BonusBalance += (double)(amount * 0.10m);
        }

        public bool PayForPurchase(UserProfile profile, ProductItem product, out string message)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            Cart cart = new Cart();
            cart.Add(product);

            return PayForPurchase(profile, cart, out message);
        }

        public bool PayForPurchase(UserProfile profile, Cart cart, out string message)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            if (cart == null)
                throw new ArgumentNullException(nameof(cart));

            if (cart.IsEmpty)
                throw new InvalidOperationException("Корзина пуста: нечего оплачивать.");

            //Корзина сама считает цену с учётом персональной скидки профиля
            CartPrice price = cart.Calculate((decimal)profile.PersonalDiscount);

            return TryPay(profile, cart, price, out message);
        }

        /// <summary>
        /// Сколько денег нужно доплатить за корзину при текущем балансе бонусов
        /// (без учёта возможного пополнения).
        /// </summary>
        public decimal CalculateMoneyPayment(UserProfile profile, Cart cart)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            if (cart == null)
                throw new ArgumentNullException(nameof(cart));

            if (cart.IsEmpty)
                throw new InvalidOperationException("Корзина пуста: нечего оплачивать.");

            CartPrice price = cart.Calculate((decimal)profile.PersonalDiscount);
            BuyerBalanceState balance = CreateBalanceState(profile);

            return CalculateMoneyPayment(price, balance);
        }

        //Рома: расчёт оплаты бонусами и деньгами
        private static decimal CalculateBonusPayment(CartPrice price, BuyerBalanceState balance)
        {
            //Максимальная сумма, которую разрешено оплатить бонусами
            decimal maxBonusPayment = Math.Floor(
                price.FinalPrice * (decimal)balance.BonusPaymentShareMultiplier);

            //Фактическая сумма оплаты бонусами
            return Math.Min(maxBonusPayment, (decimal)balance.Bonuses);
        }

        private static decimal CalculateMoneyPayment(CartPrice price, BuyerBalanceState balance)
        {
            //Сумма, которую необходимо оплатить деньгами
            return price.FinalPrice - CalculateBonusPayment(price, balance);
        }

        //Соня: проведение оплаты и формирование сообщения
        private static bool TryPay(UserProfile profile, Cart cart, CartPrice price, out string message)
        {
            BuyerBalanceState balance = CreateBalanceState(profile);

            decimal bonusPayment = CalculateBonusPayment(price, balance);
            decimal moneyPayment = CalculateMoneyPayment(price, balance);
            int positionsCount = cart.PositionsCount;

            // Проверяем достаточность денежных средств.
            // При нехватке средств корзина и балансы остаются без изменений.
            if (balance.Money < moneyPayment)
            {
                decimal missing = moneyPayment - balance.Money;

                message = BuildPaymentMessage(
                    profile, cart, price, bonusPayment, moneyPayment, missing, false, positionsCount);

                return false;
            }

            //Списываем денежные средства
            profile.MoneyBalance = balance.Money - moneyPayment;

            //Списываем использованные бонусы
            profile.BonusBalance = balance.Bonuses - (double)bonusPayment;

            //Добавляем покупки в историю
            foreach (var item in cart.Items)
                profile.AddPurchase(item);

            //Опустошаем корзину: оплаченные позиции в ней больше не нужны
            cart.Clear();

            message = BuildPaymentMessage(
                profile, cart, price, bonusPayment, moneyPayment, 0m, true, positionsCount);

            return true;
        }

        private static string BuildPaymentMessage(
            UserProfile profile,
            Cart cart,
            CartPrice price,
            decimal bonusPayment,
            decimal moneyPayment,
            decimal missing,
            bool success,
            int positionsCount)
        {
            if (!success)
            {
                return
                    "Недостаточно денежных средств для оплаты покупки.\n" +
                    $"Требуется деньгами: {moneyPayment:F2} котокоинов\n" +
                    $"На балансе: {profile.MoneyBalance:F2} котокоинов\n" +
                    $"Не хватает: {missing:F2} котокоинов\n" +
                    "Корзина и баланс не изменены.";
            }

            return
                "Покупка успешно оплачена!\n" +
                $"Позиций оплачено: {positionsCount}\n" +
                $"Стоимость до скидки: {price.OriginalPrice:F2} котокоинов\n" +
                $"Размер скидки: {price.DiscountAmount:F2} котокоинов\n" +
                $"Стоимость со скидкой: {price.FinalPrice:F2} котокоинов\n" +
                $"Оплачено бонусами: {bonusPayment:F2}\n" +
                $"Оплачено деньгами: {moneyPayment:F2} котокоинов\n" +
                $"Корзина очищена: товаров {cart.PositionsCount}, итоговая цена {cart.TotalPrice:F2} котокоинов";
        }

        private static BuyerBalanceState CreateBalanceState(UserProfile profile)
        {
            return new BuyerBalanceState(
                profile.BonusBalance,
                profile.MoneyBalance,
                profile.BonusPaymentLimit);
        }
        //Рома ДО СЮДА

        public UserProfile CreateProfile(User user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return new UserProfile(user);
        }

        public void AddContactToProfile(UserProfile profile, Contact contact)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            if (contact == null)
                throw new ArgumentNullException(nameof(contact));

            profile.AddContact(contact);
        }

        public bool RemoveContactFromProfile(UserProfile profile, int contactId)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            return profile.RemoveContact(contactId);
        }

        public void PrintProfileSummary(UserProfile profile, List<Channel> activeChannels)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            if (activeChannels == null)
                throw new ArgumentNullException(nameof(activeChannels));

            var (shortName, userLogin, contactsCount, moneyBalance, bonusBalance) = profile;

            Console.WriteLine($"\nСоздан новый профиль: (ID: {profile.User.UserId})");
            Console.WriteLine($"Пользователь: {shortName} | Логин: {userLogin}");
            Console.WriteLine($"Профиль создан: {profile.CreatedAt}");
            Console.WriteLine($"Баланс: {moneyBalance:F2} котокоинов | Бонусы: {bonusBalance:F2}");
            Console.WriteLine($"Всего контактов: {contactsCount}");
            Console.WriteLine($"Совершено покупок: {profile.Purchases.Count}");

            foreach (var contact in profile.Contacts)
            { 
                var channelName = activeChannels.FirstOrDefault(c => c.ID == contact.ChannelID)?.Name ?? "Неизвестный канал";
                Console.WriteLine($"  -> [Контакт ID: {contact.ContactId}] {channelName}: {contact.Value}");
            }
            
        }
    }
}
