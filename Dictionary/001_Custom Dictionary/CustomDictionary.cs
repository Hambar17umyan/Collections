using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace _001_Custom_Dictionary
{
    public class CustomDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, ICollection, IDictionary where TKey : notnull
    {
        private readonly static IEqualityComparer<TKey> _defaultComparer = new DefaultEqualityComparer();
        private IEqualityComparer<TKey> _comparer;
        private List<KeyValuePair<TKey, TValue>>[] _buckets;
        private List<TKey> _keys;
        private List<TValue> _values;

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
            var arr = new List<KeyValuePair<TKey, TValue>>[Capacity];
            foreach (var a in this)
            {
                var index = _comparer.GetHashCode(a.Key) % Capacity;
                if (arr[index] == null)
                {
                    arr[index] = new List<KeyValuePair<TKey, TValue>> { a };
                }
                else
                {
                    arr[index].Add(a);
                }
            }
        }
        private bool LocateTheItem(TKey key, out (int bucketID, int index) res)
        {
            var bucket = _comparer.GetHashCode(key) % Capacity;
            if (_buckets[bucket].Count == 0)
            {
                res = (bucket, -1);
                return false;
            }
            for (int i = 0; i < _buckets[bucket].Count; i++)
            {
                if (_comparer.Equals(key, _buckets[bucket][i].Key))
                {
                    res = (bucket, i);
                    return true;
                }
            }
            res = (bucket, -1);
            return false;
        }

        public TValue this[TKey key]
        {
            get
            {
                var success = LocateTheItem(key, out var res);
                if (success)
                {
                    return _buckets[res.bucketID][res.index].Value;
                }
                throw new InvalidOperationException();
            }
            set
            {
                var success = LocateTheItem(key, out var res);
                if (success)
                {
                    _buckets[res.bucketID][res.index] = new KeyValuePair<TKey, TValue>(key, value);
                }
                else
                {
                    _buckets[res.bucketID].Add(new KeyValuePair<TKey, TValue>(key, value));
                }
            }
        }

        #region Explicit implementations
        object? IDictionary.this[object key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int ICollection.Count => throw new NotImplementedException();

        bool ICollection.IsSynchronized => throw new NotImplementedException();

        object ICollection.SyncRoot => throw new NotImplementedException();

        bool IDictionary.IsFixedSize => throw new NotImplementedException();

        bool IDictionary.IsReadOnly => throw new NotImplementedException();

        ICollection IDictionary.Keys => throw new NotImplementedException();

        ICollection IDictionary.Values => throw new NotImplementedException();

        void IDictionary.Add(object key, object? value)
        {
            throw new NotImplementedException();
        }

        void IDictionary.Clear()
        {
            throw new NotImplementedException();
        }

        bool IDictionary.Contains(object key)
        {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }


        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        void IDictionary.Remove(object key)
        {
            throw new NotImplementedException();
        }

        #endregion

        public CustomDictionary() : this(identifier: true)
        {

        }
        public CustomDictionary(int capacity) : this(capacity: capacity, identifier: true)
        {

        }
        public CustomDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) : this(identifier: true, collection: collection)
        {

        }
        public CustomDictionary(IEqualityComparer<TKey> comparer) : this(identifier: true, comparer: comparer)
        {

        }
        public CustomDictionary(IDictionary<TKey, TValue> dictionary) : this(identifier: true, dictionary: dictionary)
        {

        }
        public CustomDictionary(int capacity, IEqualityComparer<TKey>? comparer) : this(identifier: true, capacity: capacity)
        {

        }
        public CustomDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey>? comparer) : this(identifier: true, collection: collection, comparer: comparer)
        {

        }
        public CustomDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey>? comparer) : this(identifier: true, dictionary: dictionary, comparer: comparer)
        {

        }
        private CustomDictionary(bool identifier, int capacity = 4, IEnumerable<KeyValuePair<TKey, TValue>>? collection = null, IDictionary<TKey, TValue>? dictionary = null, IEqualityComparer<TKey>? comparer = null)
        {
            Count = 0;
            _keys = new List<TKey>();
            _values = new List<TValue>();
            _buckets = new List<KeyValuePair<TKey, TValue>>[capacity];

            Capacity = capacity;

            if (comparer != null)
                _comparer = comparer;
            else
                _comparer = _defaultComparer;

            if (dictionary != null)
            {
                collection = dictionary;

            }

            if (collection != null)
            {
                foreach (var a in collection)
                    Add(a);
            }

        }


        public bool IsReadOnly => throw new NotImplementedException();

        public ICollection<TKey> Keys => _keys;

        public ICollection<TValue> Values => _values;

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            var success = LocateTheItem(item.Key, out var res);
            if (success)
            {
                throw new InvalidOperationException();
            }
            else
            {
                _buckets[res.bucketID].Add(item);
                _keys.Add(item.Key);
                _values.Add(item.Value);
            }
        }

        public void Add(TKey key, TValue value)
        {
            Add(new KeyValuePair<TKey, TValue>(key, value));
        }

        public void Clear()
        {
            for (int i = 0; i < Capacity; i++)
            {
                _buckets[i] = new List<KeyValuePair<TKey, TValue>>();
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            var success = LocateTheItem(item.Key, out var res);
            return success;
        }

        public bool ContainsKey(TKey key)
        {
            return _keys.Contains(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
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

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (var a in _buckets)
            {
                foreach (var b in a)
                {
                    yield return b;
                }
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            var success = LocateTheItem(item.Key, out var res);
            if (success)
            {
                if (item.Equals(_buckets[res.bucketID][res.index]))
                {
                    _values.Remove(item.Value);
                    _keys.Remove(item.Key);
                    _buckets[res.bucketID].RemoveAt(res.index);
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }

        public bool Remove(TKey key)
        {
            var success = LocateTheItem(key, out var res);
            if (success)
            {
                var item = _buckets[res.bucketID][res.index];
                _values.Remove(item.Value);
                _keys.Remove(item.Key);
                _buckets[res.bucketID].RemoveAt(res.index);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
        {
            var success = LocateTheItem(key, out var res);
            value = default;
            if (success)
            {
                value = this[key];
                return true;
            }
            else
            {
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class DefaultEqualityComparer : IEqualityComparer<TKey>
        {
            public bool Equals(TKey? x, TKey? y)
            {
                if (x is null)
                    if (y is null)
                        return true;
                    else
                        return false;
                else
                    return x.Equals(y);
            }

            public int GetHashCode([DisallowNull] TKey obj)
            {
                return obj.GetHashCode();
            }
        }
    }
}
