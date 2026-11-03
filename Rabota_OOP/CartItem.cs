using System;
using System.Threading;

namespace classiki
{
    /// <summary>
    /// Товарная позиция в корзине: товар и количество единиц.
    /// </summary>
    public class CartItem
    {
        private static int cartItemId = 0;

        public int Id { get; }
        public int ProductId => Product.Id;
        public Product Product { get; }
        public int Quantity { get; }
        public decimal TotalPrice => Product.Price * Quantity;

        public CartItem(Product product, int quantity)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (quantity <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(quantity),
                    "Количество должно быть больше нуля.");

            Id = Interlocked.Increment(ref cartItemId);
            Product = product;
            Quantity = quantity;
        }
    }
}
