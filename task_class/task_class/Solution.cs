using System.Data;

namespace task_class
{
    internal class Solution
    {
        class Product
        {
            private string _name;
            private string _manufacturer;
            private double _price;
            private DateOnly _expirationDate;
            private DateOnly _productionDate;

            public string Name => _name;
            public string Manufacturer => _manufacturer;
            public double Price => _price;
            public DateOnly ExpirationDate => _expirationDate;
            public DateOnly ProductionDate => _productionDate;

            public Product(string name, string manufacturer, double price, DateOnly expirationDate, DateOnly productionDate)
            {
                if (string.IsNullOrEmpty(name)) 
                {
                    throw new ArgumentException("Наименование не может быть пустым");
                }

                if (string.IsNullOrEmpty(manufacturer))
                {
                    throw new ArgumentException("Производитель не может быть пустым");
                }

                if (price <= 0)
                {
                    throw new ArgumentException("Цена должна быть положительной");
                }

                if (productionDate > DateOnly.FromDateTime(DateTime.Now))
                {
                    throw new ArgumentException("Дата производства не может быть в будущем");
                }

                if (expirationDate <= productionDate)
                {
                    throw new ArgumentException("Срок годности должен быть позже даты производства");
                }

                _name = name;
                _manufacturer = manufacturer;
                _price = price;
                _expirationDate = expirationDate;
                _productionDate = productionDate;
            }

            public override string ToString()
            {
                return $"Наименование: {_name}\nПроизводитель: {_manufacturer}\nЦена: {_price:0.00} руб.\n" +
                    $"Дата произв.: {_productionDate}\nСрок годности: {_expirationDate}";
            }
        }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Введите данные о продукте:");

                Console.Write("Наименование: ");
                string name = Console.ReadLine();

                Console.Write("Производитель: ");
                string manufacturer = Console.ReadLine();

                Console.Write("Цена: ");
                if (!double.TryParse(Console.ReadLine(), out double price))
                {
                    throw new ArgumentException("Неверный формат цены");
                }

                Console.Write("Дата производства (гггг-мм-дд): ");
                if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly productionDate))
                {
                    throw new ArgumentException("Неверный формат даты производства");
                }

                Console.Write("Срок годности (гггг-мм-дд): ");
                if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly expirationDate))
                {
                    throw new ArgumentException("Неверный формат срока годности");
                }

                Product product = new Product(name, manufacturer, price, expirationDate, productionDate);

                Console.WriteLine("\nИнформация о продукте:");
                Console.WriteLine(product.ToString());
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\nОшибка ввода: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nНепредвиденная ошибка: {ex.Message}");
            }
        }
    }
}
