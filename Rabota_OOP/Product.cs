using System;

namespace classiki
{
    /// <summary>
    /// Товар: идентификатор, название и цена за единицу.
    /// </summary>
    public class Product
    {
        public int Id { get; }
        public string Name { get; }
        public decimal Price { get; }

        public Product(int id, string name, decimal price)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "Идентификатор товара должен быть положительным.");

            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Название товара не может быть пустым.", nameof(name));

            if (price < 0m)
                throw new ArgumentOutOfRangeException(
                    nameof(price),
                    "Цена товара не может быть отрицательной.");

            Id = id;
            Name = name.Trim();
            Price = price;
        }
    }
}
