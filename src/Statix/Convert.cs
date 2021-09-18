using System;

namespace Statix
{
    public static class Convert
    {
        public static string ToHtml(string markdown)
        {
            return Markdig.Markdown.ToHtml(markdown);
        }
    }
}
