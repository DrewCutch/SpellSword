using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Util
{
    static class EnumExtensions
    {
        public static IEnumerable<T> GetFlags<T>(this T flags) where T : Enum
        {
            foreach (Enum value in Enum.GetValues(flags.GetType()))
            {
                if (flags.HasFlag(value))
                    yield return (T) value;
            }
        }
    }
}
