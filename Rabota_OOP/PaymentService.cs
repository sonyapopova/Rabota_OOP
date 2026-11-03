using System;

namespace classiki
{
    /// <summary>
    /// Сервис оплаты: рассчитывает стоимость корзины с учётом бонусов
    /// и денежного баланса покупателя.
    /// </summary>
    public class PaymentService
    {
        public bool PayForPurchase(UserProfile profile, CartItem item, out string message)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            if (item == null)
                throw new ArgumentNullException(nameof(item));

            Cart cart = new Cart(profile.User.UserId);
            cart.Add(item);

            return PayForPurchase(profile, cart, out message);
        }

        public bool PayForPurchase(UserProfile profile, Cart cart, out string message)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            if (cart == null)
                throw new ArgumentNullException(nameof(cart));

            EnsureCartOwner(profile, cart);

            if (cart.IsEmpty)
                throw new InvalidOperationException("Корзина пуста: нечего оплачивать.");

            CartPrice price = cart.Calculate(profile.PersonalDiscount);

            return TryPay(profile, cart, price, out message);
        }

        /// <summary>
        /// Сколько денег нужно доплатить за корзину при текущем балансе бонусов
        /// (без учёта возможного пополнения).
        /// </summary>
        public decimal CalculateMoneyPayment(UserProfile profile, Cart cart)
        {
            if (profile == null)
                throw new ArgumentNullException(nameof(profile));

            if (cart == null)
                throw new ArgumentNullException(nameof(cart));

            EnsureCartOwner(profile, cart);

            if (cart.IsEmpty)
                throw new InvalidOperationException("Корзина пуста: нечего оплачивать.");

            CartPrice price = cart.Calculate(profile.PersonalDiscount);
            BuyerBalanceState balance = CreateBuyerBalance(profile).ToState();

            return CalculateMoneyPayment(price, balance);
        }

        private static void EnsureCartOwner(UserProfile profile, Cart cart)
        {
            if (cart.UserId != profile.User.UserId)
                throw new InvalidOperationException(
                    "Корзина принадлежит другому пользователю.");
        }

        private static BuyerBalance CreateBuyerBalance(UserProfile profile)
        {
            return new BuyerBalance(
                profile.BonusBalance,
                profile.MoneyBalance,
                profile.BonusPaymentLimit);
        }

        private static decimal CalculateBonusPayment(CartPrice price, BuyerBalanceState balance)
        {
            //Максимальная сумма, которую разрешено оплатить бонусами
            decimal maxBonusPayment = Math.Floor(
                price.FinalPrice * balance.BonusPaymentShareMultiplier);

            //Фактическая сумма оплаты бонусами
            return Math.Min(maxBonusPayment, balance.Bonuses);
        }

        private static decimal CalculateMoneyPayment(CartPrice price, BuyerBalanceState balance)
        {
            //Сумма, которую необходимо оплатить деньгами
            return price.FinalPrice - CalculateBonusPayment(price, balance);
        }

        private static bool TryPay(UserProfile profile, Cart cart, CartPrice price, out string message)
        {
            BuyerBalance buyer = CreateBuyerBalance(profile);
            BuyerBalanceState balance = buyer.ToState();

            decimal bonusPayment = CalculateBonusPayment(price, balance);
            decimal moneyPayment = CalculateMoneyPayment(price, balance);
            int positionsCount = cart.PositionsCount;

            // Проверяем достаточность денежных средств.
            // При нехватке средств корзина и балансы остаются без изменений.
            if (balance.Money < moneyPayment)
            {
                decimal missing = moneyPayment - balance.Money;

                message = BuildPaymentMessage(
                    profile, cart, price, bonusPayment, moneyPayment, missing, false, positionsCount);

                return false;
            }

            //Списываем бонусы и деньги
            buyer.Debit(bonusPayment, moneyPayment);
            profile.MoneyBalance = buyer.Money;
            profile.BonusBalance = buyer.Bonuses;

            //Добавляем покупки в историю
            foreach (var item in cart.Items)
                profile.AddPurchase(new ProductItem(item.Product, item.Quantity));

            //Опустошаем корзину: оплаченные позиции в ней больше не нужны
            cart.Clear();

            message = BuildPaymentMessage(
                profile, cart, price, bonusPayment, moneyPayment, 0m, true, positionsCount);

            return true;
        }

        private static string BuildPaymentMessage(
            UserProfile profile,
            Cart cart,
            CartPrice price,
            decimal bonusPayment,
            decimal moneyPayment,
            decimal missing,
            bool success,
            int positionsCount)
        {
            if (!success)
            {
                return
                    "Недостаточно денежных средств для оплаты покупки.\n" +
                    $"Требуется деньгами: {moneyPayment:F2} котокоинов\n" +
                    $"На балансе: {profile.MoneyBalance:F2} котокоинов\n" +
                    $"Не хватает: {missing:F2} котокоинов\n" +
                    "Корзина и баланс не изменены.";
            }

            return
                "Покупка успешно оплачена!\n" +
                $"Позиций оплачено: {positionsCount}\n" +
                $"Стоимость до скидки: {price.OriginalPrice:F2} котокоинов\n" +
                $"Размер скидки: {price.DiscountAmount:F2} котокоинов\n" +
                $"Стоимость со скидкой: {price.FinalPrice:F2} котокоинов\n" +
                $"Оплачено бонусами: {bonusPayment:F2}\n" +
                $"Оплачено деньгами: {moneyPayment:F2} котокоинов\n" +
                $"Корзина очищена: товаров {cart.PositionsCount}, итоговая цена {cart.TotalPrice:F2} котокоинов";
        }
    }
}
