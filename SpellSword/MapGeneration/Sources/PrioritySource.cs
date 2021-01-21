using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Troschuetz.Random;

namespace SpellSword.MapGeneration.Sources
{
    class PrioritySource<TContext, TValue> : Source<TContext, TValue>
    {
        private Queue<Source<TContext, TValue>> _sources;

        public PrioritySource(bool shared): base(shared)
        {
            _sources = new Queue<Source<TContext, TValue>>();
        }

        public void Add(Source<TContext, TValue> source)
        {
            _sources.Enqueue(source);
        }

        public override SourceCursor<TValue> Pull(TContext context)
        {
            while (_sources.Count > 0 && _sources.Peek().IsEmpty())
                _sources.Dequeue();

            if (_sources.Count == 0)
                return default;

            return _sources.Peek().Pull(context);
        }

        public override bool IsEmpty()
        {
            return _sources.Count == 0 || _sources.All((source) => source.IsEmpty());
        }

        public override Source<TContext, TValue> Clone()
        {
            if (Shared)
                return this;

            
            PrioritySource<TContext, TValue> clone = new PrioritySource<TContext, TValue>(false);

            foreach (Source<TContext, TValue> source in _sources)
            {
                clone.Add(source.Clone());
            }

            return clone;
        }
    }
}
