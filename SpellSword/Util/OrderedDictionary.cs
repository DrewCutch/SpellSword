using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpellSword.Util
{
    class OrderedDictionary<T, U>: IDictionary<T, U>
    {
        private readonly Dictionary<T, int> _insertionOrder;

        private readonly SortedDictionary<T, U> _underlyingDict;

        private int _entryIndex;

        public OrderedDictionary()
        {
            _entryIndex = 0;
            _insertionOrder = new Dictionary<T, int>();
            _underlyingDict = new SortedDictionary<T, U>(Comparer<T>.Create(((a, b) => _insertionOrder[a].CompareTo(_insertionOrder[b]))));
        }

        private int GetNextEntryIndex()
        {
            _entryIndex = _entryIndex + 1;

            if (_entryIndex == Int32.MaxValue)
            {
                // TODO: normalize indices
                throw new NotImplementedException();
            }

            return _entryIndex;
        }


        public IEnumerator<KeyValuePair<T, U>> GetEnumerator()
        {
            return _underlyingDict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable) _underlyingDict).GetEnumerator();
        }

        public void Add(KeyValuePair<T, U> item)
        {
            if (!_insertionOrder.ContainsKey(item.Key))
                _insertionOrder[item.Key] = GetNextEntryIndex();

            _underlyingDict.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _underlyingDict.Clear();
        }

        public bool Contains(KeyValuePair<T, U> item)
        {
            return _underlyingDict.ContainsKey(item.Key);
        }

        public void CopyTo(KeyValuePair<T, U>[] array, int arrayIndex)
        {
            _underlyingDict.CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<T, U> item)
        {
            return _underlyingDict.Remove(item.Key);
        }

        public int Count => _underlyingDict.Count;

        public bool IsReadOnly => false;

        public void Add(T key, U value)
        {
            if (!_insertionOrder.ContainsKey(key))
                _insertionOrder[key] = GetNextEntryIndex();
            _underlyingDict.Add(key, value);
        }

        public bool ContainsKey(T key)
        {
            return _underlyingDict.ContainsKey(key);
        }

        public bool Remove(T key)
        {
            bool underlyingRemove = _underlyingDict.Remove(key);
            _insertionOrder.Remove(key);
            return underlyingRemove;
        }

        public bool TryGetValue(T key, out U value)
        {
            return _underlyingDict.TryGetValue(key, out value);
        }

        public U this[T key]
        {
            get => _underlyingDict[key];
            set
            {
                if(!_insertionOrder.ContainsKey(key))
                    _insertionOrder[key] = GetNextEntryIndex();

                _underlyingDict[key] = value;
            }
        }

        public ICollection<T> Keys => _underlyingDict.Keys;

        public ICollection<U> Values => _underlyingDict.Values;
    }
}
