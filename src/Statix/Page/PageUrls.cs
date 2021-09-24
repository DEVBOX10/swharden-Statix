using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Statix.Page
{
    public class PageUrls
    {
        public readonly string Site;

        public readonly string Page;

        public readonly string PageSource;

        public string SiteWithSlash => Site + "/";
        public string PageWithSlash => Page + "/";
        public string PageSourceWithSlash => PageSource + "/";

        public PageUrls(string mdFilePath, string contentRootPath, string siteRootUrl, string sourceRootUrl)
        {
            Site = siteRootUrl.TrimEnd('/');
            Page = (SiteWithSlash + GetRelativeUrl(contentRootPath, mdFilePath)).TrimEnd('/');
            PageSource = (SiteWithSlash + sourceRootUrl).TrimEnd('/');
        }

        private string GetRelativeUrl(string rootFolder, string mdFile)
        {
            // TODO: better cross-platform way to do this?
            rootFolder = Path.GetFullPath(rootFolder).TrimEnd('/').TrimEnd('\\');
            string mdFolder = Path.GetFullPath(Path.GetDirectoryName(mdFile));
            string relativeFolder = mdFolder.Replace(rootFolder, "");
            string relativeUrl = relativeFolder.Replace("\\", "/");
            return relativeUrl;
        }
    }
}
