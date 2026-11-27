#nullable enable
using System.Linq;
using CustomerId = System.Int32;
using static Marketplace.Domain.ContactRules;

namespace Marketplace.Domain;

public class CustomerProfile
{
    private string? email;
    private int rating;

    public CustomerId Id { get; init; }
    public required string FullName { get; init; }
    public string? MiddleName { get; set; }
    public string? Phone { get; set; }

    public string? Email
    {
        get => email;
        set { EnsureValidEmail(value); email = value; }
    }

    public int Rating
    {
        get => rating;
        set { EnsureRating(value); rating = value; }
    }

    public bool IsTrusted => Rating >= 4;

    public string GetDisplayName() =>
        $"{FullName} {MiddleName ?? string.Empty}".Trim();

    public string GetEmail() => Email ?? UnknownEmail;

    public void EnsureEmail() => Email ??= UnknownEmail;

    public string TryNormalizeEmail() => NormalizeEmail(Email);

    public string GetInfo() => GetInfo(true);

    public string GetInfo(bool includeRating)
    {
        var baseInfo = $"{FullName} ({GetEmail()})";
        return includeRating
            ? $"{baseInfo} — Рейтинг: {Rating}/5"
            : baseInfo;
    }

    public string GetContactInfo(bool includePhone)
    {
        if (!includePhone || string.IsNullOrWhiteSpace(Phone))
            return $"Email: {GetEmail()}";

        return $"Email: {GetEmail()}, Телефон: {NormalizePhone(Phone)}";
    }

    private static void EnsureRating(int value)
    {
        if (value < 0 || value > 5)
            throw new ArgumentOutOfRangeException(
                nameof(value), "Рейтинг должен быть от 0 до 5.");
    }

    private static void EnsureValidEmail(string? value)
    {
        if (string.IsNullOrEmpty(value) || value == UnknownEmail) return;
        if (!value.Contains('@'))
            throw new ArgumentException(
                "Некорректный формат email. Адрес должен содержать символ '@'.",
                nameof(value));
    }

    private static string NormalizePhone(string value)
    {
        string trimmed = value.Trim();
        string digits = new(trimmed.Where(char.IsDigit).ToArray());

        if (digits.Length < 7 || digits.Length > 15)
            throw new ArgumentException(
                "Телефон должен содержать от 7 до 15 цифр.",
                nameof(value));

        return trimmed.StartsWith('+') ? "+" + digits : digits;
    }
}
