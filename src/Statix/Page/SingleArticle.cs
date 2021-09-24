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
            html = html.Replace("{{TITLE }}", header.Title);
            html = html.Replace("{{DESCRIPTION}}", header.Description);
            html = html.Replace("{{URL_SITE_ROOT}}", urls.SiteRootUrl);
            html = html.Replace("{{URL_PAGE_BASE}}", urls.ThisFolderUrl + "/");
            html = html.Replace("{{URL_PAGE_SOURCE}}", urls.PageSourceUrl);
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
