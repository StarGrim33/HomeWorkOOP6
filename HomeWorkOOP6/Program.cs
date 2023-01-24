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

                string? userInput = Console.ReadLine();

                switch (userInput)
                {
                    case CommandBuyProduct:
                        seller.Sell(customer);
                        break;

                    case CommandShowInventory:
                        customer.ShowProducts();
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

    abstract class Human
    {
        protected List<Stack> Stack = new();

        public Human(string name)
        {
            Name = name;
        }

        public string Name { get; protected set; }

        public virtual void ShowProducts()
        {
            for (int i = 0; i < Stack.Count; i++)
            {
                var stack = Stack[i];
                Console.Write($"{i + 1}.");
                stack.ShowInfo();
                Console.WriteLine();
            }
        }
    }

    class Seller : Human
    {
        public Seller(string name) : base(name)
        {
            Name = name;
            Stack = new List<Stack>()
            {
                new Stack(new Product("Лечебное зелье", 500), 5),
                new Stack(new Product("Зелье выносливости", 400), 5),
                new Stack(new Product("Пистолетные патроны", 300), 200),
                new Stack(new Product("Патроны для дробовика", 500), 100),
                new Stack(new Product("Винтовочные патроны", 700), 20),
                new Stack(new Product("Граната", 600), 2),
                new Stack(new Product("Отмычка", 500), 10),
                new Stack(new Product("Коктейль молотова", 700), 4),
            };
        }

        public void Sell(Customer customer)
        {
            Console.Clear();
            Console.WriteLine($"Золото: {customer.Money}");
            Console.WriteLine("Ассортимент товаров: ");

            ShowProducts();

            if (TryTakeProduct(out Stack stack, customer))
            {
                customer.Buy(stack);
                Console.WriteLine("Покупка успешна");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Ошибка");
            }
        }

        private bool TryTakeProduct(out Stack stack, Customer customer)
        {
            stack = null;

            Console.WriteLine("Выберите товар:");

            if (int.TryParse(Console.ReadLine(), out int numberProduct) == false)
            {
                Console.WriteLine("Ошибка");
                return false;
            }

            if (numberProduct < 0 || numberProduct >= Stack.Count)
            {
                Console.WriteLine("Таких продуктов нет");
                return false;
            }

            Console.WriteLine("Выберите количество товара: ");

            if (int.TryParse(Console.ReadLine(), out int quantity) == false)
            {
                Console.WriteLine("Нужно ввести число.");
                return false;
            }

            if (Stack[numberProduct].TryGetProducts(out stack, quantity, customer) == false)
            {
                Console.WriteLine("Недостаточно количество");
                return false;
            }

            return true;
        }
    }

    class Customer : Human
    {
        public Customer(string name) : base(name) { }

        public int Money { get; private set; } = 1500;

        public override void ShowProducts()
        {
            Console.WriteLine($"Золото: {Money}");

            if (Stack.Count > 0)
            {
                for (int i = 0; i < Stack.Count; i++)
                {
                    var stack = Stack[i];
                    Console.Write($"{i + 1}.");
                    stack.ShowInfo();
                    Console.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("Инвентарь пуст");
            }

            Console.ReadKey();
        }

        public void Buy(Stack stack)
        {
            foreach (Stack currentStack in Stack)
            {
                if (currentStack.Product == stack.Product)
                {
                    currentStack.AddQuantity(stack.Quantity);
                    return;
                }
            }

            Money -= stack.Product.Cost * stack.Quantity;
            Stack.Add(stack);
        }
    }

    class Product
    {
        public Product(string productName, int cost)
        {
            Name = productName;
            Cost = cost;
        }

        public string Name { get; private set; }
        public int Cost { get; private set; }
    }

    class Stack
    {
        public Stack(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public Product Product { get; }
        public int Quantity { get; private set; }

        public bool TryGetProducts(out Stack stack, int quantity, Customer customer)
        {
            stack = null;

            if (quantity < 0)
            {
                return false;
            }

            if (quantity > Quantity)
            {
                return false;
            }

            if (customer.Money < Product.Cost * quantity)
            {
                Console.WriteLine("Недостаточно денег");
                Console.ReadKey();
                return false;
            }

            Quantity -= quantity;
            stack = new Stack(Product, quantity);
            return true;
        }

        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
        }

        public void ShowInfo()
        {
            Console.Write($"{Product.Name}, стоимость: {Product.Cost}, количество: {Quantity}-шт.");
        }
    }
}