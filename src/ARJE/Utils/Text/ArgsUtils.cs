using System;
using System.Collections.Generic;
using System.Linq;
using ARJE.Utils.Collections;

namespace ARJE.Utils.Text
{
    public static class ArgsUtils
    {
        public static string Join(params string?[] args)
        {
            return Join(args.AsEnumerable());
        }

        public static string Join(IEnumerable<string?> args)
        {
            ArgumentNullException.ThrowIfNull(args);

            string[] argsArray = EnumerableUtils.FilterNull(args).ToArray();
            if (argsArray.Length == 0)
            {
                return string.Empty;
            }

            IEnumerable<string> wrappedArgs = argsArray.Select(arg => $"\"{arg}\"");
            return string.Join(" ", wrappedArgs);
        }
    }
}
