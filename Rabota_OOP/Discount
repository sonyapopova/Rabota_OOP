using System;

namespace Marketplace.Domain;

public class Discount
{
    public required string Name { get; init; }
    public DateTime ValidUntil { get; init; }

    private decimal percent;
    public decimal Percent
    {
        get => percent;
        set { EnsurePercent(value); percent = value; }
    }

    private int minSum;
    public int MinSum
    {
        get => minSum;
        set => minSum = value >= 0
            ? value
            : throw new ArgumentOutOfRangeException(nameof(value), "MinSum должен быть >= 0.");
    }

    public bool IsActive => ValidUntil > DateTime.Now;

    public int Calculate(int total) =>
        total < MinSum ? total : ApplyDiscount(total, Percent);

    public int Calculate(int total, decimal percent)
    {
        EnsurePercent(percent);
        return ApplyDiscount(total, percent);
    }

    private static int ApplyDiscount(int total, decimal percent) =>
        (int)Math.Round(total * (1 - percent / 100m));

    private static void EnsurePercent(decimal value)
    {
        if (value < 0 || value > 100)
            throw new ArgumentOutOfRangeException(nameof(value), "Percent должен быть 0..100.");
    }
}
