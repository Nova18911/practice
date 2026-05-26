namespace task_class
{
    internal class Solution
    {
        class Product
        {
            public string Name { get; }
            public string Manufacturer { get; }
            public double Price { get; }
            public DateOnly ExpirationDate { get; }
            public DateOnly ProductionDate { get; }

            public Product(string name, string manufacturer, double price,
                           DateOnly expirationDate, DateOnly productionDate)
            {
                if (string.IsNullOrWhiteSpace(name))
                    throw new ArgumentNullException(nameof(name), "Наименование не может быть пустым.");
                if (string.IsNullOrWhiteSpace(manufacturer))
                    throw new ArgumentNullException(nameof(manufacturer), "Производитель не может быть пустым.");
                if (price <= 0)
                    throw new ArgumentOutOfRangeException(nameof(price), "Цена должна быть положительной.");
                if (productionDate > DateOnly.FromDateTime(DateTime.Now))
                    throw new ArgumentOutOfRangeException(nameof(productionDate), "Дата производства не может быть в будущем.");
                if (expirationDate <= productionDate)
                    throw new ArgumentOutOfRangeException(nameof(expirationDate), "Срок годности должен быть позже даты производства.");

                Name = name;
                Manufacturer = manufacturer;
                Price = price;
                ExpirationDate = expirationDate;
                ProductionDate = productionDate;
            }

            public override string ToString() =>
                $"Наименование: {Name}\n" +
                $"Производитель: {Manufacturer}\n" +
                $"Цена: {Price:0.00} руб.\n" +
                $"Дата произв.: {ProductionDate}\n" +
                $"Срок годности: {ExpirationDate}";
        }

        /// <summary>
        /// Считывает значение из консоли и преобразует его в указанный тип.
        /// </summary>
        /// <typeparam name="T">тип, к которому приводится введённая строка.</typeparam>
        /// <param name="prompt">сообщение-подсказка для пользователя.</param>
        /// <returns>введённое значение, приведённое к типу <typeparamref name="T"/>.</returns>
        static T ReadValue<T>(string prompt) where T : IConvertible
        {
            Console.Write(prompt);
            return (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
        }

        /// <summary>
        /// Считывает дату из консоли в формате гггг-мм-дд.
        /// </summary>
        /// <param name="prompt">сообщение-подсказка для пользователя.</param>
        /// <returns>введённая дата типа <see cref="DateOnly"/>.</returns>
        /// <exception cref="ArgumentException">если формат даты неверен.</exception>
        static DateOnly ReadDate(string prompt)
        {
            Console.Write(prompt);
            if (!DateOnly.TryParse(Console.ReadLine(), out DateOnly date))
                throw new ArgumentException("Неверный формат даты (ожидается гггг-мм-дд).");
            return date;
        }

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Введите данные о продукте:");

                string name = ReadValue<string>("Наименование: ");
                string manufacturer = ReadValue<string>("Производитель: ");
                double price = ReadValue<double>("Цена: ");
                DateOnly productionDate = ReadDate("Дата производства (гггг-мм-дд): ");
                DateOnly expirationDate = ReadDate("Срок годности (гггг-мм-дд): ");

                Product product = new Product(name, manufacturer, price, expirationDate, productionDate);

                Console.WriteLine("\nИнформация о продукте:");
                Console.WriteLine(product);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\nОшибка ввода: {ex.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("\nОшибка: введите корректные значения.");
            }
        }
    }
}