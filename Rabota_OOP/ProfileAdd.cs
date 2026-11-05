using System;
using System.Collections.Generic;
using System.Linq;

namespace Marketplace.Domain
{
    public class ProfileAdd
    {
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
            profile.BonusBalance += amount * 0.10m;
        }

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
