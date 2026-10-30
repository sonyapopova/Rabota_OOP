namespace classiki
{
    //Товарная позиция: товар и его количество в корзине
    public record CartItem(Product Product, int Quantity)
    {
        public decimal Total => Product.Price * Quantity; // цена × количество
    }
}
