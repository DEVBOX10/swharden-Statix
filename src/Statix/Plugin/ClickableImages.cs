using System;
using System.Collections.Generic;
using System.Text;

namespace Statix.Plugin
{
    public class ClickableImages : IMarkdownPlugin
    {
        public string[] Apply(string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (IMarkdownPlugin.IsMagicLine(lines[i]))
                {
                    string url = IMarkdownPlugin.MagicUrl(lines[i]);
                    lines[i] = $"<a href='{url}'><img src='{url}' /></a>";
                }
            }
            return lines;
        }
    }
}
