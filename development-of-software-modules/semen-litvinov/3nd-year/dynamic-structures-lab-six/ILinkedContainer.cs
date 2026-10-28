using System.Collections.Generic;

namespace LinkedListsLab
{
    // Общий интерфейс двух контейнеров: через него одна и та же
    // процедура измерения работает и с односвязным, и с двусвязным списком.
    public interface ILinkedContainer<T> : IEnumerable<T>
    {
        int Count { get; }

        void AddFirst(T value);   // вставка в начало
        void RemoveFirst();       // удаление первого элемента
        void AddLast(T value);    // вставка в конец
    }
}