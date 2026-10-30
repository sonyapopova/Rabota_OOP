using System;

namespace classiki
{
    /// <summary>
    /// Расчётная цена корзины: исходная цена, множитель скидки и размер скидки в деньгах.
    /// </summary>
    public readonly record struct CartPrice
    {
        public decimal OriginalPrice { get; }
        public decimal DiscountMultiplier { get; }
        public decimal DiscountAmount { get; }

        /// <summary>Итоговая цена корзины с учётом скидки.</summary>
        public decimal FinalPrice => OriginalPrice - DiscountAmount;

        public CartPrice(decimal originalPrice, decimal discountMultiplier)
        {
            if (originalPrice < 0m)
                throw new ArgumentOutOfRangeException(
                    nameof(originalPrice),
                    "Исходная цена не может быть отрицательной.");

            EnsureDiscountRange(discountMultiplier);

            OriginalPrice = originalPrice;
            DiscountMultiplier = discountMultiplier;

            decimal finalPrice = Math.Round(originalPrice * discountMultiplier, 2);
            DiscountAmount = originalPrice - finalPrice;
        }

        private static void EnsureDiscountRange(decimal discountMultiplier)
        {
            if (discountMultiplier < 0.01m || discountMultiplier > 1.00m)
                throw new ArgumentOutOfRangeException(
                    nameof(discountMultiplier),
                    "Множитель скидки должен находиться в диапазоне от 0.01 до 1.00.");
        }
    }
}
