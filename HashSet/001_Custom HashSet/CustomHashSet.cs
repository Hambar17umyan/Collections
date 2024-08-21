using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace _001_Custom_HashSet
{
    public class CustomHashSet<T> : ICollection<T>, ISet<T>
    {
        private static IEqualityComparer<T> _defaultComparer;
        static CustomHashSet()
        {
            DefaultEqualityComparer defaultEqualityComparer = new DefaultEqualityComparer();
            _defaultComparer = defaultEqualityComparer;
        }
        public CustomHashSet()
        {
            _comparer = _defaultComparer;
            Capacity = 4;
            _buckets = new List<T>[Capacity];
        }
        public CustomHashSet(int capacity)
        {
            _comparer = _defaultComparer;
            Capacity = capacity;
            _buckets = new List<T>[Capacity];
        }
        public CustomHashSet(IEnumerable<T> collection) : this()
        {
            foreach (var a in collection)
            {
                Add(a);
            }
        }
        public CustomHashSet(IEqualityComparer<T> comparer) : this()
        {
            _comparer = comparer ?? _defaultComparer;
        }
        public CustomHashSet(int capacity, IEqualityComparer<T> comparer) : this(capacity)
        {
            _comparer = comparer ?? _defaultComparer;
        }
        public CustomHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer) : this(collection)
        {
            _comparer = comparer ?? _defaultComparer;
        }
        public int Capacity
        {
            get;
            private set;
        }
        public int Count
        {
            get
            {
                return Capacity;
            }
            private set
            {
                Count = value;
                if (Count > Capacity)
                {
                    resize();
                }
            }
        }

        private void resize()
        {
            Capacity <<= 1;
            var arr = new List<T>[Capacity];
            foreach (var a in this)
            {
                var index = _comparer.GetHashCode(a) % Capacity;
                if (arr[index] == null)
                {
                    arr[index] = new List<T> { a };
                }
                else
                {
                    arr[index].Add(a);
                }
            }
        }

        private List<T>[] _buckets;
        private IEqualityComparer<T> _comparer;
        public bool IsReadOnly => throw new NotImplementedException();


        public void Add(T item)
        {
            int bucketIndex = _comparer.GetHashCode(item) % Capacity;
            var bucket = _buckets[bucketIndex];

            if (bucket == null)
            {
                bucket = new List<T>();
                bucket.Add(item);
                Count++;
                return;
            }
            else
            {
                foreach (var a in bucket)
                {
                    if (_comparer.Equals(a, item))
                    {
                        return;
                    }
                }
                Count++;
                bucket.Add(item);
            }
        }

        public void Clear()
        {
            for (int i = 0; i < Capacity; i++)
            {
                _buckets[i] = new List<T>();
            }
        }

        public bool Contains(T item)
        {
            int bucketIndex = _comparer.GetHashCode(item) % Capacity;
            var bucket = _buckets[bucketIndex];

            if (bucket == null)
            {
                bucket = new List<T>();
                return false;
            }
            else
            {
                foreach (var a in bucket)
                {
                    if (_comparer.Equals(a, item))
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            int i = arrayIndex;
            if (array.Length <= arrayIndex)
                throw new ArgumentException();

            foreach (var a in this)
            {
                if (i == array.Length)
                    break;
                array[i++] = a;
            }
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            foreach (var a in other)
            {
                Remove(a);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var a in _buckets)
            {
                foreach (var b in a)
                {
                    yield return b;
                }
            }
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            CustomHashSet<T> newSet = new CustomHashSet<T>();
            foreach (var a in other)
            {
                if (Contains(a))
                    newSet.Add(a);
            }

            _buckets = newSet._buckets;
            Capacity = newSet.Capacity;
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            foreach (var a in this)
            {
                if (!other.Contains(a))
                {
                    return false;
                }
            }
            return other.Count() == Count;
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            foreach (var a in other)
            {
                if (!Contains(a))
                {
                    return false;
                }
            }
            return other.Count() == Count;
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            foreach (var a in this)
            {
                if (!other.Contains(a))
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            foreach (var a in other)
            {
                if (!Contains(a))
                {
                    return false;
                }
            }
            return true;
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            foreach (var a in other)
            {
                if (Contains(a))
                {
                    return true;
                }
            }
            return false;
        }

        public bool Remove(T item)
        {
            int bucketIndex = _comparer.GetHashCode(item) % Capacity;
            var bucket = _buckets[bucketIndex];

            if (bucket == null)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < bucket.Count; i++)
                {
                    if (_comparer.Equals(bucket[i], item))
                    {
                        _buckets[bucketIndex].RemoveAt(i);
                        Count--;
                        return true;
                    }
                }
                return false;
            }
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            if (other.Count() != Count)
                return false;
            if (IsSubsetOf(other))
                return true;
            return false;
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            foreach (var a in other)
            {
                if (Contains(a))
                    Remove(a);
                else Add(a);
            }
        }

        public void UnionWith(IEnumerable<T> other)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            foreach (var a in other)
            {
                Add(a);
            }
        }

        bool ISet<T>.Add(T item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class DefaultEqualityComparer : IEqualityComparer<T>
        {
            public bool Equals(T? x, T? y)
            {
                if (x is null)
                    if (y is null)
                        return true;
                    else
                        return false;
                else
                    return x.Equals(y);
            }

            public int GetHashCode([DisallowNull] T obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
