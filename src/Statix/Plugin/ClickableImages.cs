using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Statix.Plugin
{
    public class ClickableImages : IMarkdownPlugin
    {
        private string[] ImageExtensions = { ".jpg", ".jpeg", ".gif", ".png" };

        public string[] Apply(string[] lines)
        {
            bool inCodeBlock = false;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("```"))
                    inCodeBlock = !inCodeBlock;

                if (inCodeBlock)
                    continue;

                if (IMarkdownPlugin.IsMagicLine(lines[i]))
                {
                    string url = IMarkdownPlugin.MagicUrl(lines[i]);
                    string extension = System.IO.Path.GetExtension(url).ToLower();
                    if (ImageExtensions.Contains(extension))
                        lines[i] = $"<a href='{url}'><img src='{url}' /></a>";
                }
            }
            return lines;
        }
    }
}
