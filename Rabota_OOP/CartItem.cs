using System;

namespace Marketplace.Domain;

/// <summary>
/// Позиция корзины: товар, количество и цена за единицу.
/// Данные позиции после создания не меняются, поэтому тип объявлен как readonly struct,
/// а стоимость позиции считается вычисляемым свойством.
/// </summary>
public readonly struct CartItem
{
    public int ProductId { get; }
    public int Quantity { get; }
    public int PricePerUnit { get; }

    /// <summary>Стоимость позиции: цена за единицу × количество.</summary>
    public int Total => PricePerUnit * Quantity;

    public CartItem(int productId, int quantity, int pricePerUnit)
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
    }
}
