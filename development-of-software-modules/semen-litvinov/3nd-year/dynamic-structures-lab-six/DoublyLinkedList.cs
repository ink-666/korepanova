using System;
using System.Collections;
using System.Collections.Generic;

namespace LinkedListsLab
{
    // Двунаправленный (двусвязный) список.
    // Хранятся ссылки на голову (_head) и хвост (_tail),
    // у узла есть Prev и Next.
    public class DoublyLinkedList<T> : ILinkedContainer<T>
    {
        private class Node
        {
            public T Value;
            public Node Prev;
            public Node Next;
        }

        private Node _head;
        private Node _tail;
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

            if (_head != null)
                _head.Prev = node;
            else
                _tail = node;

            _head = node;
            _count++;
        }

        // Удаление первого элемента: O(1)
        public void RemoveFirst()
        {
            if (_head == null)
                throw new InvalidOperationException("Список пуст");

            _head = _head.Next;
            if (_head != null)
                _head.Prev = null;
            else
                _tail = null;

            _count--;
        }

        // Вставка в конец: O(1), потому что есть ссылка на хвост
        public void AddLast(T value)
        {
            Node node = new Node();
            node.Value = value;
            node.Prev = _tail;

            if (_tail != null)
                _tail.Next = node;
            else
                _head = node;

            _tail = node;
            _count++;
        }

        // Итератор вперёд (от головы к хвосту)
        public IEnumerator<T> GetEnumerator()
        {
            Node current = _head;
            while (current != null)
            {
                yield return current.Value;
                current = current.Next;
            }
        }

        // Итератор назад (от хвоста к голове) - доступен только в двусвязном списке
        public IEnumerable<T> Reverse()
        {
            Node current = _tail;
            while (current != null)
            {
                yield return current.Value;
                current = current.Prev;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}