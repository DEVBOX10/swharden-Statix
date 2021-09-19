using System;
using System.Collections.Generic;
using System.Text;

namespace Statix.Plugin
{
    class YouTube : IMarkdownPlugin
    {
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
