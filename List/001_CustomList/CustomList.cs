using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _001_CustomList
{
    internal class CustomList<T> : IList<T>, IList
    {
        private T[] _items;

        #region Properties
        public int Capacity
        {
            get
            {
                return Capacity;
            }
            set
            {
                if (value < _items.Length)
                {
                    throw new ArgumentOutOfRangeException();
                }
                else if (value != _items.Length)
                {
                    Array.Resize(ref _items, value);
                    Capacity = value;
                }
            }
        }
        public T this[int index]
        {
            get
            {
                if (index >= Count)
                    throw new IndexOutOfRangeException();
                else
                    return _items[index];
            }
            set
            {
                if (index >= Count)
                    throw new IndexOutOfRangeException();
                else
                    _items[index] = value;
            }
        }
        object? IList.this[int index]
        {
            get
            {
                if (index >= Count)
                    throw new IndexOutOfRangeException();
                else
                    return _items[index];
            }
            set
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (index >= Count)
                    throw new IndexOutOfRangeException();
                else if (typeof(T) == value.GetType())
                    _items[index] = (T)value;
                else
                    throw new InvalidCastException();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
        }
        public int Count
        {
            get
            {
                return Count;
            }
            private set
            {
                if (value > Capacity)
                    Capacity *= 2;
                if (value < Capacity / 3)
                    Capacity /= 2;
                Count = value;
            }
        }
        #endregion
        #region Not Implemented Properties
        public bool IsReadOnly => throw new NotImplementedException();

        public bool IsFixedSize => throw new NotImplementedException();

        public bool IsSynchronized => throw new NotImplementedException();

        public object SyncRoot => throw new NotImplementedException();
        #endregion
        #region Constructors
        public CustomList() : this(4) { }

        public CustomList(int capacity)
        {
            _items = new T[capacity];
            Capacity = capacity;
        }

        public CustomList(IEnumerable<T> enumerable) : this()
        {
            foreach (var item in enumerable)
            {
                Add(item);
            }
        }
        #endregion
        #region Public Methods
        public void Add(T item)
        {
            Count++;
            _items[Count - 1] = item;
        }

        public void Clear()
        {
            Array.Clear(_items, 0, Count);
        }

        public bool Contains(T item)
        {
            foreach (var c in this)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (item.Equals(c))
                {
                    return true;
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_items, 0, array, arrayIndex, Count);
        }

        public int IndexOf(T item)
        {
            for (var i = 0; i < Count; i++)
            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                if (item.Equals(_items[i]))
                {
                    return i;
                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            }

            return -1;
        }

        public void Insert(int index, T item)
        {
            if (index == Count)
            {
                Add(item);
            }
            else if (index < Count)
            {
                Count++;
                T[] newArr = new T[Capacity];
                for (var i = 0; i < index; i++)
                {
                    newArr[i] = _items[i];
                }
                newArr[index] = item;
                for (var i = index; i < Count - 1; i++)
                {
                    newArr[i + 1] = _items[i];
                }

                _items = newArr;
            }
            else
            {
                throw new IndexOutOfRangeException();
            }
        }

        public bool Remove(T item)
        {
            int i = IndexOf(item);
            if (i < 0)
                return false;
            RemoveAt(i);
            return true;
        }

        public void RemoveAt(int index)
        {
            Count--;
            T[] newArr = new T[Count];
            for (var i = 0; i < index; i++)
            {
                newArr[i] = _items[i];
            }
            for (var i = index + 1; i < Count + 1; i++)
            {
                newArr[i - 1] = _items[i];
            }
            _items = newArr;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
            {
                yield return _items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
        #region Not Implemented Explicit Methods

        int IList.Add(object? value)
        {
            throw new NotImplementedException();
        }

        bool IList.Contains(object? value)
        {
            throw new NotImplementedException();
        }

        int IList.IndexOf(object? value)
        {
            throw new NotImplementedException();
        }

        void IList.Insert(int index, object? value)
        {
            throw new NotImplementedException();
        }

        void IList.Remove(object? value)
        {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
