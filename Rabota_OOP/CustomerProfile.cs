namespace Marketplace.Domain;

public class CustomerProfile
{
    private string email = "";
    private int rating;

    public required string FullName { get; init; }
    
    public string Phone { get; set; } = string.Empty;

    public string Email
    {
        get => email;
        set
        {
            EnsureValidEmail(value);
            email = value;
        }
    }

    public int Rating
    {
        get => rating;
        set
        {
            if (value < 0 || value > 5)
            {
                throw new ArgumentOutOfRangeException(nameof(value), "Рейтинг должен находиться в диапазоне от 0 до 5.");
            }
            rating = value;
        }
    }

    public bool IsTrusted => Rating >= 4;

    public string GetInfo()
    {
        return GetInfo(false);
    }

    public string GetInfo(bool includeRating)
    {
        var baseInfo = $"{FullName} ({Email})";
        return includeRating 
            ? $"{baseInfo} — Рейтинг: {Rating}/5" 
            : baseInfo;
    }

    public string GetContactInfo(bool includePhone)
    {
        if (!includePhone || string.IsNullOrWhiteSpace(Phone))
        {
            return $"Email: {Email}";
        }

        return $"Email: {Email}, Телефон: {NormalizePhone(Phone)}";
    }

    private static void EnsureValidEmail(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
        {
            throw new ArgumentException("Некорректный формат email. Адрес должен содержать символ '@'.", nameof(value));
        }
    }

    private static string NormalizePhone(string value)
    {
        return value.Trim();
    }
}
