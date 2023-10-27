using System;
using Spectre.Console;

namespace ARJE.Utils.Spectre.Console
{
    public static class StyleUtils
    {
        public static string Wrap(string text, Color? foreground = null, Color? background = null, Decoration? decoration = null, string? link = null)
        {
            ArgumentNullException.ThrowIfNull(text);

            var style = new Style(foreground, background, decoration, link);
            return Wrap(text, style);
        }

        public static string Wrap(string text, Style style)
        {
            ArgumentNullException.ThrowIfNull(text);
            ArgumentNullException.ThrowIfNull(style);

            string markup = style.ToMarkup();
            string escapedText = Markup.Escape(text);
            return $"[{markup}]{escapedText}[/]";
        }
    }
}
