using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Util
{
    static class StringHash
    {
        public static int StableHash(string str)
        {
            int hash = 5381;

            foreach (char c in str)
                hash = ((hash << 5) + hash) + c;

            return hash;
        }
    }
}
