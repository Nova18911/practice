using System;
using System.Text;

namespace task2
{
    internal class Solution2
    {
        /// <summary>
        /// Формирует ромб с диагональю <paramref name="n"/>.
        /// </summary>
        /// <param name="n"> диагональ ромба</param>
        /// <returns>ромб в виде послед-ти символов.</returns>
        /// <exception cref="ArgumentOutOfRangeException">если число меньше или равно 0 или четное.</exception>
        static string GetRhomb(int n)
        {
            if (n <= 0 || n % 2 == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(n),
                    "Ожидается положительное число и нечетное число");
                
            }

            StringBuilder rhomb = new StringBuilder();
            var center = n / 2;
            for (var i = 0; i < n; i++)
            {
                for (var j = 0; j < n; j++)
                {
                    rhomb.Append(
                        Math.Abs(i - center) + Math.Abs(j - center) == center
                        ? "X"
                        : " "
                        );
                }
                rhomb.AppendLine();
            }
            return rhomb.ToString();
        }


        static void Main(string[] args)
        {
            try
            {
                Console.Write("Введите длину диагонали ромба: ");
                int diagonal = Convert.ToInt32(Console.ReadLine());

                string rhomb = GetRhomb(diagonal);
                Console.WriteLine(rhomb);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: введите целое положительное нечётное число.");
            }
        }
    }
}
