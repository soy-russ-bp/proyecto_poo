using System;
using System.Linq;

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

            int notNullCount = array.Count(item => item != null);
            if (array.Length == notNullCount)
            {
                return array as T[];
            }

            var newArray = new T[notNullCount];
            for (int i = 0, newI = 0; i <= notNullCount; i++)
            {
                T? item = array[i];
                if (item != null)
                {
                    newArray[newI] = item;
                    newI++;
                }
            }

            return newArray;
        }
    }
}
