using System;
using System.Collections;
using System.Collections.Generic;

namespace collections_task
{
    public class SmartStack<T> : IEnumerable<T>
    {
        private T[] _items;
        private int _count;
        private const int DefaultCapacity = 4;

        public SmartStack()
        {
            _items = new T[DefaultCapacity];
            _count = 0;
        }

        public SmartStack(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentException("Емкость не может быть отрицательной", nameof(capacity));
            }

            _items = new T[capacity];
            _count = 0;
        }

        public SmartStack(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            var list = new List<T>();
            foreach (var item in collection)
            {
                list.Add(item);
            }

            _items = new T[list.Count];
            _count = list.Count;

            for (int i = 0; i < list.Count; i++)
            {
                _items[i] = list[list.Count - 1 - i];
            }
        }

        public void Push(T item)
        {
            if (_count == _items.Length)
            {
                Resize(_items.Length * 2);
            }

            _items[_count] = item;
            _count++;
        }

        public void PushRange(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            var items = new List<T>();
            foreach (var item in collection)
            {
                items.Add(item);
            }

            if (items.Count == 0)
            {
                return;
            }

            int newCount = _count + items.Count;
            if (newCount > _items.Length)
            {
                int newCapacity = _items.Length;
                while (newCapacity < newCount)
                {
                    newCapacity *= 2;
                }
                Resize(newCapacity);
            }

            for (int i = items.Count - 1; i >= 0; i--)
            {
                _items[_count] = items[i];
                _count++;
            }
        }

        public T Pop()
        {
            if (_count == 0)
                throw new InvalidOperationException("Стек пуст");

            _count--;
            T item = _items[_count];
            _items[_count] = default(T);
            return item;
        }

        public T Peek()
        {
            if (_count == 0)
                throw new InvalidOperationException("Стек пуст");

            return _items[_count - 1];
        }

        public bool Contains(T item)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int i = 0; i < _count; i++)
            {
                if (comparer.Equals(_items[i], item))
                {
                    return true;
                }
            }
            return false;
        }

        public int Count => _count;

        public int Capacity => _items.Length;

        private void Resize(int newCapacity)
        {
            T[] newArray = new T[newCapacity];
            Array.Copy(_items, 0, newArray, 0, _count);
            _items = newArray;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = _count - 1; i >= 0; i--)
            {
                yield return _items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T this[int depth]
        {
            get
            {
                if (depth < 0 || depth >= _count)
                    throw new ArgumentOutOfRangeException(nameof(depth), "Индекс находится вне границ стека");

                return _items[_count - 1 - depth];
            }
            set
            {
                if (depth < 0 || depth >= _count)
                    throw new ArgumentOutOfRangeException(nameof(depth), "Индекс находится вне границ стека");

                _items[_count - 1 - depth] = value;
            }
        }

        public void Display()
        {
            Console.Write("Стек (вершина -> основание): ");
            if (_count == 0)
            {
                Console.WriteLine("пуст");
                return;
            }

            for (int i = _count - 1; i >= 0; i--)
            {
                Console.Write(_items[i]);
                if (i > 0)
                    Console.Write(" <- ");
            }
            Console.WriteLine($" (Количество: {_count}, Ёмкость: {Capacity})");
        }
    }

    internal class Solution
    {
        static void Main(string[] args)
        {
            SmartStack<string> stack = new SmartStack<string>();

            while (true)
            {
                Console.WriteLine("1. Push (добавить элемент)");
                Console.WriteLine("2. PushRange (добавить несколько)");
                Console.WriteLine("3. Pop (извлечь элемент)");
                Console.WriteLine("4. Peek (посмотреть вершину)");
                Console.WriteLine("5. Contains (найти элемент)");
                Console.WriteLine("6. Показать стек");
                Console.WriteLine("7. Показать Count и Capacity");
                Console.WriteLine("8. Получить элемент по глубине");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.Write("Введите элемент: ");
                            string value = Console.ReadLine();
                            stack.Push(value);
                            Console.WriteLine($"Элемент '{value}' добавлен.");
                            break;

                        case "2":
                            Console.Write("Введите элементы через запятую: ");
                            string[] parts = Console.ReadLine().Split(',');
                            var items = new List<string>();
                            foreach (var p in parts)
                            {
                                string trimmed = p.Trim();
                                if (!string.IsNullOrEmpty(trimmed))
                                    items.Add(trimmed);
                            }
                            stack.PushRange(items);
                            Console.WriteLine($"Добавлено {items.Count} элементов.");
                            break;

                        case "3":
                            string popped = stack.Pop();
                            Console.WriteLine($"Извлечён элемент: '{popped}'");
                            break;

                        case "4":
                            string top = stack.Peek();
                            Console.WriteLine($"Вершина стека: '{top}'");
                            break;

                        case "5":
                            Console.Write("Введите элемент для поиска: ");
                            string search = Console.ReadLine();
                            bool found = stack.Contains(search);
                            Console.WriteLine(found ? "Элемент найден." : "Элемент не найден.");
                            break;

                        case "6":
                            stack.Display();
                            break;

                        case "7":
                            Console.WriteLine($"Количество элементов: {stack.Count}");
                            Console.WriteLine($"Ёмкость массива: {stack.Capacity}");
                            break;

                        case "8":
                            Console.Write("Введите глубину (0 — вершина): ");
                            if (!int.TryParse(Console.ReadLine(), out int depth))
                            {
                                Console.WriteLine("Ошибка: введите целое число.");
                                break;
                            }
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
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Ошибка операции: {ex.Message}");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Console.WriteLine($"Ошибка индекса: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Непредвиденная ошибка: {ex.Message}");
                }
            }
        }
    }
}
