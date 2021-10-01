using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Statix.Plugin
{
    public class HeadingAnchors : IHtmlPlugin
    {
        public string[] Apply(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i].Trim();
                if (line.StartsWith("<h") && line.EndsWith(">") && !line.StartsWith("<hr"))
                {
                    Heading heading = Heading.FromHtml(line);
                    lines[i] =
                        $"<h{heading.Level} id='{heading.URL}'>" +
                        $"<a href='#{heading.URL}' class='text-dark'>{heading.Title}</a>" +
                        $"</h{heading.Level}>";
                }
            }

            return lines;
        }
    }
}
