using System;
using System.Collections.Generic;
using System.Linq;
using Marketplace.Domain;

namespace Marketplace.Services;

/// <summary>
/// Сервис оформления заказа: работает только с открытыми свойствами CartItem.
/// Класс не хранит состояния — все методы можно вызывать независимо.
/// </summary>
public class CheckoutService
{
    /// <summary>Сумма стоимостей всех доступных позиций.</summary>
    public int GetTotal(IEnumerable<CartItem> items)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));

        return items.Where(IsAvailable).Sum(item => item.Total);
    }

    /// <summary>Позиции, цена за единицу которых меньше заданной.</summary>
    public IReadOnlyList<CartItem> FindCheaperThan(IEnumerable<CartItem> items, int maxPrice)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));

        if (maxPrice < 0)
            throw new ArgumentOutOfRangeException(
                nameof(maxPrice),
                "Максимальная цена не может быть отрицательной.");

        return items
            .Where(IsAvailable)
            .Where(item => item.PricePerUnit < maxPrice)
            .ToList();
    }

    /// <summary>Короткая сводка по корзине.</summary>
    public string GetReport(IEnumerable<CartItem> items) => GetReport(items, false);

    /// <summary>Сводка по корзине; при includeLines добавляются строки позиций.</summary>
    public string GetReport(IEnumerable<CartItem> items, bool includeLines)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));

        var availableItems = items.Where(IsAvailable).ToList();

        if (availableItems.Count == 0)
            return "Корзина пуста.";

        string report =
            $"Позиций: {availableItems.Count}; " +
            $"единиц товара: {availableItems.Sum(item => item.Quantity)}; " +
            $"итого: {GetTotal(availableItems)} котокоинов.";

        if (!includeLines)
            return report;

        foreach (var item in availableItems)
            report += Environment.NewLine + PrepareLine(item);

        return report;
    }

    /// <summary>Доступна ли позиция для покупки (количество задано положительным числом).</summary>
    private bool IsAvailable(CartItem item) => item.Quantity > 0 && item.PricePerUnit >= 0;

    /// <summary>Строка позиции для отчёта или чека.</summary>
    private string PrepareLine(CartItem item) =>
        $"  Товар №{item.ProductId}: {item.Quantity} × {item.PricePerUnit} = {item.Total} котокоинов";
}
