using System;

namespace Marketplace.Domain;

public class Product
{
    private int price;
    private string article = string.Empty;

    public int Id { get; init; }

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
            if (value < 0)
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "Цена товара не может быть отрицательной.");

            price = value;
        }
    }

    public bool IsBudget => Price < 25;

    public string Status { get; private set; } = "В наличии";

    private static readonly string[] AllowedStatuses =
    {
        "В наличии",
        "Нет в наличии",
        "Под заказ"
    };

    public string GetInfo() => GetInfo(true);

    public string GetInfo(bool includeIntProperty)
    {
        string info = $"Название: {Title}, Артикул: {Article}, Цена: {Price}";

        if (includeIntProperty)
            info += $", Бюджетный: {(IsBudget ? "да" : "нет")}";

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
