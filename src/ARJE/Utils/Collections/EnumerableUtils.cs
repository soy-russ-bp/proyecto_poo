using System.Collections.Generic;
using System.Linq;

namespace ARJE.Utils.Collections
{
    public static class EnumerableUtils
    {
        public static IEnumerable<T> FilterNull<T>(IEnumerable<T?> array)
        {
            return array.Where(e => e != null) as IEnumerable<T>;
        }
    }
}
