using System;
using System.Threading;

namespace classiki
{
    //Начинается. НЕ МОЯ (Ромы) ЧАСТЬ РАБОТЫ, НО БЕЗ ЭТОГО МОЯ НЕ РАБОТАЕТ. ВМЕСТО ТОВАРНОЙ ПОЗИЦИИ
    public class ProductItem
    {
        private static int productItemId = 0;

        public int ID { get; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public DateTime PurchaseDate { get; set; }

        public ProductItem(string name, double price, int quantity)
        {
            if (price < 0)
                throw new ArgumentException("Цена не может быть отрицательной.");

            if (quantity <= 0)
                throw new ArgumentException("Количество должно быть больше нуля.");

            ID = Interlocked.Increment(ref productItemId);
            Name = name;
            Price = price;
            Quantity = quantity;
            PurchaseDate = DateTime.Now;
        }

        public double TotalPrice => Price * Quantity;
    } //Заканчивается. НЕ МОЯ (Ромы) ЧАСТЬ РАБОТЫ, НО БЕЗ ЭТОГО МОЯ НЕ РАБОТАЕТ. ВМЕСТО ТОВАРНОЙ ПОЗИЦИИ
}
