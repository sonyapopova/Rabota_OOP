using System;
using System.Collections.Generic;
using System.Linq;

namespace classiki
{
    public class ProfilleAdd
    {
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
