using System;
using System.Collections.Generic;
using System.Text;

namespace SpellSword.Util
{
    static class QueueExtensions
    {
        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> elements)
        {
            foreach (T element in elements)
            {
                queue.Enqueue(element);
            }
        }
    }
}
