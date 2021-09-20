using System;
using System.Collections.Generic;
using System.Text;

namespace Statix.Plugin
{
    class YouTube : IMarkdownPlugin
    {
        public string[] Apply(string[] lines, int[] linesWithoutCode)
        {
            foreach (int i in linesWithoutCode)
            {
                if (IMarkdownPlugin.IsMagicLine(lines[i]))
                {
                    string url = IMarkdownPlugin.MagicUrl(lines[i]);

                    if (url.ToLower().Contains("://youtu"))
                    {
                        url = "https://www.youtube.com/embed/" + System.IO.Path.GetFileName(url);
                        lines[i] =
                            $"<div class='ratio ratio-16x9 my-5 youTubeVideo'>" +
                            $"<object class='border border-dark shadow' data='{url}'></object>" +
                            $"</div>";
                    }
                }
            }
            return lines;
        }
    }
}
