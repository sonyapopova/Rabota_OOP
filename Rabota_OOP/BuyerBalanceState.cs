using System;

namespace Marketplace.Domain
{
    /// <summary>
    /// Состояние баланса покупателя: бонусы, деньги и разрешённая доля оплаты бонусами.
    /// </summary>
    public readonly record struct BuyerBalanceState
    {
        public decimal Bonuses { get; }
        public decimal Money { get; }
        public decimal BonusPaymentShareMultiplier { get; }

        public BuyerBalanceState(decimal bonuses, decimal money, decimal bonusPaymentShareMultiplier)
        {
            EnsureBonusesRange(bonuses);
            EnsureMoneyRange(money);
            EnsureBonusShareRange(bonusPaymentShareMultiplier);

            Bonuses = bonuses;
            Money = money;
            BonusPaymentShareMultiplier = bonusPaymentShareMultiplier;
        }

        private static void EnsureBonusesRange(decimal bonuses)
        {
            if (bonuses < 0m)
                throw new ArgumentOutOfRangeException(
                    nameof(bonuses),
                    "Баланс бонусов не может быть отрицательным.");
        }

        private static void EnsureMoneyRange(decimal money)
        {
            if (money < 0m)
                throw new ArgumentOutOfRangeException(
                    nameof(money),
                    "Денежный баланс не может быть отрицательным.");
        }

        private static void EnsureBonusShareRange(decimal bonusPaymentShareMultiplier)
        {
            if (bonusPaymentShareMultiplier < 0.0m || bonusPaymentShareMultiplier > 0.5m)
                throw new ArgumentOutOfRangeException(
                    nameof(bonusPaymentShareMultiplier),
                    "Доля оплаты бонусами должна находиться в диапазоне от 0.0 до 0.5.");
        }
    }
}
