using System;
using System.Collections.Generic;
using System.Linq;

namespace collections_task
{
    internal class Solution
    {
        static T ReadValue<T>(string prompt) where T : IConvertible
        {
            Console.Write(prompt);
            return (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
        }

        static void Main(string[] args)
        {
            SmartStack<string> stack = new();

            var menu = """
                1. Push          — добавить элемент
                2. PushRange     — добавить несколько
                3. Pop           — извлечь элемент
                4. Peek          — посмотреть вершину
                5. Contains      — найти элемент
                6. Display       — показать стек
                7. Count/Capacity
                8. Элемент по глубине
                0. Выход
                """;

            while (true)
            {
                Console.WriteLine(menu);
                Console.Write("Выберите действие: ");

                try
                {
                    switch (Console.ReadLine())
                    {
                        case "1":
                            string value = ReadValue<string>("Введите элемент: ");
                            stack.Push(value);
                            Console.WriteLine($"Элемент '{value}' добавлен.");
                            break;

                        case "2":
                            var items = ReadValue<string>("Введите элементы через запятую: ")
                                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                                .ToList();
                            stack.PushRange(items);
                            Console.WriteLine($"Добавлено {items.Count} элементов.");
                            break;

                        case "3":
                            Console.WriteLine($"Извлечён элемент: '{stack.Pop()}'");
                            break;

                        case "4":
                            Console.WriteLine($"Вершина стека: '{stack.Peek()}'");
                            break;

                        case "5":
                            string search = ReadValue<string>("Введите элемент для поиска: ");
                            Console.WriteLine(stack.Contains(search) ? "Элемент найден." : "Элемент не найден.");
                            break;

                        case "6":
                            stack.Display();
                            break;

                        case "7":
                            Console.WriteLine($"Количество: {stack.Count}, Ёмкость: {stack.Capacity}");
                            break;

                        case "8":
                            int depth = ReadValue<int>("Введите глубину (0 — вершина): ");
                            Console.WriteLine($"Элемент на глубине {depth}: '{stack[depth]}'");
                            break;

                        case "0":
                            Console.WriteLine("Выход.");
                            return;

                        default:
                            Console.WriteLine("Неизвестная команда.");
                            break;
                    }
                }
                catch (InvalidOperationException ex) { Console.WriteLine($"Ошибка операции: {ex.Message}"); }
                catch (ArgumentOutOfRangeException ex) { Console.WriteLine($"Ошибка индекса: {ex.Message}"); }
                catch (Exception ex) { Console.WriteLine($"Непредвиденная ошибка: {ex.Message}"); }
            }
        }
    }
}