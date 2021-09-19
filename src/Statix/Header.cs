using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Statix
{
    /// <summary>
    /// The header class contains information about a markdown page defined by frontmatter.
    /// Frontmatter is a section of YAML at the top of the page surrounded by --- lines.
    /// </summary>
    public class Header
    {
        public string Title { get; private set; }
        public string Description { get; private set; }
        public string[] Tags { get; private set; } = new string[0];
        public string Date { get; private set; }
        public readonly bool Exists;

        public Header(string md)
        {
            ProcessLines(md.Split("\n"));
        }

        public Header(string[] lines)
        {
            ProcessLines(lines);
        }

        private void ProcessLines(string[] lines)
        {
            if (lines is null || lines.Length == 0)
                return;

            if (lines[0].Trim() != "---")
                return;

            for (int i = 1; i < lines.Length; i++)
            {
                if (lines[i].Trim() == "---")
                    break;
                else
                    ProcessHeaderLine(lines[i]);
            }
        }

        private void ProcessHeaderLine(string line)
        {
            // https://jekyllrb.com/docs/front-matter
            // TODO: use a YAML parser and add proper YAML support for lists

            if (!line.Contains(":"))
                return;

            var parts = line.Split(":", 2);
            string key = parts[0].Trim().ToLowerInvariant();
            string value = parts[1].Trim();

            if (key == "title")
                Title = value;
            else if (key == "description")
                Description = value;
            else if (key == "tags")
                Tags = value.Split(",").Select(x => x.Trim()).ToArray();
            else if (key == "date")
                Date = value;
        }
    }
}
