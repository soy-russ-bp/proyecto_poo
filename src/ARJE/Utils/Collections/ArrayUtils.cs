using System;

namespace ARJE.Utils.Collections
{
    public static class ArrayUtils
    {
        public static T[] FilterNull<T>(T?[] array)
        {
            return Array.FindAll(array, e => e != null) as T[];
        }
    }
}
