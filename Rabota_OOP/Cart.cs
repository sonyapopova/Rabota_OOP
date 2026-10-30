using System;
using System.Collections.Generic;
using System.Linq;

namespace classiki
{
    public struct CartPrice
    {
        public decimal OriginalPrice { get; set; }
        public decimal DiscountMultiplier { get; set; }
        public decimal DiscountAmount { get; set; }
    }
    public class Cart
    {
        public int Id { get; }
        public int UserId { get; }
        private readonly List<CartItem> _items = new List<CartItem>();

        public Cart(int id, int usetId) 
        { 
            Id = id;
            UserId = usetId;
        }

        public int PositionsCount => _items.Count;

        public bool IsEmpty => _items.Count == 0;

        public decimal TotalPrice => FullPrice;

        public void Add(CartItem item) => _items.Add(item);

        public bool Remove(CartItem item) => _items.Remove(item);

        public void Clear() => _items.Clear();

        private decimal FullPrice => _items.Sum(i => i.Total);

        public CartPrice Calculate(decimal discountMultiplier)
        {
            var original = FullPrice;
            return new CartPrice
            {
                OriginalPrice = original,
                DiscountMultiplier = discountMultiplier,
                DiscountAmount = original * (1 - discountMultiplier)
            };
        }

        //public decimal ShowFinalPrice()
        //{ var price = CalculatePrice(); Console.WriteLine($"Итог: {price} УЕ"); return price; }

        //public decimal ShowDiscountAmount()
        //{ var amount = CalculateDiscountAmount(); Console.WriteLine($"Скидка: {amount} УЕ"); return amount; }
    }
}
