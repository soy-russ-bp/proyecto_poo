using System;
using System.Collections.Generic;
using EnumsNET;
using Spectre.Console;

namespace ARJE.Utils.Spectre.Console.Extensions
{
    public static class SelectionPromptExt
    {
        public static SelectionPrompt<string> AddStyledChoices(
            this SelectionPrompt<string> obj,
            params (string Text, Style Style)[] choices)
        {
            foreach (var choice in choices)
            {
                string styledChoice = StyleUtils.MarkupWrap(choice.Text, choice.Style);
                obj.AddChoice(styledChoice);
            }

            return obj;
        }

        public static SelectionPrompt<TEnum> AddEnumChoices<TEnum>(this SelectionPrompt<TEnum> obj)
            where TEnum : struct, Enum
        {
            IReadOnlyList<TEnum> choices = Enums.GetValues<TEnum>();
            obj.AddChoices(choices);
            return obj;
        }

        public static SelectionPrompt<T> UseStyledConverter<T>(
            this SelectionPrompt<T> obj,
            Func<T, Style> converter)
            where T : notnull
        {
            Func<T, string> styledConverter = SelectionPromptUtils.CreateStyledConverter(converter);
            obj.UseConverter(styledConverter);
            return obj;
        }
    }
}
