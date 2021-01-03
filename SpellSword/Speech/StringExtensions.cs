using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SpellSword.Speech
{
    static class StringExtensions
    {
        public static string WithTrailingSpace(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            return str + " ";
        }
    }
}
