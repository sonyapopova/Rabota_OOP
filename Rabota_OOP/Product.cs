using System;

namespace Marketplace.Domain
{
    /// <summary>
    /// Товар: идентификатор, название и цена за единицу.
    /// </summary>
    public class Product
    {
        private decimal price;

        public int Id { get; }
        public string Name { get; }
        public decimal Price => price;

        public Product(int id, string name, decimal price)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "Идентификатор товара должен быть положительным.");

            EnsureTitle(name);
            EnsurePrice(price);

            Id = id;
            Name = name.Trim();
            this.price = price;
        }

        public int GetId() => Id;

        public string GetTitle() => Name;

        public decimal GetPrice() => Price;

        public void SetPrice(int newPrice)
        {
            EnsurePrice(newPrice);
            price = newPrice;
        }

        public void SetPrice(decimal newPrice)
        {
            EnsurePrice(newPrice);
            price = newPrice;
        }

        public string GetInfo() => $"Товар №{Id}: {Name} — {Price:F2} котокоинов";

        private static void EnsureTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException(
                    "Название товара не может быть пустым.",
                    nameof(title));
        }

        private static void EnsurePrice(decimal price)
        {
            if (price < 0m)
                throw new ArgumentOutOfRangeException(
                    nameof(price),
                    "Цена товара не может быть отрицательной.");
        }
    }
}
