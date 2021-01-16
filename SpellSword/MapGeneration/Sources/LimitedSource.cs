using System;
using System.Collections.Generic;
using System.Text;
using Troschuetz.Random;

namespace SpellSword.MapGeneration.Sources
{
    class LimitedSource<T>: Source<T>
    {
        private Source<T> _baseSource;
        public int Limit { get; }
        private int _used;

        public LimitedSource(Source<T> baseSource, int limit, bool shared): base(shared)
        {
            _baseSource = baseSource;
            Limit = limit;
            _used = 0;
        }

        public override SourceCursor<T> Pull(IGenerator rng)
        {
            if (_used == Limit)
                return default;

            SourceCursor<T> baseCursor = _baseSource.Pull(rng);

            baseCursor.OnUsed += (value) => { _used += 1; };

            return baseCursor;
        }

        public override bool IsEmpty()
        {
            return _used == Limit || _baseSource.IsEmpty();
        }

        public override Source<T> Clone()
        {
            if (Shared)
                return this;

            LimitedSource<T> clone = new LimitedSource<T>(_baseSource.Clone(), Limit, false);

            return clone;
        }
    }
}
