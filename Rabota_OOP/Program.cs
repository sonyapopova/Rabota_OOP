using System;
using System.Collections.Generic;

namespace classiki
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ProfileAdd profileService = new ProfileAdd();

            
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

            Console.WriteLine("\n--- Корзина и оплата ---");

            Cart cart = new Cart();
            cart.Add(new ProductItem("Ноутбук", 1000, 1));
            cart.Add(new ProductItem("Мышь", 50, 2));

            Console.WriteLine($"Позиций в корзине: {cart.PositionsCount}, сумма: {cart.TotalPrice:F2} УЕ");

            studentProfile.PersonalDiscount = 0.90;
            studentProfile.BonusPaymentLimit = 0.30;

            profileService.TopUpBalance(studentProfile, 2000m);
            Console.WriteLine($"После пополнения: {studentProfile.MoneyBalance:F2} УЕ, бонусов: {studentProfile.BonusBalance:F2}");

            bool paid = profileService.PayForPurchase(studentProfile, cart, out string paymentMessage);
            Console.WriteLine(paymentMessage);

            profileService.PrintProfileSummary(studentProfile, channels);
        }
    }
}
