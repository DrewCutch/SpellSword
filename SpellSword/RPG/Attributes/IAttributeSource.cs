using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.RPG.Attributes
{
    interface IAttributeSource
    {
        public AttributeSet GetAttributes(int level);
    }
}
