using System;
using System.Collections.Generic;
using System.Linq;
using Marketplace.Domain;

namespace Marketplace.Presentation
{
    public class CatalogPrinter
    {
        public void PrintAll(IEnumerable<Product> products)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            foreach (var product in products)
                Console.WriteLine(PrepareLine(product));
        }

        public void PrintTop(IEnumerable<Product> products, int count)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            if (count <= 0)
                return;

            foreach (var product in products.OrderByDescending(p => p.Price).Take(count))
                Console.WriteLine(PrepareLine(product));
        }

        private string PrepareLine(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            return $"Товар №{product.Id}: {product.Title} — {PriceFormatter.Format(product.Price)}";
        }
    }

    file static class PriceFormatter
    {
        public static string Format(int price) => $"{price} котокоинов";

        public static string Format(decimal price) => $"{price:F2} котокоинов";
    }
}
