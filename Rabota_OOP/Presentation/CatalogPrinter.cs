using System;
using System.Collections.Generic;
using System.Linq;
using Marketplace.Domain;

namespace Marketplace.Presentation
{
    /// <summary>
    /// Вывод информации о товарах каталога.
    /// </summary>
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

            foreach (var product in products.OrderByDescending(p => p.GetPrice()).Take(count))
                Console.WriteLine(PrepareLine(product));
        }

        private string PrepareLine(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            return $"Товар №{product.GetId()}: {product.GetTitle()} — {PriceFormatter.Format(product.GetPrice())}";
        }
    }

    /// <summary>
    /// Форматирование цены. Тип виден только в пределах этого файла.
    /// </summary>
    file static class PriceFormatter
    {
        public static string Format(int price) => $"{price} котокоинов";

        public static string Format(decimal price) => $"{price:F2} котокоинов";
    }
}
