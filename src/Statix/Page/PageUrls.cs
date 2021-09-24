using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Statix.Page
{
    public class PageUrls
    {
        public readonly string SiteRootUrl;
        public readonly string ThisFolderUrl;
        public readonly string PageSourceUrl;
        public readonly string RelativeUrl;

        public PageUrls(string mdFilePath, string contentRootPath, string siteRootUrl, string sourceRootUrl)
        {
            // TODO: use more descriptive types
            RelativeUrl = GetRelativeUrl(contentRootPath, mdFilePath);
            SiteRootUrl = siteRootUrl.TrimEnd('/');
            PageSourceUrl = sourceRootUrl.TrimEnd('/') + "/" + RelativeUrl + "/" + Path.GetFileName(mdFilePath);
            ThisFolderUrl = siteRootUrl.TrimEnd('/') + "/" + RelativeUrl;
        }

        private string GetRelativeUrl(string rootFolder, string mdFile)
        {
            // TODO: better cross-platform way to do this?
            rootFolder = Path.GetFullPath(rootFolder).TrimEnd('/').TrimEnd('\\');
            string mdFolder = Path.GetFullPath(Path.GetDirectoryName(mdFile));
            string relativeFolder = mdFolder.Replace(rootFolder, "");
            string relativeUrl = relativeFolder.Replace("\\", "/");
            return relativeUrl.Trim('/');
        }
    }
}
