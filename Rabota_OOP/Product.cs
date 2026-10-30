namespace classiki
{
    //Товар: неизменяемая запись (record сам генерирует конструктор и геттеры)
    public record Product(int Id, string Name, decimal Price);
}
