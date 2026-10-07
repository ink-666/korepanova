using System;

namespace SimpleCollections
{
    // Стек на основе динамического массива.
    public class Stack<T> : IDisposable
    {
        private T[] _data;
        private int _size;

        // Конструктор по умолчанию
        public Stack()
        {
            _data = new T[4];
            _size = 0;
        }

        // Копирующий конструктор
        public Stack(Stack<T> other)
        {
            _data = new T[Math.Max(other._data.Length, 4)];
            Array.Copy(other._data, _data, other._size);
            _size = other._size;
        }

        // Перемещающий конструктор: забирает буфер, source становится пустым
        public Stack(Stack<T> source, bool move)
        {
            if (!move)
            {
                _data = new T[Math.Max(source._data.Length, 4)];
                Array.Copy(source._data, _data, source._size);
                _size = source._size;
                return;
            }
            _data = source._data;
            _size = source._size;
            source._data = new T[4];
            source._size = 0;
        }

        // Деструктор (финализатор)
        ~Stack()
        {
            _data = null;
        }

        public void Dispose()
        {
            _data = new T[4];
            _size = 0;
            GC.SuppressFinalize(this);
        }

        // Присваивание с копированием
        public Stack<T> Assign(Stack<T> other)
        {
            if (!ReferenceEquals(this, other))
            {
                T[] newData = new T[Math.Max(other._data.Length, 4)];
                Array.Copy(other._data, newData, other._size);
                _data = newData;
                _size = other._size;
            }
            return this;
        }

        // Присваивание с перемещением
        public Stack<T> MoveFrom(Stack<T> other)
        {
            if (!ReferenceEquals(this, other))
            {
                _data = other._data;
                _size = other._size;
                other._data = new T[4];
                other._size = 0;
            }
            return this;
        }

        // push() - добавляет элемент в конец
        public void Push(T value)
        {
            if (_size == _data.Length)
            {
                T[] newData = new T[_data.Length * 2];
                Array.Copy(_data, newData, _size);
                _data = newData;
            }
            _data[_size++] = value;
        }

        // pop() - удаляет последний элемент
        public void Pop()
        {
            if (Empty())
                throw new InvalidOperationException("Stack.Pop: стек пуст");
            _size--;
            _data[_size] = default(T);
        }

        // top() - возвращает ссылку на самый верхний элемент
        public ref T Top()
        {
            if (Empty())
                throw new InvalidOperationException("Stack.Top: стек пуст");
            return ref _data[_size - 1];
        }

        // empty() - проверка пустоты
        public bool Empty()
        {
            return _size == 0;
        }

        public int Size()
        {
            return _size;
        }
    }
}
