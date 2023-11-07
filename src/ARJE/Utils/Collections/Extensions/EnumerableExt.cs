using System;
using System.Collections.Generic;
using System.Linq;

namespace ARJE.Utils.Collections.Extensions
{
    public static class EnumerableExt
    {
        public static IEnumerable<(T Item, int Index)> Enumerated<T>(this IEnumerable<T> self, int startIndex = 0)
        {
            ArgumentNullException.ThrowIfNull(self);

            return self.Select((item, index) => (item, index + startIndex));
        }
    }
}
