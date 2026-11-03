using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace classiki
{
    /// <summary>
    /// Корзина: набор товарных позиций, привязанный к пользователю.
    /// Скидка и лимит оплаты бонусами берутся из профиля пользователя (UserProfile).
    /// </summary>
    public class Cart
    {
        private static int cartId = 0;
        private readonly List<ProductItem> _items = new List<ProductItem>();

        public int Id { get; }
        public int UserId { get; }
        public IReadOnlyList<ProductItem> Items => _items;

        public int PositionsCount => _items.Count;

        public bool IsEmpty => _items.Count == 0;

        public decimal TotalPrice => RecalculateTotal();

        public Cart(int userId)
        {
            if (userId <= 0)
                throw new ArgumentOutOfRangeException(
                    nameof(userId),
                    "Идентификатор пользователя должен быть положительным.");

            Id = Interlocked.Increment(ref cartId);
            UserId = userId;
        }

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
            return _items.Sum(item => item.TotalPrice);
        }

        private static void ValidateItem(ProductItem item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));
        }
    }
}
