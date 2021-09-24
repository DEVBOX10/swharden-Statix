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

        public SingleArticle(string themeDirectory)
        {
            string templateFile = Path.Combine(themeDirectory, FILENAME);
            TemplateHtml = File.ReadAllText(templateFile);
        }

        public string GetHtml(Header header, string content, PageUrls urls)
        {
            string html = TemplateHtml;
            html = html.Replace("{{TITLE}}", header.Title);
            html = html.Replace("{{DESCRIPTION}}", header.Description);
            html = html.Replace("{{BASE_HREF}}", urls.PageWithSlash);
            html = html.Replace("{{URL_SITE}}", urls.Site);
            html = html.Replace("{{HREF_SOURCE}}", urls.PageSourceWithSlash);
            html = html.Replace("{{CONTENT}}", content);
            return html;
        }

        public void SaveHtml(Header header, string content, PageUrls urls, string saveAs)
        {
            string html = GetHtml(header, content, urls);
            File.WriteAllText(saveAs, html);
        }
    }
}
