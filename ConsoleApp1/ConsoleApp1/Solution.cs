using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    internal class Solution
    {
        /// <summary>
        /// Вычисляет начисление процентов на вклад за каждый год.
        /// </summary>
        /// <param name="initialDeposit">начальная сумма вклада.</param>
        /// <param name="years">количество лет.</param>
        /// <param name="interestRate">годовая процентная ставка (в процентах).</param>
        /// <returns>список кортежей (номер года, сумма на конец года).</returns>
        /// <exception cref="ArgumentOutOfRangeException">если любой из параметров меньше или равен 0.</exception>
        static List<(int Year, double Amount)> GetInterestCalculation(double initialDeposit, int years, int interestRate)
        {
            if (initialDeposit <= 0)
                throw new ArgumentOutOfRangeException(nameof(initialDeposit),
                    "Начальный вклад должен быть больше 0.");

            if (years <= 0)
                throw new ArgumentOutOfRangeException(nameof(years),
                    "Количество лет должно быть больше 0.");

            if (interestRate <= 0)
                throw new ArgumentOutOfRangeException(nameof(interestRate),
                    "Годовая процентная ставка должна быть больше 0.");

            List<(int Year, double Amount)> result = new List<(int, double)>();
            double currentAmount = initialDeposit;

            for (int i = 1; i <= years; i++)
            {
                currentAmount *= 1 + interestRate / 100.0;
                result.Add((i, currentAmount));
            }

            return result;
        }

        /// <summary>
        /// Считывает значение из консоли и преобразует его в указанный тип.
        /// </summary>
        /// <typeparam name="T">тип, к которому приводится введённая строка.</typeparam>
        /// <param name="prompt">сообщение-подсказка для пользователя.</param>
        /// <returns>введённое значение, приведённое к типу <typeparamref name="T"/>.</returns>
        static T ReadValue<T>(string message) where T : IConvertible
        {
            Console.Write(message);
            return (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
        }

        static void Main(string[] args)
        {
            try
            {
                double initialDeposit = ReadValue<double>("Введите начальный вклад: ");
                int years = ReadValue<int>("Введите количество лет: ");
                int interestRate = ReadValue<int>("Введите годовую процентную ставку: ");

                List<(int Year, double Amount)> report = GetInterestCalculation(initialDeposit, years, interestRate);

                foreach (var (year, amount) in report)
                {
                    Console.WriteLine($"Год {year}: {amount:0.00} руб.");
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (Exception)
            {
                Console.WriteLine("Ошибка: введите корректные числовые значения.");
            }
        }
    }
}