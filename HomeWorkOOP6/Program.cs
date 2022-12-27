namespace HomeWorkOOP6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string CommandBuyProduct = "1";
            const string CommandShowInventory = "2";
            const string CommandExit = "3";

            bool isProgramOn = true;

            Seller seller = new("Герцог");
            Customer customer = new("Итан");

            while (isProgramOn)
            {
                Console.Clear();
                Console.WriteLine("Торговец Герцог: What are you buying?");
                Console.WriteLine("Итан: ");
                Console.WriteLine($"{CommandBuyProduct}-Приобрести товары");
                Console.WriteLine($"{CommandShowInventory}-Показать инвентарь");
                Console.WriteLine($"{CommandExit}-Уйти");

                string userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandBuyProduct:
                        seller.Sell(customer);
                        break;
                    case CommandShowInventory:
                        customer.ShowInventory(seller.UserProductChoice);
                        break;
                    case CommandExit:
                        isProgramOn = false;
                        break;
                    default:
                        Console.WriteLine("Герцог: Какое жалкое зрелище");
                        break;
                }
            }
        }
    }

    class Seller
    {
        private List<Product> _products = new();
        public string Name { get; private set; }
        public int UserProductChoice { get; private set; }

        public Seller(string name)
        {
            Name = name;
            _products.Add(new Product(1, "Лечебное зелье", 500, 5));
            _products.Add(new Product(2, "Зелье выносливости", 400, 5));
            _products.Add(new Product(3, "Пистолетные патроны", 300, 200));
            _products.Add(new Product(4, "Патроны для дробовика", 500, 100));
            _products.Add(new Product(5, "Винтовочные патроны", 700, 20));
            _products.Add(new Product(6, "Граната", 600, 2));
            _products.Add(new Product(7, "Отмычка", 500, 10));
            _products.Add(new Product(8, "Коктейль молотова", 700, 4));
        }

        public void Sell(Customer customer)
        {
            Console.Clear();
            Console.WriteLine($"Золото: {customer.Money}");
            Console.WriteLine("Ассортимент товаров: ");

            ShowProducts();

            if (GetProduct(out Product? product, customer))
            {
                customer.AddToInventory(product);
                Console.WriteLine("Покупка успешна");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Ошибка");
            }
        }

        private void ShowProducts()
        {
            for (int i = 0; i < _products.Count; i++)
            {
                Console.WriteLine($"{_products[i].Id}.{_products[i].ProductName}:{_products[i].Cost}-золотых. Доступно:{_products[i].Available}");
            }
        }

        private bool GetProduct(out Product? product, Customer customer)
        {
            product = null;

            Console.WriteLine("Выберите товар:");

            if (int.TryParse(Console.ReadLine(), out int numberProduct))
            {
                Console.WriteLine("Выберите количество товара: ");

                if (int.TryParse(Console.ReadLine(), out int Quantity))
                {
                    UserProductChoice = Quantity;

                    foreach (Product _product in _products)
                    {

                        if (numberProduct == _product.Id && _product.Available >= 1)
                        {
                            product = _product;

                            if (customer.Buy(product, UserProductChoice, customer))
                            {
                                return true;
                            }
                        }
                    }

                    return false;
                }
                else
                {
                    Console.WriteLine("Нужно ввести число.");
                    Console.ReadKey();
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Ошибка");
                product = null;
                return false;
            }
        }
    }

    class Customer
    {
        private List<Product> _productsInInvetory = new();
        public int Money { get; private set; } = 1500;
        public string Name { get; private set; }

        public Customer(string name)
        {
            Name = name;
        }

        public void ShowInventory(int userProductChoice)
        {
            Console.WriteLine($"Золото: {Money}");

            if (_productsInInvetory.Count > 0)
            {
                for (int i = 0; i < _productsInInvetory.Count; i++)
                {
                    Console.WriteLine($"{_productsInInvetory[i].Id}.{_productsInInvetory[i].ProductName}, количество: {userProductChoice}-шт.");
                }
            }
            else
            {
                Console.WriteLine("Инвентарь пуст");
            }

            Console.ReadKey();
        }

        public void AddToInventory(Product product)
        {
            _productsInInvetory.Add(product);
        }

        public bool Buy(Product product, int productCount, Customer customer)
        {
            bool isBuy = product.Available >= productCount && customer.Money >= product.Cost;

            if (isBuy)
            {
                product.Available -= productCount;
                customer.Money -= product.Cost * productCount;
                return true;
            }
            else
            {
                return isBuy;
            }
        }
    }

    class Product
    {
        public int Id { get; private set; }
        public string ProductName { get; private set; }
        public int Cost { get; private set; }
        public int Available { get; set; }

        public Product(int id, string productName, int cost, int available)
        {
            Id = id;
            ProductName = productName;
            Cost = cost;
            Available = available;
        }
    }
}
