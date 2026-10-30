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

        public decimal TotalPrice => _items.Sum(item => (decimal)item.TotalPrice);

        public void Add(ProductItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _items.Add(item);
        }

        public bool Remove(ProductItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            return _items.Remove(item);
        }

        public void Clear() => _items.Clear();
    }
}
