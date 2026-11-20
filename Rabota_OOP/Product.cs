using System;

namespace Marketplace.Domain;

public class Product
{
    public const int MaxPrice = 1_000_000;

    private static int nextId;
    private readonly DateTime createdAt;
    private int price;
    private string article = string.Empty;

    public static int CreatedCount => nextId;

    public static readonly DateTime CatalogStartedAt;

    public int Id { get; }

    public required string Title { get; init; }

    public string Article
    {
        get => article;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException(
                    "Артикул обязателен.",
                    nameof(value));

            article = value;
        }
    }

    public int Price
    {
        get => price;
        set
        {
            if (value < 0 || value > MaxPrice)
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    $"Цена должна быть от 0 до {MaxPrice}.");

            price = value;
        }
    }

    public DateTime CreatedAt => createdAt;

    public bool IsBudget => Price < 25;

    public string Status { get; private set; } = "В наличии";

    private static readonly string[] AllowedStatuses =
    {
        "В наличии",
        "Нет в наличии",
        "Под заказ"
    };

    static Product()
    {
        CatalogStartedAt = DateTime.Now;
    }

    public Product()
    {
        Id = ++nextId;
        createdAt = DateTime.Now;
    }

    public string GetInfo() => GetInfo(true);

    public string GetInfo(bool includeIntProperty)
    {
        string info = $"Название: {Title}, Артикул: {Article}";

        if (includeIntProperty)
            info += $", Цена: {Price}, Бюджетный: {(IsBudget ? "да" : "нет")}";

        return info;
    }

    public void ChangeStatus(string newStatus)
    {
        EnsureStatus(newStatus);
        Status = newStatus;
    }

    private static void EnsureStatus(string value)
    {
        if (Array.IndexOf(AllowedStatuses, value) < 0)
            throw new ArgumentException(
                $"Недопустимый статус: {value}",
                nameof(value));
    }
}