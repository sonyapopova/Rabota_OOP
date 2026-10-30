using System;

namespace classiki
{
    public class PaymentService
    {
        public bool Pay(BuyerBalance buyer, decimal price)
        {
            // Бонусами списываем не больше разрешённых 10% и не больше, чем есть у покупателя
            decimal bonusPayment = Math.Min(buyer.Bonuses, price * 0.10m);

            // Остаток — с денежного баланса
            decimal moneyPayment = price - bonusPayment;

            if (buyer.Money < moneyPayment)
            {
                Console.WriteLine("Недостаточно средств");
                return false;
            }

            buyer.Bonuses -= bonusPayment;
            buyer.Money -= moneyPayment;

            Console.WriteLine($"Списано бонусами: {bonusPayment} УЕ, деньгами: {moneyPayment} УЕ");
            return true;
        }
    }
}
