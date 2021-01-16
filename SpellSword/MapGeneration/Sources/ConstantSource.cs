using Troschuetz.Random;

namespace SpellSword.MapGeneration.Sources
{
    class ConstantSource<T>: Source<T>
    {
        private T _value;
        public ConstantSource(T value) : base(true)
        {
            _value = value;
        }

        public override SourceCursor<T> Pull(IGenerator rng)
        {
            return new SourceCursor<T>(_value, (val) => { });
        }

        public override bool IsEmpty()
        {
            return false;
        }

        public override Source<T> Clone()
        {
            return this;
        }
    }
}
