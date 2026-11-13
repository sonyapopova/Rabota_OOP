using System;

namespace Marketplace.Domain
{
    /// <summary>
    /// Товар: идентификатор, название, цена, артикул и статус наличия.
    /// </summary>
    public class Product
    {
        private int id;
        private string title = string.Empty;
        private decimal price;
        private string article = string.Empty;

        public Product(int id, string name, decimal price)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "Идентификатор товара должен быть положительным.");

            EnsureTitle(name);
            EnsurePrice(price);

            this.id = id;
            title = name.Trim();
            this.price = price;
        }

        public string Article
        {
            get => article;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(
                        "Артикул обязателен.",
                        nameof(value));

                article = value;
            }
        }

        public bool IsBudget => GetPrice() < 25m;

        public string Status { get; private set; } = "В наличии";

        private static readonly string[] AllowedStatuses =
        {
            "В наличии",
            "Нет в наличии",
            "Под заказ"
        };

        public int GetId() => id;

        public string GetTitle() => title;

        public decimal GetPrice() => price;

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

        public string GetInfo() => GetInfo(true);

        public string GetInfo(bool includeIntProperty)
        {
            string info = $"Товар №{GetId()}: {GetTitle()} — {GetPrice():F2} котокоинов";

            if (!string.IsNullOrWhiteSpace(Article))
                info += $", артикул: {Article}";

            if (includeIntProperty)
                info += $", бюджетный: {(IsBudget ? "да" : "нет")}";

            return info;
        }

        public void ChangeStatus(string newStatus)
        {
            EnsureStatus(newStatus);
            Status = newStatus;
        }

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

        private static void EnsureStatus(string value)
        {
            if (Array.IndexOf(AllowedStatuses, value) < 0)
                throw new ArgumentException(
                    $"Недопустимый статус: {value}",
                    nameof(value));
        }
    }
}
