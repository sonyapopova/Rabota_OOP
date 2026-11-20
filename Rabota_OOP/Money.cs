using System;

namespace Marketplace.Domain;

public readonly struct Money
{
    public const decimal MaxAmount = 10_000_000m;
    public static readonly Money Zero = new(0m, "RUB");

    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency)
    {
        EnsureAmount(amount);
        EnsureCurrency(currency);
        Amount = amount;
        Currency = currency;
    }

    public static Money FromRubles(decimal amount) => new(amount, "RUB");

    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Разные валюты.");

        return new(Amount + other.Amount, Currency);
    }

    private static void EnsureAmount(decimal amount)
    {
        if (amount < 0m || amount > MaxAmount)
            throw new ArgumentOutOfRangeException(nameof(amount));
    }

    private static void EnsureCurrency(string currency)
    {
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException(nameof(currency));
    }
}
