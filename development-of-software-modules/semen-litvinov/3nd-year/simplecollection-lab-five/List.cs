using System;
using System.Collections;
using System.Collections.Generic;

namespace SimpleCollections
{
    // Динамический массив (аналог vector) в виде параметризованного класса.
    public class List<T> : IEnumerable<T>, IDisposable
    {
        private T[] _data;
        private int _size;

        // Конструктор по умолчанию
        public List()
        {
            _data = new T[4];
            _size = 0;
        }

        // Копирующий конструктор: создаёт независимую копию данных
        public List(List<T> other)
        {
            _data = new T[Math.Max(other._data.Length, 4)];
            Array.Copy(other._data, _data, other._size);
            _size = other._size;
        }

        // Перемещающий конструктор: забирает буфер у source, source остаётся пустым.
        // В C# нет ссылок rvalue (&&), поэтому перемещение оформлено отдельным
        // конструктором с явным флагом.
        public List(List<T> source, bool move)
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

        // Деструктор (финализатор). Память массива освобождает сборщик мусора,
        // здесь только сбрасываем ссылки; явная очистка - через Dispose().
        ~List()
        {
            _data = null;
        }

        public void Dispose()
        {
            _data = new T[4];
            _size = 0;
            GC.SuppressFinalize(this);
        }

        // Оператор = в C# перегрузить нельзя, поэтому присваивание
        // выполняют методы: Assign (копирование) и MoveFrom (перемещение).
        // Assign создаёт новый буфер и записывает в него данные.
        public List<T> Assign(List<T> other)
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

        public List<T> MoveFrom(List<T> other)
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

        // Добавление элемента в конец
        public void Add(T value)
        {
            if (_size == _data.Length)
                Grow();
            _data[_size++] = value;
        }

        // Получение размера
        public int Size()
        {
            return _size;
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _size) throw new IndexOutOfRangeException();
                return _data[index];
            }
            set
            {
                if (index < 0 || index >= _size) throw new IndexOutOfRangeException();
                _data[index] = value;
            }
        }

        private void Grow()
        {
            T[] newData = new T[_data.Length * 2];
            Array.Copy(_data, newData, _size);
            _data = newData;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _size; i++)
                yield return _data[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
