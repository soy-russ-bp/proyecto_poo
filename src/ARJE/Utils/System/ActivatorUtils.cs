using System;

namespace ARJE.Utils.System
{
    public static class ActivatorUtils
    {
        public static T CreateInstance<T>(params object?[]? args)
        {
            return (T)Activator.CreateInstance(typeof(T), args)!;
        }
    }
}
