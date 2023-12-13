using System;
using System.Collections.Generic;
using System.Linq;

namespace ARJE.Utils.Collections.Extensions
{
    public static class EnumerableExt
    {
        public static IEnumerable<(T Item, int Index)> Enumerated<T>(this IEnumerable<T> enumerable, int startIndex = 0)
        {
            ArgumentNullException.ThrowIfNull(enumerable);

            return enumerable.Select((item, index) => (item, index + startIndex));
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            ArgumentNullException.ThrowIfNull(enumerable);
            ArgumentNullException.ThrowIfNull(action);

            foreach (T item in enumerable)
            {
                action(item);
            }
        }

        public static string Dump<T>(this IEnumerable<T> enumerable)
        {
            ArgumentNullException.ThrowIfNull(enumerable);

            return string.Join(", ", enumerable);
        }
    }
}
