using System;

namespace Marketplace.Domain
{
    /// <summary>
    /// Сохранённая покупка: товарная позиция с датой приобретения.
    /// </summary>
    public class ProductItem : CartItem
    {
        public DateTime PurchaseDate { get; }

        public ProductItem(Product product, int quantity) : base(product, quantity)
        {
            PurchaseDate = DateTime.Now;
        }
    }
}
