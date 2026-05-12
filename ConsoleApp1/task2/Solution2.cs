using System;
using System.Text;

namespace task2
{
    internal class Solution2
    {
        static string Rhomb(int n)
        {
            StringBuilder rhomb = new StringBuilder();
            int center = n / 2;
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (Math.Abs(i - center) + Math.Abs(j - center) == center)
                    {
                        rhomb.Append("X");
                    }
                    else
                    {
                        rhomb.Append(" ");
                    }
                }
                rhomb.Append('\n');
            }
            return rhomb.ToString();
        }
        static void Main(string[] args)
        {
            try
            {
                Console.Write("Введите длину диагонали ромба: ");
                int diagonal = Convert.ToInt32(Console.ReadLine());

                if (diagonal < 0 || diagonal % 2 == 0) 
                {
                    Console.WriteLine("Число должно быть положительным и нечетным");
                    return;
                }

                string rhomb = Rhomb(diagonal);
                Console.WriteLine(rhomb);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: введите целое положительное нечётное число.");
            }
        }
    }
}
