using System;
using System.Collections.Generic;
using System.Text;

namespace Statix.Plugin
{
    class TableOfContents : IMarkdownPlugin
    {
        public string[] Apply(string[] lines, int[] linesWithoutCode)
        {
            List<Heading> headings = new List<Heading>();

            foreach (int i in linesWithoutCode)
            {
                string line = lines[i].Trim();

                if (line == "![](TOC)")
                    headings.Clear();

                if (line.StartsWith("#") && line.Contains("# "))
                    headings.Add(Heading.FromMarkdown(line));
            }

            StringBuilder sb = new StringBuilder();
            foreach (Heading heading in headings)
            {
                for (int i = 0; i < heading.Level - 1; i++)
                    sb.Append("&nbsp;&nbsp;");

                sb.Append($"<a href='#{heading.URL}'>");

                if (heading.Level <= 2)
                    sb.Append($"<b>{heading.Title}</b>");
                else
                    sb.Append($"{heading.Title}");

                sb.Append($"</a><br>\n");
            }

            foreach (int i in linesWithoutCode)
            {
                string line = lines[i].Trim();
                if (line == "![](TOC)")
                    lines[i] = sb.ToString();
            }

            return lines;
        }
    }
}
