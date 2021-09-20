using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Statix.Plugin
{
    public interface IMarkdownPlugin
    {
        string[] Apply(string[] lines, int[] linesWithoutCode);

        public static bool IsMagicLine(string markdownLine)
        {
            markdownLine = markdownLine.Trim();
            const string magicStart = "![](";
            const string magicEnd = ")";
            return markdownLine.StartsWith(magicStart) &&
                markdownLine.EndsWith(magicEnd) &&
                markdownLine.LastIndexOf(magicStart) == 0;
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

                if (!inCodeBlock)
                    lines.Add(i);
            }

            return lines.ToArray();
        }
    }
}
