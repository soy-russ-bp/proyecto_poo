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
                string styledChoice = StyleUtils.Wrap(choice.Text, choice.Style);
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
            obj.Converter = (choice) =>
            {
                string? choiceString = choice.ToString();
                if (choiceString == null)
                {
                    return null!;
                }

                Style style = converter.Invoke(choice);
                return StyleUtils.Wrap(choiceString, style);
            };
            return obj;
        }
    }
}
