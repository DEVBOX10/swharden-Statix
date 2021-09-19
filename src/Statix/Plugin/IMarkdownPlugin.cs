using System;
using System.Collections.Generic;
using System.Text;

namespace Statix.Plugin
{
    public interface IMarkdownPlugin
    {
        string[] Apply(string[] lines);

        public static bool IsMagicLine(string markdownLine)
        {
            markdownLine = markdownLine.Trim();
            return markdownLine.StartsWith("![](") && markdownLine.EndsWith(")");
        }

        public static string MagicUrl(string markdownLine)
        {
            return markdownLine.Trim()[4..^1];
        }
    }
}
