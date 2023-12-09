using System;
using Spectre.Console;

namespace ARJE.Utils.Spectre.Console
{
    public static class SelectionPromptUtils
    {
        public static Func<T, string> CreateStyledConverter<T>(Func<T, Style> converter)
            where T : notnull
        {
            return (choice) =>
            {
                string? choiceString = choice.ToString();
                if (choiceString == null)
                {
                    return null!;
                }

                Style style = converter.Invoke(choice);
                return StyleUtils.MarkupWrap(choiceString, style);
            };
        }
    }
}
