using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.MapGeneration.Sources
{
    public class SourceCursor<T>
    {
        public readonly T Value;

        public event Action<T> OnUsed;

        public SourceCursor(T value, Action<T> onUsed)
        {
            Value = value;
            OnUsed += onUsed;
        }

        public T Use()
        {
            OnUsed.Invoke(Value);

            return Value;
        }
    }
}
