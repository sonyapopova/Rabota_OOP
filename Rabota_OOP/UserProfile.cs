using System;
using System.Collections.Generic;

namespace classiki
{
    public class UserProfile
    {

        //Рома ОТСЮДА
        private double bonusBalance = 0.0;
        private double personalDiscount = 1.0;
        private double bonusPaymentLimit = 0.0;

        //Баланс бонусов: не может быть меньше 0
        public double BonusBalance
        {
            get => bonusBalance;
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value) || value < 0.0)
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        "Баланс бонусов не может быть отрицательным.");

                bonusBalance = value;
            }
        }

        //Множитель цены после скидки:
        //0.01 — скидка 99%, 1.00 — скидки нет
        public double PersonalDiscount
        {
            get => personalDiscount;
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value) ||
                    value < 0.01 || value > 1.00)
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        "Персональная скидка должна находиться в диапазоне от 0.01 до 1.00.");

                personalDiscount = value;
            }
        }

        //Максимальная доля итоговой цены, которую разрешено оплатить бонусами
        public double BonusPaymentLimit
        {
            get => bonusPaymentLimit;
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value) ||
                    value < 0.0 || value > 0.5)
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        "Доля оплаты бонусами должна находиться в диапазоне от 0.0 до 0.5.");

                bonusPaymentLimit = value;
            }
        }

        //Совершённые покупки
        public List<ProductItem> Purchases { get; set; } =
            new List<ProductItem>();
        //Рома ДО СЮДА

        //Начинается. НЕ МОЯ (Ромы) ЧАСТЬ РАБОТЫ, НО БЕЗ ЭТОГО МОЯ НЕ РАБОТАЕТ. 
        //Баланс денег. 
        private decimal moneyBalance = 0m;
        public decimal MoneyBalance
        {
            get => moneyBalance;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        "Денежный баланс не может быть отрицательным.");

                moneyBalance = value;
            }
        }
        //Заканчивается. НЕ МОЯ (Ромы) ЧАСТЬ РАБОТЫ, НО БЕЗ ЭТОГО МОЯ НЕ РАБОТАЕТ. 

        public User User { get; set; }
        public List<Contact> Contacts { get; set; } = new List<Contact>();

        //в качестве демонтрации прочих свойств мы решили добавить дату создания профиля (прим. Попова София)
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public UserProfile(User user)
        {
            User = user;
        }
    }
}
