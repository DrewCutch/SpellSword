using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.RPG.Attributes
{
    class AttributeScalar: IAttributeSource
    {
        private IOperator _strengthFormula;
        private IOperator _dexterityFormula;
        private IOperator _resolveFormula;
        private IOperator _intelligenceFormula;
        private IOperator _magicFormula;
        public AttributeSet GetAttributes(int level)
        {
            return new AttributeSet(
                _strengthFormula.Calculate(level), 
                _dexterityFormula.Calculate(level),
                _resolveFormula.Calculate(level),
                _intelligenceFormula.Calculate(level),
                _magicFormula.Calculate(level)
                );
        }
    }
}
