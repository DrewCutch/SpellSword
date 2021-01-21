using Troschuetz.Random;

namespace SpellSword.MapGeneration.Sources
{
    class ConstantSource<TContext, TValue> : Source<TContext, TValue>
    {
        private TValue _value;
        public ConstantSource(TValue value) : base(true)
        {
            _value = value;
        }

        public override SourceCursor<TValue> Pull(TContext context)
        {
            return new SourceCursor<TValue>(_value, (val) => { });
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override Source<TContext, TValue> Clone()
        {
            return this;
        }
    }
}
