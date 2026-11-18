namespace Marketplace.Domain;

public class Product
{
    public const int MaxPrice = 1_000_000;
    private static int nextId;
    public static int CreatedCount => nextId;
    public static readonly DateTime CatalogStartedAt;
    private readonly DateTime createdAt;
    public int Id { get; }
    public required string Title { get; init; }
    public string Article { get; set; } = string.Empty;
    private int price;
    public int Price
    {
        //TODO: ⚠️ get и set с проверкой value < 0 || value > MaxPrice ⚠️
        get => price;
        set 
        {
            if (value < 0 || value > MaxPrice) throw new ArgumentOutOfRangeException(nameof(value), "Цена не может быть отрицательной и должна быть меньше 1000000");
            price = value;
        }
    }
    public DateTime CreatedAt => createdAt;
    public string Status { get; private set; } = "В наличии";
    static Product()
    {
        //TODO: ⚠️ Задайте CatalogStartedAt (например, DateTime.UtcNow) ⚠️
        CatalogStartedAt = DateTime.Now;
    }
    public Product()
    {
        //TODO: ⚠️ Id = ++nextId; createdAt = DateTime.Now; ⚠️
        Id = ++nextId;
        createdAt = DateTime.Now;
    }
    private static readonly string[] AllowedStatuses =
    {
          "В наличии", "Нет в наличии", "Под заказ"
    };
    //TODO: ⚠️ public void ChangeStatus(string newStatus) с приватной проверкой ⚠️
    public void ChangeStatus(string newStatus)
    {
        EnsureStatus(newStatus);
        Status = newStatus;
    }
    //TODO: ⚠️ private static void EnsureStatus(string value) ⚠️
    private static void EnsureStatus(string value)
    {
        if (Array.IndexOf(AllowedStatuses, value) < 0)
        throw new ArgumentException($"Недопустимый статус: {value}", nameof(value));
    }

    // все что ниже чтобы не ломался fileproductrepository
    public int GetId() => Id;

    public string GetTitle() => Title;

    public int GetPrice() => Price;
}