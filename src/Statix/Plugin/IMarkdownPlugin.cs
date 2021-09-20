using System;
using System.Collections.Generic;
using System.Text;

namespace Statix.Plugin
{
    public interface IMarkdownPlugin
    {
        string[] Apply(string[] lines, int[] linesWithoutCode);

        public static bool IsMagicLine(string markdownLine)
        {
            markdownLine = markdownLine.Trim();
            return markdownLine.StartsWith("![](") && markdownLine.EndsWith(")");
        }

        public static string MagicUrl(string markdownLine)
        {
            return markdownLine.Trim()[4..^1];
        }

        public static int[] GetLinesWithoutCode(string[] mdLines)
        {
            List<int> lines = new List<int>();

            bool inCodeBlock = false;
            for (int i = 0; i < mdLines.Length; i++)
            {
                if (mdLines[i].StartsWith("```"))
                {
                    inCodeBlock = !inCodeBlock;
                    continue;
                }

                if (inCodeBlock)
                    lines.Add(i);
            }

            return lines.ToArray();
        }
    }
}
