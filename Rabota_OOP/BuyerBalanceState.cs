using System;

namespace classiki
{
    /// <summary>
    /// Состояние баланса покупателя: бонусы, деньги и разрешённая доля оплаты бонусами.
    /// </summary>
    public readonly record struct BuyerBalanceState
    {
        public double Bonuses { get; }
        public decimal Money { get; }
        public double BonusPaymentShareMultiplier { get; }

        public BuyerBalanceState(double bonuses, decimal money, double bonusPaymentShareMultiplier)
        {
            EnsureBonusesRange(bonuses);
            EnsureMoneyRange(money);
            EnsureBonusShareRange(bonusPaymentShareMultiplier);

            Bonuses = bonuses;
            Money = money;
            BonusPaymentShareMultiplier = bonusPaymentShareMultiplier;
        }

        private static void EnsureBonusesRange(double bonuses)
        {
            if (double.IsNaN(bonuses) || double.IsInfinity(bonuses) || bonuses < 0.0)
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

        private static void EnsureBonusShareRange(double bonusPaymentShareMultiplier)
        {
            if (double.IsNaN(bonusPaymentShareMultiplier) ||
                double.IsInfinity(bonusPaymentShareMultiplier) ||
                bonusPaymentShareMultiplier < 0.0 ||
                bonusPaymentShareMultiplier > 0.5)
                throw new ArgumentOutOfRangeException(
                    nameof(bonusPaymentShareMultiplier),
                    "Доля оплаты бонусами должна находиться в диапазоне от 0.0 до 0.5.");
        }
    }
}
