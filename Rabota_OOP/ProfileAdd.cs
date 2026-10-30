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

            if (cart.PositionsCount == 0)
                throw new InvalidOperationException("Корзина пуста: нечего оплачивать.");

            //Рассчитываем стоимость товаров с учётом скидки
            decimal originalPrice = cart.TotalPrice;

            decimal discountedPrice = Math.Round(originalPrice * (decimal)profile.PersonalDiscount, 2);

            //Максимальная сумма, которую разрешено оплатить бонусами
            decimal maxBonusPayment = Math.Floor(discountedPrice * (decimal)profile.BonusPaymentLimit);

            //Фактическая сумма оплаты бонусами
            decimal bonusPayment = Math.Min(maxBonusPayment, (decimal)profile.BonusBalance);

            //Сумма, которую необходимо оплатить деньгами
            decimal moneyPayment = discountedPrice - bonusPayment;

            // Проверяем достаточность денежных средств
            if (profile.MoneyBalance < moneyPayment)
            {
                message = "Недостаточно денежных средств для оплаты покупки.";
                return false;
            }

            //Списываем денежные средства
            profile.MoneyBalance -= moneyPayment;

            //Списываем использованные бонусы
            profile.BonusBalance -= (double)bonusPayment;

            //Добавляем покупки в историю
            foreach (var item in cart.Items)
                profile.AddPurchase(item);

            message =
                $"Покупка успешно оплачена!\n" +
                $"Позиций в корзине: {cart.PositionsCount}\n" +
                $"Стоимость до скидки: {originalPrice:F2} УЕ\n" +
                $"Стоимость со скидкой: {discountedPrice:F2} УЕ\n" +
                $"Оплачено бонусами: {bonusPayment:F2}\n" +
                $"Оплачено деньгами: {moneyPayment:F2} УЕ";

            return true;
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
            Console.WriteLine($"Баланс: {moneyBalance:F2} УЕ | Бонусы: {bonusBalance:F2}");
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
