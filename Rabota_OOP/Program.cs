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
            cart.Add(new CartItem(new Product("Ноутбук", 1000m), 1));
            cart.Add(new CartItem(new Product("Мышь", 50m), 2));

            cart.ShowDiscountAmount();
            decimal total = cart.ShowFinalPrice();

            BuyerBalance buyer = new BuyerBalance(500m, 2000m);
            PaymentService paymentService = new PaymentService();
            bool paid = paymentService.Pay(buyer, total);

            Console.WriteLine($"Оплата прошла: {paid}");
            Console.WriteLine($"Остаток бонусов: {buyer.Bonuses} УЕ, денег: {buyer.Money} УЕ");
        }
    }
}
