using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Util.Collections
{
    interface IReadOnlyBag<T>
    {
        public T Get(bool remove);
    }
}
