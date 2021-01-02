using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.RPG.Attributes.Operators
{
    class ValueOperator: IOperator
    {
        public int Calculate(int value)
        {
            return value;
        }
    }
}
