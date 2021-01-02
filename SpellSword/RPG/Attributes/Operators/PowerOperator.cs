using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.RPG.Attributes.Operators
{
    class PowerOperator: IOperator
    {
        private readonly IOperator _left;
        private readonly IOperator _right;

        public PowerOperator(IOperator left, IOperator right)
        {
            _left = left;
            _right = right;
        }

        public int Calculate(int value)
        {
            return (int) Math.Pow(_left.Calculate(value), _right.Calculate(value));
        }
    }
}
