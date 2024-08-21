using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _001_Custom_LinkedList
{
    public class CustomLinkedList<T> : ICollection<T>, ICollection
    {
        public int Count
        {
            get;
            private set;
        }
        public CustomLinkedListNode<T>? First
        {
            get;
            private set;
        }
        public CustomLinkedListNode<T>? Last
        {
            get;
            private set;
        }

        #region Not Implemented Properties
        public bool IsReadOnly => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();

        public object SyncRoot => throw new NotImplementedException();
        #endregion

        void ICollection<T>.Add(T item)
        {
            AddLast(item);
        }

        public CustomLinkedListNode<T> AddLast(T value)
        {
            var node = new CustomLinkedListNode<T>(this, Last, null, value);
            if (Last != null)
                Last.Next = node;
            node.Previous = Last;
            Last = node;
            if (Count == 0)
            {
                First = node;
                Last = node;
            }
            Count++;
            return node;
        }

        public CustomLinkedListNode<T> AddFirst(T value)
        {
            var node = new CustomLinkedListNode<T>(this, null, First, value);
            if (First != null)
                First.Next = node;
            node.Next = First;
            First = node;
            if (Count == 0)
            {
                First = node;
                Last = node;
            }
            Count++;
            return node;
        }

        public CustomLinkedListNode<T> AddAfter(CustomLinkedListNode<T> node, T value)
        {
            CustomLinkedListNode<T> newNode = new CustomLinkedListNode<T>(this, node, node.Next, value);
            if (newNode.Next != null)
            {
                newNode.Next.Previous = newNode;
            }
            else
            {
                Last = newNode;
            }
            if (newNode.Previous != null)
            {
                newNode.Previous.Next = newNode;
            }
            else
            {
                First = newNode;
            }

            return newNode;
        }

        public CustomLinkedListNode<T> AddBefore(CustomLinkedListNode<T> node, T value)
        {
            CustomLinkedListNode<T> newNode = new CustomLinkedListNode<T>(this, node.Previous, node, value);
            if (newNode.Next != null)
            {
                newNode.Next.Previous = newNode;
            }
            else
            {
                Last = newNode;
            }
            if (newNode.Previous != null)
            {
                newNode.Previous.Next = newNode;
            }
            else
            {
                First = newNode;
            }

            return newNode;
        }

        public void Clear()
        {
            First = null;
            Last = null;
        }

        public bool Contains(T item)
        {
            foreach (var c in this)
            {
                if (c.Equals(item))
                {
                    return true;
                }
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            int i = arrayIndex;
            if (arrayIndex < 0 || arrayIndex >= array.Length)
                throw new IndexOutOfRangeException();

            foreach (var c in this)
            {
                if (i == array.Length)
                    break;
                array[i++] = c;
            }
        }

        public void CopyTo(Array array, int index)
        {
            int i = index;
            if (index < 0 || index >= array.Length)
                throw new IndexOutOfRangeException();

            foreach (var c in this)
            {
                if (i == array.Length)
                    break;
                array.SetValue(c, i++);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var node = First;
            while (node != null)
            {
                yield return node.Value;
                node = node.Next;
            }
        }

        public bool Remove(T item)
        {
            var node = First;
            while (node != null)
            {
                if (node.Value.Equals(item))
                {
                    Remove(node);
                    return true;
                }
                node = node.Next;
            }
            return false;
        }
        public void Remove(CustomLinkedListNode<T> node)
        {
            if(node.List != this)
                throw new InvalidOperationException();
            if (node.Previous != null)
            {
                node.Previous.Next = node.Next;
            }
            if (node.Next != null)
            {
                node.Next.Previous = node.Previous;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class CustomLinkedListNode<T>
    {
        public CustomLinkedListNode<T>? Next { get; internal set; }
        public CustomLinkedListNode<T>? Previous { get; internal set; }
        public T Value { get; private set; }
        public CustomLinkedList<T> List { get; }

        internal CustomLinkedListNode(CustomLinkedList<T> list, CustomLinkedListNode<T>? prev, CustomLinkedListNode<T>? next, T value)
        {
            List = list;
            Previous = prev;
            Next = next;
            Value = value;
        }
    }

}
