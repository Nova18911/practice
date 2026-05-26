using System.Collections;

namespace collections_task
{
    public class SmartStack<T> : IEnumerable<T>
    {
        private T[] _items;
        private int _count;
        private const int DefaultCapacity = 4;

        public int Count => _count;
        public int Capacity => _items.Length;

        public SmartStack() => _items = new T[DefaultCapacity];

        /// <exception cref="ArgumentOutOfRangeException">если capacity отрицательная.</exception>
        public SmartStack(int capacity)
        {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Ёмкость не может быть отрицательной.");
            _items = new T[Math.Max(capacity, DefaultCapacity)];
        }

        /// <exception cref="ArgumentNullException">если collection равна null.</exception>
        public SmartStack(IEnumerable<T> collection)
        {
            ArgumentNullException.ThrowIfNull(collection);

            var list = new List<T>(collection);
            _items = new T[Math.Max(list.Count, DefaultCapacity)];
            _count = list.Count;

            for (int i = 0; i < _count; i++)
                _items[i] = list[_count - 1 - i];
        }

        /// <exception cref="ArgumentNullException">если collection равна null.</exception>
        public void PushRange(IEnumerable<T> collection)
        {
            ArgumentNullException.ThrowIfNull(collection);

            var items = new List<T>(collection);
            if (items.Count == 0) return;

            EnsureCapacity(_count + items.Count);

            for (int i = items.Count - 1; i >= 0; i--)
                _items[_count++] = items[i];
        }

        public void Push(T item)
        {
            EnsureCapacity(_count + 1);
            _items[_count++] = item;
        }

        /// <exception cref="InvalidOperationException">если стек пуст.</exception>
        public T Pop()
        {
            ThrowIfEmpty();
            T item = _items[--_count];
            _items[_count] = default!;
            return item;
        }

        /// <exception cref="InvalidOperationException">если стек пуст.</exception>
        public T Peek()
        {
            ThrowIfEmpty();
            return _items[_count - 1];
        }

        public bool Contains(T item) =>
            EqualityComparer<T>.Default.Equals(
                _items.Take(_count).FirstOrDefault(x =>
                    EqualityComparer<T>.Default.Equals(x, item)),
                item
            );

        public T this[int depth]
        {
            get
            {
                ValidateDepth(depth);
                return _items[_count - 1 - depth];
            }
            set
            {
                ValidateDepth(depth);
                _items[_count - 1 - depth] = value;
            }
        }

        public void Display()
        {
            Console.Write("Стек (вершина -> основание): ");
            if (_count == 0) { Console.WriteLine("пуст"); return; }
            Console.WriteLine(string.Join(" <- ", this) + $" (Количество: {_count}, Ёмкость: {Capacity})");
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = _count - 1; i >= 0; i--)
                yield return _items[i];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private void EnsureCapacity(int required)
        {
            if (required <= _items.Length) return;
            int newCapacity = Math.Max(_items.Length * 2, required);
            Array.Resize(ref _items, newCapacity);
        }

        private void ThrowIfEmpty()
        {
            if (_count == 0)
                throw new InvalidOperationException("Стек пуст.");
        }

        private void ValidateDepth(int depth)
        {
            if (depth < 0 || depth >= _count)
                throw new ArgumentOutOfRangeException(nameof(depth), "Индекс находится вне границ стека.");
        }
    }
}