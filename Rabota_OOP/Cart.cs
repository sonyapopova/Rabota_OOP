using System;
using System.Collections.Generic;
using System.Linq;

namespace classiki
{
    public class Cart
    {
        private readonly List<CartItem> _items = new List<CartItem>();

        public decimal DiscountPercent { get; set; } = 5m;

        public void Add(CartItem item) => _items.Add(item);

        private decimal FullPrice => _items.Sum(i => i.Total);

        public decimal CalculatePrice() => FullPrice * (1 - DiscountPercent / 100m);

        public decimal CalculateDiscountAmount() => FullPrice * (DiscountPercent / 100m);

        public decimal ShowFinalPrice()
        { var price = CalculatePrice(); Console.WriteLine($"Итог: {price} УЕ"); return price; }

        public decimal ShowDiscountAmount()
        { var amount = CalculateDiscountAmount(); Console.WriteLine($"Скидка: {amount} УЕ"); return amount; }
    }
}
