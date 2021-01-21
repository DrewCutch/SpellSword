using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.MapGeneration.Sources
{
    public class SourceCursor<ValueType>
    {
        public readonly ValueType Value;

        public event Action<ValueType> OnUsed;

        public SourceCursor(ValueType value, Action<ValueType> onUsed)
        {
            Value = value;
            OnUsed += onUsed;
        }

        public ValueType Use()
        {
            OnUsed.Invoke(Value);

            return Value;
        }
    }
}
