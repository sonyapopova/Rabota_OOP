using System;
using System.Collections.Generic;
using System.Linq;

namespace classiki
{
    public class UserProfile
    {
        private readonly List<Contact> _contacts = new List<Contact>();

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
        private readonly List<ProductItem> _purchases = new List<ProductItem>();
        public IReadOnlyList<ProductItem> Purchases => _purchases;
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

        public User User { get; }
        public IReadOnlyList<Contact> Contacts => _contacts;

        //в качестве демонтрации прочих свойств мы решили добавить дату создания профиля (прим. Попова София)
        public DateTime CreatedAt { get; } = DateTime.Now;

        public UserProfile(User user)
        {
            User = user ?? throw new ArgumentNullException(nameof(user));
        }

        public void AddContact(Contact contact)
        {
            if (contact == null)
                throw new ArgumentNullException(nameof(contact));

            if (_contacts.Any(c => c.ContactId == contact.ContactId))
                throw new InvalidOperationException(
                    $"Контакт с ID {contact.ContactId} уже добавлен в профиль.");

            _contacts.Add(contact);
        }

        public bool RemoveContact(int contactId)
        {
            var contact = _contacts.FirstOrDefault(c => c.ContactId == contactId);
            if (contact == null)
                return false;

            return _contacts.Remove(contact);
        }

        public void AddPurchase(ProductItem product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            _purchases.Add(product);
        }

        /// <summary>
        /// Деконструкция профиля: ФИО, логин, количество контактов, денежный и бонусный балансы.
        /// </summary>
        public void Deconstruct(
            out string fullName,
            out string login,
            out int contactsCount,
            out decimal moneyBalance,
            out double bonusBalance)
        {
            (fullName, login) = User;
            contactsCount = _contacts.Count;
            moneyBalance = MoneyBalance;
            bonusBalance = BonusBalance;
        }
    }
}
