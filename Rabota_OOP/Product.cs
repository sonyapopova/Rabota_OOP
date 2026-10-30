namespace classiki
{
    //Товар: неизменяемая запись (record сам генерирует конструктор и геттеры)
    public record Product(string Name, decimal Price);
}
