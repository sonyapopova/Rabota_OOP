using System;
using System.Threading;

namespace classiki
{
    /// <summary>
    /// Товарная позиция в корзине: товар и количество единиц.
    /// </summary>
    public class ProductItem
    {
        private static int productItemId = 0;

        public int ID { get; }
        public int ProductId => Product.Id;
        public Product Product { get; }
        public string Name => Product.Name;
        public decimal Price => Product.Price;
        public int Quantity { get; }
        public DateTime PurchaseDate { get; }
        public decimal TotalPrice => Price * Quantity;

        public ProductItem(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(quantity),
                    "Количество должно быть больше нуля.");

            ID = Interlocked.Increment(ref productItemId);
            Product = product;
            Quantity = quantity;
            PurchaseDate = DateTime.Now;
        }
    }
}
