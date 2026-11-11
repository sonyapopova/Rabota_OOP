namespace Marketplace.Domain;

public class Product
{
    private int id;
    private string title;


    // 1. string — сокращённая (автоматическая) запись, обязательна присоздании
    public required string Title { get; init; }
    // 2. string — полная запись с проверкой на пустоту
    private string article = string.Empty;
    public string Article
    {
        get => article;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Артикул обязателен",
                nameof(value));
            article = value;
        }
    }
    // 3. int — полная запись, не допускает отрицательных значений
    private int price;
    public int Price
    {
        get => price;
        set 
        {
            if (value < 0) throw new ArgumentOutOfRangeException(nameof(value), "Цена на может быть отрицательной");
            price = value;
        }
    }
    // 4. bool — вычисляемое свойство: true, если Price < 25
    public bool IsBudget => Price < 25;
    // 5. Перегрузки
    public string GetInfo() => GetInfo(true); 
    public string GetInfo(bool includeIntProperty) 
    {
        var info = $"Название: {Title}, Артикул: {Article}, Цена: {Price}";
        if (includeIntProperty)
            info += $", Бюджетный: {(IsBudget ? "да" : "нет")}";
        return info ;
    }
    // 6. Статус меняется только внутри класса
    public string Status { get; private set; } = "В наличии";
    private static readonly string[] AllowedStatuses =
    {
        "В наличии", "Нет в наличии", "Под заказ"
    };
    public void ChangeStatus(string newStatus)
    {
        EnsureStatus(newStatus);
        Status = newStatus;
    }
    private static void EnsureStatus(string value)
    {
        if (Array.IndexOf(AllowedStatuses, value) < 0)
            throw new ArgumentException($"Недопустимый статус: {value}", nameof(value));
    }

    // все что ниже чтобы не ломался fileproductrepository


    public int GetId() => id;

    public string GetTitle() => title;

    public int GetPrice() => price;


}
