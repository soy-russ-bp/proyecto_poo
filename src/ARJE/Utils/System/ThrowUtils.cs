using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ARJE.Utils.System
{
    public static class ThrowUtils
    {
        public static T ReturnOnlyIfNotNull<T>(
            [NotNull] T? argument,
            [CallerArgumentExpression(nameof(argument))] string? paramName = null)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(argument, paramName);

            return argument;
        }
    }
}
