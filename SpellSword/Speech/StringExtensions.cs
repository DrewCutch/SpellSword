using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace SpellSword.Speech
{
    static class StringExtensions
    {
        /// <summary>
        /// Returns the string with a trailing space if it is not null or empty
        /// </summary>
        /// <param name="str">The string to append a trailing space to</param>
        /// <returns>The string with a trailing space</returns>
        public static string WithTrailingSpace(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return "";

            return str + " ";
        }
    }
}
