using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public static class Extensions
    {
        public static IEnumerable<T> ToEnumerable<T>(this T element)
        {
            yield return element;
        }
    }
}
