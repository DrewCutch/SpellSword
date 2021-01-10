using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Util.Collections
{
    interface IBag<T>: IReadOnlyBag<T>
    {
        public void Put(T item);
    }
}
