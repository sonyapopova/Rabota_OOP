using System;
using System.Collections.Generic;
using System.Linq;

namespace Marketplace.Domain
{
    /// <summary>
    /// Каталог товаров: добавление, поиск и удаление по названию.
    /// </summary>
    public class Catalog
    {
        private readonly List<Product> products = new();

        public void Add(Product product)
        {
            if (product is null)
                throw new ArgumentNullException(nameof(product));

            if (products.Any(p => SameTitle(p.Title, product.Title)))
                throw new InvalidOperationException(
                    $"Товар \"{product.Title}\" уже есть в каталоге.");

            products.Add(product);
        }

        public bool Remove(string title)
        {
            int index = products.FindIndex(p => SameTitle(p.Title, title));

            if (index < 0)
                return false;

            products.RemoveAt(index);
            return true;
        }

        public Product? Find(string title) =>
            products.FirstOrDefault(p => SameTitle(p.Title, title));

        public IReadOnlyList<Product> GetProducts() => products.ToList();

        public int GetCount() => products.Count;

        private static string Normalize(string title) =>
            (title ?? string.Empty).Trim().ToLowerInvariant();

        private static bool SameTitle(string left, string right) =>
            Normalize(left) == Normalize(right);
    }
}
