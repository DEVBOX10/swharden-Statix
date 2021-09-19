using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Statix.Page
{
    public class SingleArticle
    {
        private readonly string TemplateHtml;

        public static readonly string FILENAME = "article-single.html";

        public SingleArticle(DirectoryInfo themeDirectory)
        {
            string templateFile = Path.Combine(themeDirectory.FullName, FILENAME);
            TemplateHtml = File.ReadAllText(templateFile);
        }
        public string GetHtml(string title, string description, string content, Stopwatch sw)
        {
            string html = TemplateHtml.Replace("{{ ", "{{").Replace(" }}", "}}");
            html = html.Replace("{{title}}", title);
            html = html.Replace("{{description}}", description);
            html = html.Replace("{{buildTimeMilliseconds}}", $"{sw.Elapsed.TotalMilliseconds:F3}");
            html = html.Replace("{{content}}", content);
            return html;
        }
    }
}
