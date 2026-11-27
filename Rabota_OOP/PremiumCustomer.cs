using System;

namespace Marketplace.Domain;

public class PremiumCustomer : CustomerProfile
{
    public int DiscountPercent { get; }

    public PremiumCustomer(int discountPercent) : base()
    {
        EnsureDiscountPercent(discountPercent);
        DiscountPercent = discountPercent;
    }

    public string GetPremiumInfo() =>
        $"{base.GetDisplayName()} — скидка {DiscountPercent}%";

    private static void EnsureDiscountPercent(int value)
    {
        if (value < 0 || value > 50)
            throw new ArgumentOutOfRangeException(
                nameof(value),
                "Скидка премиального покупателя должна быть от 0 до 50%.");
    }
}
