using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.RPG.Attributes.Operators
{
    class MinOperator: IOperator
    {
        private readonly IOperator _left;
        private readonly IOperator _right;

        public MinOperator(IOperator left, IOperator right)
        {
            _left = left;
            _right = right;
        }

        public int Calculate(int value)
        {
            return Math.Min(_left.Calculate(value), _right.Calculate(value));
        }
    }
}
