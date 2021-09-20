using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Statix.Plugin
{
    public struct Heading
    {
        public readonly int Level;
        public readonly string Title;
        public string URL => SafeUrl(Title);

        public Heading(int level, string title)
        {
            Level = level;
            Title = title;
        }

        public static Heading FromMarkdown(string line)
        {
            int level = line.IndexOf(' ');
            string title = line.Substring(level, line.Length - level);
            return new Heading(level, title);
        }

        public static Heading FromHtml(string line)
        {
            int level = int.Parse(line.Substring(2, 1));
            string title = Regex.Replace(line, "<.*?>", "");
            return new Heading(level, title);
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
