using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Troschuetz.Random;

namespace SpellSword.Util.Collections
{
    class Bag<T> : IBag<T>
    {
        public int Count => _contents.Count;

        private List<T> _contents;

        private IGenerator _rng;

        public Bag(IGenerator rng)
        {
            _rng = rng;
            _contents = new List<T>();
        }

        public T Get(bool remove)
        {
            int i = _rng.Next(0, _contents.Count);

            T value = _contents[i];

            if (remove)
                _contents.RemoveAt(i);

            return value;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _contents.GetEnumerator();
            
        }

        public void Put(T item)
        {
            _contents.Add(item);
        }

        public void PutRange(IEnumerable<T> items)
        {
            _contents.AddRange(items);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _contents.GetEnumerator();
        }
    }
}
