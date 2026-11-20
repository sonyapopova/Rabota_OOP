using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Marketplace.Domain;

namespace Marketplace.Services;

/// <summary>
/// Формирование чека: номер заказа, строки позиций и итог с НДС.
/// </summary>
public static class ReceiptService
{
    public static string BuildReceipt(IEnumerable<CartItem> items)
    {
        if (items == null)
            throw new ArgumentNullException(nameof(items));

        var availableItems = items
            .Where(item => item.ProductId > 0 && item.Quantity > 0 && item.PricePerUnit >= 0)
            .ToList();

        var receipt = new StringBuilder();
        receipt.AppendLine($"Чек №{OrderNumberGenerator.Next()}");

        foreach (CartItem item in availableItems)
            receipt.AppendLine(PrepareLine(item));

        var checkoutService = new CheckoutService();
        int total = checkoutService.GetTotal(availableItems);
        receipt.Append($"Итого с НДС: {FormatMoney(PriceRules.WithVat(total))}");

        return receipt.ToString();
    }

    private static string PrepareLine(CartItem item) =>
        $"  Товар №{item.ProductId}: {item.Quantity} × {FormatMoney(item.PricePerUnit)} = {FormatMoney(item.Total)}";

    private static string FormatMoney(int amount) =>
        $"{amount} {PriceRules.DefaultCurrency}";
}