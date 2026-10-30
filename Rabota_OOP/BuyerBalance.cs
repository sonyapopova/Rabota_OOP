namespace classiki
{
    public struct BuyerBalanceState
    {
        private decimal _bonusPaymentShareMultiplier;
        public decimal _money;
        public decimal Bonuses { get; set;  }
        public decimal Money
        {
            get => _money;
            set
            {
                if (value < 0m)
                    throw new ArgumentOutOfRangeException(nameof(value), "Балланc не может быть меньше 0.0");
                _money = value;
            }
        }
        public decimal BonusPaymentShareMultiplier 
        { 
            get => _bonusPaymentShareMultiplier; 
            set
            {
                if (value < 0.0m || value > 0.5m)
                    throw new ArgumentOutOfRangeException(nameof(value), "Доля оплаты бонусами должна быть от 0.0 до 0.5");
                _bonusPaymentShareMultiplier = value;
            }
        }

        public BuyerBalanceState(decimal bonuses, decimal money, decimal bonusPaymentShareMultiplier)
        {
            Bonuses = bonuses;
            _money = 0m;
            _bonusPaymentShareMultiplier = 0m;
            Money = money;
            BonusPaymentShareMultiplier = bonusPaymentShareMultiplier;
        }
    }
    public class BuyerBalance
    {
        public decimal Bonuses, Money;

        public BuyerBalance(decimal bonuses, decimal money) { Bonuses = bonuses; Money = money; }
    }
}
