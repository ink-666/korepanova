using System;
using System.Collections;
using System.Collections.Generic;

namespace LinkedListsLab
{
    // Однонаправленный (односвязный) список.
    // Хранится только ссылка на голову (_head), у узла есть только Next.
    public class SinglyLinkedList<T> : ILinkedContainer<T>
    {
        private class Node
        {
            public T Value;
            public Node Next;
        }

        private Node _head;
        private int _count;

        public int Count
        {
            get { return _count; }
        }

        // Вставка в начало: O(1)
        public void AddFirst(T value)
        {
            Node node = new Node();
            node.Value = value;
            node.Next = _head;
            _head = node;
            _count++;
        }

        // Удаление первого элемента: O(1)
        public void RemoveFirst()
        {
            if (_head == null)
                throw new InvalidOperationException("Список пуст");

            _head = _head.Next;
            _count--;
        }

        // Вставка в конец: O(n). Ссылки на хвост нет, поэтому
        // нужно пройти весь список от головы до последнего узла.
        public void AddLast(T value)
        {
            Node node = new Node();
            node.Value = value;

            if (_head == null)
            {
                _head = node;
            }
            else
            {
                Node current = _head;
                while (current.Next != null)
                    current = current.Next;
                current.Next = node;
            }
            _count++;
        }

        // Итератор: движение только вперёд
        public IEnumerator<T> GetEnumerator()
        {
            Node current = _head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}