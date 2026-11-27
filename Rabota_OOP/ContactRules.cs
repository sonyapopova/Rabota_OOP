#nullable enable
namespace Marketplace.Domain;

public static class ContactRules
{
    public const string UnknownEmail = "почта не указана";

    public static string NormalizeEmail(string? value) =>
        value is null ? UnknownEmail : value.Trim().ToLowerInvariant();

    public static bool LooksLikeEmail(string? value) =>
        value?.Contains('@') == true;
}
