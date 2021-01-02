using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.RPG.Attributes
{
    interface IOperator
    {
        public int Calculate(int value);
    }
}
