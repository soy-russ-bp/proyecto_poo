using System;

namespace ARJE.Utils.Collections
{
    public static class ArrayUtils
    {
        public static T[] FilterNull<T>(T?[]? array)
        {
            if (array == null)
            {
                return Array.Empty<T>();
            }

            return Array.FindAll(array, e => e != null) as T[];
        }
    }
}
