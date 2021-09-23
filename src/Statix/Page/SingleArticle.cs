﻿using System;
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

        public string GetHtml(string title, string description, string content, string sourceUrl, string baseUrl, string siteUrlBase)
        {
            string html = TemplateHtml;
            html = html.Replace("{{TITLE}}", title);
            html = html.Replace("{{DESCRIPTION}}", description);
            html = html.Replace("{{URL_FOLDER}}", baseUrl.TrimEnd('/'));
            html = html.Replace("{{URL_HOME}}", siteUrlBase.TrimEnd('/'));
            html = html.Replace("{{URL_SOURCE}}", sourceUrl.TrimEnd('/'));
            html = html.Replace("{{CONTENT}}", content);
            return html;
        }
    }
}
