using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.RPG.Attributes.Operators
{
    class ConstantOperator: IOperator
    {
        private readonly int _value;
        public ConstantOperator(int value)
        {
            _value = value;
        }
        public int Calculate(int value)
        {
            return _value;
        }

        public static implicit operator ConstantOperator(int value)
        {
            return new ConstantOperator(value);
        }
    }
}
