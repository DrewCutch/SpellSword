using System;
using System.Collections.Generic;
using System.Text;
using Troschuetz.Random;

namespace SpellSword.MapGeneration.Sources
{
    class LimitedSource<TContext, TValue> : Source<TContext, TValue>
    {
        private Source<TContext, TValue> _baseSource;
        public int Limit { get; }
        private int _used;

        public LimitedSource(Source<TContext, TValue> baseSource, int limit, bool shared): base(shared)
        {
            _baseSource = baseSource;
            Limit = limit;
            _used = 0;
        }

        public override SourceCursor<TValue> Pull(TContext context)
        {
            if (_used == Limit)
                return default;

            SourceCursor<TValue> baseCursor = _baseSource.Pull(context);

            baseCursor.OnUsed += (value) => { _used += 1; };

            return baseCursor;
        }

        public override bool IsEmpty()
        {
            return _used == Limit || _baseSource.IsEmpty();
        }

        public override Source<TContext, TValue> Clone()
        {
            if (Shared)
                return this;

            LimitedSource<TContext, TValue> clone = new LimitedSource<TContext, TValue>(_baseSource.Clone(), Limit, false);

            return clone;
        }
    }
}
