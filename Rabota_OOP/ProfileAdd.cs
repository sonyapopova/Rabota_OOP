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
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            if (product == null)
                throw new ArgumentNullException(nameof(product));

            //Рассчитываем стоимость товаров с учётом скидки
            decimal originalPrice = (decimal)product.TotalPrice;

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

            //Добавляем покупку в историю
            profile.Purchases.Add(product);

            message =
                $"Покупка успешно оплачена!\n" +
                $"Стоимость до скидки: {originalPrice:F2} УЕ\n" +
                $"Стоимость со скидкой: {discountedPrice:F2} УЕ\n" +
                $"Оплачено бонусами: {bonusPayment:F2}\n" +
                $"Оплачено деньгами: {moneyPayment:F2} УЕ";

            return true;
        }
        //Рома ДО СЮДА

        public UserProfile CreateProfile(User user)
        {
            return new UserProfile(user);
        }

        public void AddContactToProfile(UserProfile profile, Contact contact)
        {
            profile.Contacts.Add(contact);
        }

        public bool RemoveContactFromProfile(UserProfile profile, int contactId)
        {
            var contact = profile.Contacts.FirstOrDefault(c => c.ContactId == contactId);
            if (contact != null)
            {
                profile.Contacts.Remove(contact);
                return true;
            }
            return false;
        }

        public void PrintProfileSummary(UserProfile profile, List<Channel> activeChannels)
        {
            var (shortName, userLogin) = profile.User;

            Console.WriteLine($"\nСоздан новый профиль: (ID: {profile.User.UserId})");
            Console.WriteLine($"Пользователь: {shortName} | Логин: {userLogin}");
            Console.WriteLine($"Профиль создан: {profile.CreatedAt}");
            Console.WriteLine($"Всего контактов: {profile.Contacts.Count}");

            foreach (var contact in profile.Contacts)
            { 
                var channelName = activeChannels.FirstOrDefault(c => c.ID == contact.ChanelID)?.Name ?? "Неизвестный канал";
                Console.WriteLine($"  -> [Контакт ID: {contact.ContactId}] {channelName}: {contact.Value}");
            }
            
        }
    }
}
