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
                string line = lines[i];
                if (line.StartsWith("<h"))
                {
                    line = line.Trim();
                    int level = int.Parse(line.Substring(2, 1));
                    string title = Regex.Replace(line, "<.*?>", "");
                    string url = SafeUrl(title);
                    lines[i] = $"<h{level} id='{url}'><a href='#{url}' class='text-dark'>{title}</a></h{level}>";
                }
            }

            return lines;
        }

        public static string SafeUrl(string text)
        {
            var validChars = text
                .ToLower()
                .Replace(" ", "-")
                .ToCharArray()
                .Where(x => char.IsLetterOrDigit(x) || x == '-');

            text = string.Join("", validChars);

            while (text.Contains("--"))
                text = text.Replace("--", "-");
            text = text.Trim('-');

            return text;
        }
    }
}
