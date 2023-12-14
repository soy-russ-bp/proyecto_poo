using System;
using System.Diagnostics.CodeAnalysis;

namespace ARJE.Utils.Collections
{
    public static class ArrayUtils
    {
        public static T[] FilterNull<T>(T?[] array)
        {
            return Array.FindAll(array, e => e != null) as T[];
        }

        public static void EmptyIfNull<T>([NotNull] ref T[]? array)
        {
            array ??= Array.Empty<T>();
        }
    }
}
