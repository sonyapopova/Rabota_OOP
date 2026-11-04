namespace Marketplace.Domain
{
    //родной, у меня без тебя не заработает задачко
    //private static string Serialize(Product product) => $"{product.GetId()};{product.GetTitle()};{product.GetPrice()}";
    //вот эта строчка ^^^ в FileProductRepository должна скопилиться

    //Товар: неизменяемая запись (record сам генерирует конструктор и геттеры)
    public record Product(int Id, string Name, decimal Price)
    {
        public int GetId() => Id;
        public string GetTitle() => Name;
        public int GetPrice() => (int)Price;
    }
}
