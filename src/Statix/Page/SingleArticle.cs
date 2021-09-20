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

        public string GetHtml(string title, string description, string content, string sourceUrl, string baseUrl)
        {
            string html = TemplateHtml;
            html = html.Replace("{{TITLE}}", title);
            html = html.Replace("{{DESCRIPTION}}", description);
            html = html.Replace("{{URL_BASE}}", baseUrl);
            html = html.Replace("{{CONTENT}}", content);
            html = html.Replace("{{UTC_DATE}}", DateTime.UtcNow.ToShortDateString());
            html = html.Replace("{{UTC_TIME}}", DateTime.UtcNow.ToShortTimeString());
            html = html.Replace("{{URL_SOURCE}}", sourceUrl);
            return html;
        }
    }
}
