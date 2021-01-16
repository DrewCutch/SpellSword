using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Troschuetz.Random;

namespace SpellSword.MapGeneration.Sources
{
    class PrioritySource<T>: Source<T>
    {
        private Queue<Source<T>> _sources;

        public PrioritySource(bool shared): base(shared)
        {
            _sources = new Queue<Source<T>>();
        }

        public void Add(Source<T> source)
        {
            _sources.Enqueue(source);
        }

        public override SourceCursor<T> Pull(IGenerator rng)
        {
            while (_sources.Count > 0 && _sources.Peek().IsEmpty())
                _sources.Dequeue();

            if (_sources.Count == 0)
                return default;

            return _sources.Peek().Pull(rng);
        }

        public override bool IsEmpty()
        {
            return _sources.Count == 0 || _sources.All((source) => source.IsEmpty());
        }

        public override Source<T> Clone()
        {
            if (Shared)
                return this;

            
            PrioritySource<T> clone = new PrioritySource<T>(false);

            foreach (Source<T> source in _sources)
            {
                clone.Add(source.Clone());
            }

            return clone;
        }
    }
}
