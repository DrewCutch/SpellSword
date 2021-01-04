using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Util
{
    static class IEnumerableExtensions
    {

        public static IEnumerable<(T, int)> WithIndex<T>(this IEnumerable<T> enumerable)
        {
            int i = 0;
            foreach (T value in enumerable)
            {
                yield return (value, i);

                i += 1;
            }
        }

        public static T AtIndex<T>(this IEnumerable<T> enumerable, int index)
        {
            int i = 0;
            foreach (T value in enumerable)
            {
                if (i == index)
                    return value;

                i += 1;
            }

            throw new IndexOutOfRangeException();
        }
    }
}
