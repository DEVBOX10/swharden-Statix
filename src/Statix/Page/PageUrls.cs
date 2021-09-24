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
            Site = RemoveTrailingSlashes(siteRootUrl);
            Page = SiteWithSlash + RemoveEdgeSlashes(GetRelativeUrl(contentRootPath, mdFilePath));
            PageSource = SiteWithSlash + RemoveTrailingSlashes(sourceRootUrl);
        }

        private string GetRelativeUrl(string rootFolder, string filePath)
        {
            string folderPath = Path.GetFullPath(Path.GetDirectoryName(filePath));
            string siteRootPath = Path.GetFullPath(rootFolder);
            string relativeFolderPath = folderPath.Replace(siteRootPath, "");
            return relativeFolderPath;
        }

        private string RemoveEdgeSlashes(string url) => RemoveLeadingSlashes(RemoveTrailingSlashes(url));

        private string RemoveLeadingSlashes(string url)
        {
            url = url.Replace("\\", "/");
            while (url.StartsWith("/"))
                url = url.TrimStart('/');
            return url;
        }

        private string RemoveTrailingSlashes(string url)
        {
            url = url.Replace("\\", "/");
            while (url.EndsWith("/"))
                url = url.TrimEnd('/');
            return url;
        }
    }
}
