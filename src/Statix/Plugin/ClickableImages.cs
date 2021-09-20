using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Statix.Plugin
{
    public class ClickableImages : IMarkdownPlugin
    {
        private string[] ImageExtensions = { ".jpg", ".jpeg", ".gif", ".png" };

        public string[] Apply(string[] lines, int[] linesWithoutCode)
        {
            foreach (int i in linesWithoutCode)
            {
                if (IMarkdownPlugin.IsMagicLine(lines[i]))
                {
                    string url = IMarkdownPlugin.MagicUrl(lines[i]);
                    string extension = System.IO.Path.GetExtension(url).ToLower();
                    if (ImageExtensions.Contains(extension))
                        lines[i] = $"<a href='{url}' class='img-fluid'><img src='{url}' /></a>";
                }
            }
            return lines;
        }
    }
}
