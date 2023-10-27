using System;
using Spectre.Console;

namespace ARJE.Utils.Spectre.Console
{
    public static class StyleUtils
    {
        public static string MarkupWrap(string text, Color? foreground = null, Color? background = null, Decoration? decoration = null, string? link = null, bool escape = true)
        {
            ArgumentNullException.ThrowIfNull(text);

            var style = new Style(foreground, background, decoration, link);
            return MarkupWrap(text, style, escape);
        }

        public static string MarkupWrap(string text, Style style, bool escape = true)
        {
            ArgumentNullException.ThrowIfNull(text);
            ArgumentNullException.ThrowIfNull(style);

            string markup = style.ToMarkup();
            string textToWrap = escape ? Markup.Escape(text) : text;
            return $"[{markup}]{textToWrap}[/]";
        }
    }
}
