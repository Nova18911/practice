using System.Text;

namespace ConsoleApp1
{
    internal class Solution
    {
        static string InterestCalculation(double initialDeposit, int years, int interestRate) 
        {
            StringBuilder result = new StringBuilder();
            double currentAmount = initialDeposit;
            for (int i = 1; i <= years; i++)
            {
                currentAmount = currentAmount * (1 + interestRate / 100.0);
                initialDeposit = currentAmount;
                result.AppendLine($"Год {i}: {currentAmount:0.00} руб.");
            }

            return result.ToString().TrimEnd();
        }

        static void Main(string[] args)
        {
            Console.Write("Введите начальный вклад: ");
            double initialDeposit = Convert.ToDouble(Console.ReadLine());

            Console.Write("Введите количество лет: ");
            int years = Convert.ToInt32(Console.ReadLine());

            Console.Write("Введите годовую процентную ставку: ");
            int interestRate = Convert.ToInt32(Console.ReadLine());

            if (initialDeposit < 0) 
            {
                Console.WriteLine("Начальный вклад должен быть больше 0");
                return;
            }

            if (years < 0)
            {
                Console.WriteLine("Количество лет должно быть больше 0");
                return;
            }

            if (interestRate < 0) 
            { 
                Console.WriteLine("Годовая процентная ставка должна быть больше 0");
                return;
            }

            string report = InterestCalculation(initialDeposit, years, interestRate);
            Console.WriteLine(report);
        }
    }
}
