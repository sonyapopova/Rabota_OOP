using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Marketplace.Domain;

namespace Marketplace.DataAccess
{
    /// <summary>
    /// Файловое хранилище товаров.
    /// Формат строки: id;title;price — по одному товару на строку.
    /// </summary>
    internal class FileProductRepository : IProductRepository
    {
        private readonly string filePath;

        public FileProductRepository(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentException(
                    "Путь к файлу не может быть пустым.",
                    nameof(filePath));

            this.filePath = filePath;
        }

        public void Save(IEnumerable<Product> products)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            var lines = new List<string>();

            foreach (var product in products)
            {
                if (product == null)
                    continue;

                lines.Add(Serialize(product));
            }

            File.WriteAllLines(filePath, lines);
        }

        public List<Product> Load()
        {
            var resultProducts = new List<Product>();

            if (!File.Exists(filePath))
                return resultProducts;

            foreach (var line in File.ReadAllLines(filePath))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                resultProducts.Add(Deserialize(line));
            }

            return resultProducts;
        }

        private static string Serialize(Product product) =>
            $"{product.GetId()};{product.GetTitle()};{product.GetPrice().ToString(CultureInfo.InvariantCulture)}";

        private static Product Deserialize(string line)
        {
            var parts = line.Split(';');

            if (parts.Length != 3)
                throw new FormatException($"Некорректная строка товара: {line}");

            int id = int.Parse(parts[0], CultureInfo.InvariantCulture);
            string title = parts[1];
            decimal price = decimal.Parse(parts[2], CultureInfo.InvariantCulture);

            return new Product(id, title, price);
        }
    }
}
