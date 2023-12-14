using System;

namespace ARJE.Utils.System.Extensions
{
    public static class LazyExt
    {
        public static T? GetValueOrNull<T>(this Lazy<T> lazy)
            where T : class
        {
            if (lazy.IsValueCreated)
            {
                return lazy.Value;
            }

            return null;
        }
    }
}
