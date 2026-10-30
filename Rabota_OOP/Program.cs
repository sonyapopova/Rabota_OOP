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

            Cart cart = new Cart(1, student.UserId);
            cart.Add(new CartItem(1, new Product(1, "Ноутбук", 1000m), 1));
            cart.Add(new CartItem(2, new Product(2, "Мышь", 50m), 2));

            CartPrice price = cart.Calculate(0.8m);

            Console.WriteLine($"Исходная цена: {price.OriginalPrice} котокоинов");
            Console.WriteLine($"Скидка: {price.DiscountAmount} котокоинов");

            decimal total = price.OriginalPrice - price.DiscountAmount;
            Console.WriteLine($"Итог: {total} котокоинов");

            BuyerBalance buyer = new BuyerBalance(500m, 2000m);
            PaymentService paymentService = new PaymentService();
            bool paid = paymentService.Pay(buyer, total);

            Console.WriteLine($"Оплата прошла: {paid}");
            Console.WriteLine($"Остаток бонусов: {buyer.Bonuses} котокоинов, денег: {buyer.Money} котокоинов");
        }
    }
}
