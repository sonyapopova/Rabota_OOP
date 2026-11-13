using System;

namespace Marketplace.Domain;

/// <summary>
/// Сохранённая покупка: позиция корзины с датой приобретения.
/// </summary>
public class ProductItem
{
    public int ProductId { get; }
    public int Quantity { get; }
    public int PricePerUnit { get; }
    public DateTime PurchaseDate { get; }

    /// <summary>Стоимость покупки: цена за единицу × количество.</summary>
    public int Total => PricePerUnit * Quantity;

    public ProductItem(int productId, int quantity, int pricePerUnit)
    {
        if (productId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(productId),
                "Идентификатор товара должен быть положительным.");

        if (quantity <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(quantity),
                "Количество должно быть больше нуля.");

        if (pricePerUnit < 0)
            throw new ArgumentOutOfRangeException(
                nameof(pricePerUnit),
                "Цена за единицу не может быть отрицательной.");

        ProductId = productId;
        Quantity = quantity;
        PricePerUnit = pricePerUnit;
        PurchaseDate = DateTime.Now;
    }
}
