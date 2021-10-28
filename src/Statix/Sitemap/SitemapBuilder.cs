using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Statix.Sitemap
{
    /// <summary>
    /// Sitemap generator (sitemaps.org format)
    /// Instantiate, Add(), then Get()
    /// </summary>
    public class SitemapBuilder
    {
        private readonly List<Url> URLs = new List<Url>();

        public int Count => URLs.Count;

        public SitemapBuilder()
        {
        }

        public void AddScan(string localRoot, string urlRoot)
        {
            if (!urlRoot.EndsWith("/"))
                urlRoot += "/";

            localRoot = Path.GetFullPath(localRoot.TrimEnd('/'));
            string[] mdIndexes = Directory.GetFiles(localRoot, "index.md", SearchOption.AllDirectories);
            string[] htmlIndexes = Directory.GetFiles(localRoot, "index.html", SearchOption.AllDirectories);
            string[] indexFolders = mdIndexes.Concat(htmlIndexes).Select(x => Path.GetDirectoryName(x)).ToArray();

            HashSet<string> seenUrls = new HashSet<string>();

            foreach (string indexFolder in indexFolders)
            {
                string relativePath = indexFolder.Replace(localRoot, "").Replace("\\", "/").Trim('/');
                string url = urlRoot + relativePath;

                if (seenUrls.Contains(url))
                    continue;

                Add(url);
                seenUrls.Add(url);
            }
        }

        public void Add(Url url)
        {
            URLs.Add(url);
        }

        public void Add(string url)
        {
            URLs.Add(new Url() { Location = url });
        }

        public void AddNow(string url)
        {
            URLs.Add(new Url()
            {
                Location = url,
                Modified = DateTime.UtcNow
            });
        }

        /// <summary>
        /// Return a text sitemap.
        /// This will be a line-separated text block with one URL per line.
        /// https://developers.google.com/search/docs/advanced/sitemaps/build-sitemap#text
        /// </summary>
        public string GetText()
        {
            return string.Join("\n", URLs.Select(x => x.Location));
        }

        /// <summary>
        /// Return the sitemap in RSS format.
        /// https://developers.google.com/search/docs/advanced/sitemaps/build-sitemap#rss
        /// </summary>
        [Obsolete("not implemented", true)]
        public string GetRSS()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return the sitemap in XML format.
        /// https://developers.google.com/search/docs/advanced/sitemaps/build-sitemap#xml
        /// </summary>
        public string GetXML()
        {
            // format definition: https://www.sitemaps.org/protocol.html

            XDeclaration declaration = new XDeclaration(version: "1.0", encoding: "UTF-8", standalone: "yes");
            XDocument doc = new XDocument(declaration);

            XNamespace xn = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XElement root = new XElement(xn + "urlset");
            doc.Add(root);

            doc.Add(new XComment($"Statix {Generator.Version} {DateTime.UtcNow} UTC"));

            foreach (Url url in URLs)
            {
                XElement elUrl = new XElement("url");
                root.Add(elUrl);

                elUrl.Add(new XElement("loc", url.Location));

                if (url.Modified > DateTime.MinValue)
                    elUrl.Add(new XElement("lastmod", url.Modified.ToString("s", System.Globalization.DateTimeFormatInfo.InvariantInfo)));

                if (url.ChangeFreq != ChangeFreq.Unknown)
                    elUrl.Add(new XElement("changefreq", url.ChangeFreq));

                if (!double.IsNaN(url.Priority))
                    elUrl.Add(new XElement("priority", url.Priority));
            }

            var ms = new MemoryStream();
            doc.Save(ms, SaveOptions.OmitDuplicateNamespaces);
            ms.Flush();

            string xml = Encoding.UTF8.GetString(ms.ToArray());
            xml = xml.Replace(" xmlns=\"\"", "");
            return xml;
        }
    }
}
