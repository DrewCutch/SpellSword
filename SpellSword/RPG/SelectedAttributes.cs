using System;
using System.Collections.Generic;
using System.Text;
using SpellSword.RPG.Attributes;

namespace SpellSword.RPG
{
    class SelectedAttributes: IAttributeSource
    {
        public AttributeSet AttributeSet;

        public SelectedAttributes(AttributeSet attributeSet)
        {
            AttributeSet = attributeSet;
        }
        public AttributeSet GetAttributes(int level)
        {
            return AttributeSet;
        }
    }
}
