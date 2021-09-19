using System;
using System.Collections.Generic;
using System.Text;

namespace Statix
{
    /// <summary>
    /// This class stores content of a markdown article with header frontmatter removed.
    /// </summary>
    public class Content
    {
        public  string Markdown { get; private set; }
        public  string HTML { get; private set; }

        public Content(string md)
        {
            ProcessLines(md.Split("\n"));
        }
        public Content(string[] lines)
        {
            ProcessLines(lines);
        }

        private void ProcessLines(string[] lines)
        {
            int firstContentLine = 0;

            if (lines[0].Trim() == "---")
            {
                for (int i = 1; i < lines.Length; i++)
                {
                    if (lines[i].Trim() == "---")
                    {
                        firstContentLine = i + 1;
                        break;
                    }
                }
            }

            int contentLineCount = lines.Length - firstContentLine;
            Markdown = string.Join('\n', lines, firstContentLine, contentLineCount);
            HTML = Markdig.Markdown.ToHtml(Markdown);
        }
    }
}
