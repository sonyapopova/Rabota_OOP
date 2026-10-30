using System;
using System.Collections.Generic;
using System.Linq;

namespace classiki
{
    /// <summary>
    /// Корзина: набор товарных позиций. Скидка и лимит оплаты бонусами
    /// берутся из профиля пользователя (UserProfile).
    /// </summary>
    public class Cart
    {
        private readonly List<ProductItem> _items = new List<ProductItem>();

        public IReadOnlyList<ProductItem> Items => _items;

        public int PositionsCount => _items.Count;

        public bool IsEmpty => _items.Count == 0;

        public decimal TotalPrice => RecalculateTotal();

        public void Add(ProductItem item)
        {
            ValidateItem(item);

            _items.Add(item);
        }

        public bool Remove(ProductItem item)
        {
            ValidateItem(item);

            return _items.Remove(item);
        }

        public void Clear() => _items.Clear();

        /// <summary>
        /// Рассчитывает цену корзины с учётом множителя скидки.
        /// </summary>
        public CartPrice Calculate(decimal discountMultiplier)
        {
            return new CartPrice(RecalculateTotal(), discountMultiplier);
        }

        private decimal RecalculateTotal()
        {
            return _items.Sum(item => (decimal)item.TotalPrice);
        }

        private static void ValidateItem(ProductItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
        }
    }
}
