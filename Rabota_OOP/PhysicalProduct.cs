namespace Marketplace.Domain;

public class PhysicalProduct : Product
{
    public double WeightKg { get; }
    public string? WarehouseCode { get; set; }

    public PhysicalProduct(double weightKg, string? warehouseCode) : base()
    {
        EnsureWeight(weightKg);
        WeightKg = weightKg;
        WarehouseCode = warehouseCode;
    }

    public string DeliveryInfo => WarehouseCode ?? "Склад не назначен";

    public string GetFullInfo()
    {
        return $"{base.GetDescription()}. \n Вес: {WeightKg} кг. \n Склад: {DeliveryInfo}.";
    }

    private static void EnsureWeight(double weightKg)
    {
        if (weightKg <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(weightKg), 
                "Вес товара должен быть больше нуля.");
        }
    }
}
