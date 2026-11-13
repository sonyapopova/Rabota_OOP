using System;
using System.Collections.Generic;
using System.Linq;
using Marketplace.Domain;

namespace Marketplace.Presentation
{
    /// <summary>
    /// Расчёты по каталогу товаров. Не выполняет печать и не работает с файлами.
    /// </summary>
    public class CatalogReport
    {
        public decimal GetAveragePrice(IEnumerable<Product> products)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            var productList = products.ToList();

            return productList.Count == 0
                ? 0m
                : productList.Average(product => (decimal)product.Price);
        }

        public string GetSummary(IEnumerable<Product> products)
        {
            if (products == null)
                throw new ArgumentNullException(nameof(products));

            var productList = products.ToList();

            if (productList.Count == 0)
                return "Товаров нет.";

            decimal totalPrice = productList.Sum(product => (decimal)product.Price);
            decimal averagePrice = GetAveragePrice(productList);
            decimal minPrice = productList.Min(product => (decimal)product.Price);
            decimal maxPrice = productList.Max(product => (decimal)product.Price);

            return
                $"Товаров: {productList.Count}; " +
                $"общая стоимость: {totalPrice:F2} котокоинов; " +
                $"средняя цена: {averagePrice:F2} котокоинов; " +
                $"минимальная цена: {minPrice:F2} котокоинов; " +
                $"максимальная цена: {maxPrice:F2} котокоинов.";
        }
    }
}
