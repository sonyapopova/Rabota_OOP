using System;
using System.Collections.Generic;
using System.Text;
using Marketplace.Domain;

namespace Marketplace.DataAccess
{
    internal class FileProductRepository : IProductRepository
    {
        private readonly string filePath;
        public FileProductRepository(string filePath) => this.filePath =
        filePath;

        public void Save(IEnumerable<Product> products)
        {
            var lines = new List<string>();
            foreach (var product in products)
            {
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
            return new List<Product>();
        }
        private static string Serialize(Product product) => $"{product.GetId()};{product.GetTitle()};{product.GetPrice()}";
        private static Product Deserialize(string line)
        {
            var parts = line.Split(';');
            int id = int.Parse(parts[0]);
            string title = parts[1];
            int price = int.Parse(parts[2]);
            return new Product(id, title, price);
        }
    }
}