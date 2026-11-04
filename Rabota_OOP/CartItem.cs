namespace Marketplace.Domain
{
    //Товарная позиция: товар и его количество в корзине
    public record CartItem(int Id, Product Product, int Quantity)
    {
        public decimal Total => Product.Price * Quantity; // цена × количество
    }
}
