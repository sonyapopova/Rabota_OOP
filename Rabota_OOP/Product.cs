namespace classiki
{
    /// <summary>
    /// Товар: идентификатор, название и цена за единицу.
    /// </summary>
    public class Product
    {
        private int _id;
        private string _title = null!;
        private int _price; 

        public Product(int id, string title, int price)
        {
            title = title.Trim();
            EnsureId(id);
            EnsureTitle(title);
            EnsurePrice(price);

            _id = id;
            _title = title;
            _price = price;
        }

        public int GetId()
        {
            return _id;
        }

        public string GetTitle()
        {
            return _title;
        }

        public int GetPrice()
        {
            return _price;
        }

        public void SetPrice(int price)
        {
            EnsurePrice(price);
            _price = price;
        }

        public string GetInfo()
        {
            return $"Товар №{_id}: {_title} — {_price} руб.";
        }

        private void EnsureId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "Идентификатор товара должен быть положительным.");
            }
        }

        private void EnsureTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException(
                    "Название товара не может быть пустым.",
                    nameof(title));
            }
        }

        private void EnsurePrice(int price)
        {
            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(price),
                    "Цена товара не может быть отрицательной.");
            }
        }
    }
}

/*Выше код чётко по заданию, но задание немного странное: 
цена int, а не decimal; 
геттеры и сеттеры через методы, хотя тут явно лучше свойства подходят.
Так что я (Макаров) бы так написал:


namespace classiki
{
    /// <summary>
    /// Товар: идентификатор, название и цена за единицу.
    /// </summary>
    public class Product
    {
        private string _name = null!;
        private decimal _price;

        public int Id { get; }

        public string Name
        {
            get => _name;
            set
            {
                EnsureName(value);
                _name = value.Trim();
            }
        }

        public decimal Price
        {
            get => _price;
            set
            {
                EnsurePrice(value);
                _price = value;
            }
        }

        public Product(int id, string name, decimal price)
        {
            EnsureId(id);
            
            Id = id;
            Name = name;   
            Price = price; 
        }

        public string GetInfo()
        {
            return $"Товар №{Id}: {Name} — {Price} руб.";
        }

        private static void EnsureId(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(id),
                    "Идентификатор товара должен быть положительным.");
            }
        }

        private static void EnsureName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    "Название товара не может быть пустым.",
                    nameof(name));
            }
        }

        private static void EnsurePrice(decimal price)
        {
            if (price < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(price),
                    "Цена товара не может быть отрицательной.");
            }
        }
    }
}


*/