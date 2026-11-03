using System;

namespace classiki
{
    /// <summary>
    /// Баланс покупателя: бонусы, деньги и разрешённая доля оплаты бонусами.
    /// </summary>
    public class BuyerBalance
    {
        private BuyerBalanceState _state;

        public decimal Bonuses => _state.Bonuses;
        public decimal Money => _state.Money;
        public decimal BonusPaymentShareMultiplier => _state.BonusPaymentShareMultiplier;

        public BuyerBalance(decimal bonuses, decimal money, decimal bonusPaymentShareMultiplier)
        {
            _state = new BuyerBalanceState(bonuses, money, bonusPaymentShareMultiplier);
        }

        public BuyerBalanceState ToState() => _state;

        public void Debit(decimal bonusPayment, decimal moneyPayment)
        {
            if (bonusPayment < 0m)
                throw new ArgumentOutOfRangeException(
                    nameof(bonusPayment),
                    "Сумма списания бонусов не может быть отрицательной.");

            if (moneyPayment < 0m)
                throw new ArgumentOutOfRangeException(
                    nameof(moneyPayment),
                    "Сумма списания денег не может быть отрицательной.");

            _state = new BuyerBalanceState(
                _state.Bonuses - bonusPayment,
                _state.Money - moneyPayment,
                _state.BonusPaymentShareMultiplier);
        }
    }
}
