using Markdig;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Statix
{
    public class Generator
    {
        /// <summary>
        /// Name of the file markdown file in each folder that will be used to generate the HTML page
        /// </summary>
        public string IndexMarkdownFilename = "index.md";

        /// <summary>
        /// Name of the HTML page to create in each folder where a markdown index file exists
        /// </summary>
        public string IndexHtmlFilename = "index.html";

        /// <summary>
        /// Plugins that run in order on the markdown before it is converted to HTML
        /// </summary>
        public readonly List<Plugin.IMarkdownPlugin> MarkdownPlugins = new List<Plugin.IMarkdownPlugin>();

        /// <summary>
        /// Plugins that run on the HTML after it is has been converged from markdown
        /// </summary>
        public readonly List<Plugin.IHtmlPlugin> HtmlPlugins = new List<Plugin.IHtmlPlugin>();

        /// <summary>
        /// Local folder containing markdown files that will become the root of the website.
        /// Folder path contains a trailing slash.
        /// </summary>
        public readonly string ContentFolder;

        /// <summary>
        /// Local folder containing HTML template files.
        /// Folder path contains a trailing slash.
        /// </summary>
        public readonly string ThemeFolder;

        /// <summary>
        /// Base URL to view source, typically "https://GitHub.com/USERNAME/PROJECT/blob/main/content/".
        /// Folder URL contains a trailing slash.
        /// </summary>
        public readonly string RootUrl;

        /// <summary>
        /// Website URL corresponding to the root of the local content folder
        /// Folder URL contains a trailing slash.
        /// </summary>
        public readonly string SourceUrl;

        /// <summary>
        /// Warn if the heading is missing or lacking important elements
        /// </summary>
        public bool ShowHeaderWarnings = true;

        /// <summary>
        /// Create a new static website generator.
        /// Customize behvior using public properties, then Generate() your static site.
        /// </summary>
        /// <param name="contentFolder">local folder containing website content</param>
        /// <param name="themeFolder">local folder containing template HTML files</param>
        /// <param name="sourceUrl">Base URL to view source, typically "https://GitHub.com/USERNAME/PROJECT/blob/main/content"</param>
        /// <param name="rootUrl">URL of the content directory (used for the header base href)</param>
        public Generator(string contentFolder, string themeFolder, string rootUrl, string sourceUrl)
        {
            if (!Directory.Exists(contentFolder))
                throw new ArgumentException("folder not found", nameof(ContentFolder));
            ContentFolder = Path.GetFullPath(contentFolder).Replace("\\", "/").TrimEnd('/') + '/';

            if (!Directory.Exists(themeFolder))
                throw new ArgumentException("folder not found", nameof(themeFolder));
            ThemeFolder = Path.GetFullPath(themeFolder).Replace("\\", "/").TrimEnd('/') + '/';

            if (!rootUrl.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentException("URL must start with HTTP", nameof(rootUrl));
            if (!rootUrl.EndsWith("/"))
                rootUrl += "/";
            RootUrl = rootUrl;

            if (!sourceUrl.StartsWith("http", StringComparison.InvariantCultureIgnoreCase))
                throw new ArgumentException("URL must start with HTTP", nameof(sourceUrl));
            if (!sourceUrl.EndsWith("/"))
                sourceUrl += "/";
            SourceUrl = sourceUrl;

            LoadDefaultPlugins();
        }

        public void LoadDefaultPlugins()
        {
            MarkdownPlugins.Clear();
            MarkdownPlugins.Add(new Plugin.ClickableImages());
            MarkdownPlugins.Add(new Plugin.YouTube());
            MarkdownPlugins.Add(new Plugin.TableOfContents());

            HtmlPlugins.Clear();
            HtmlPlugins.Add(new Plugin.HeadingAnchors());
        }

        /// <summary>
        /// Generate a static site by creating an index HTML file for every index Markdown file found.
        /// </summary>
        public void Generate()
        {
            Stopwatch sw = Stopwatch.StartNew();

            Page.SingleArticle page = new Page.SingleArticle(ThemeFolder);

            string[] mdFilePaths = FindIndexMarkdownFiles(new DirectoryInfo(ContentFolder));

            for (int i = 0; i < mdFilePaths.Length; i++)
            {
                // read markdown file
                string mdFilePath = Path.GetFullPath(mdFilePaths[i]);
                Console.WriteLine($"[{i + 1}/{mdFilePaths.Length}] {mdFilePath}");
                string[] mdLines = File.ReadAllLines(mdFilePath);

                // parse the header and remove it from the markdown lines
                Header header = new Header(mdLines);
                if (ShowHeaderWarnings)
                    page.ValidateHeader(header);
                mdLines = mdLines[header.FirstContentLine..];

                // apply markdown plugins
                int[] linesWithoutCode = Plugin.IMarkdownPlugin.GetLinesWithoutCode(mdLines);
                foreach (Plugin.IMarkdownPlugin p in MarkdownPlugins)
                    mdLines = p.Apply(mdLines, linesWithoutCode);

                // convert to HTML
                string md = string.Join('\n', mdLines);
                var pipeline = new MarkdownPipelineBuilder().UseAdvancedExtensions().Build();
                string mdHtml = Markdown.ToHtml(md, pipeline);

                // apply HTML plugins
                string[] htmlLines = mdHtml.Split("\n");
                foreach (var p in HtmlPlugins)
                    htmlLines = p.Apply(htmlLines);
                mdHtml = string.Join("\n", htmlLines);

                // wrap the content in the page template
                var urls = new Page.PageUrls(mdFilePath, ContentFolder, RootUrl, SourceUrl);
                string htmlFilePath = Path.Combine(Path.GetDirectoryName(mdFilePath), IndexHtmlFilename);
                page.SaveHtml(header, mdHtml, urls, htmlFilePath);
                Console.WriteLine();
            }

            Console.WriteLine($"Generated {mdFilePaths.Length} pages in {sw.Elapsed.TotalMilliseconds:F3} milliseconds.");
        }

        /// <summary>
        /// Recursively scan a directory tree and build an array containing paths to all markdown
        /// files found with the index filename.
        /// </summary>
        private string[] FindIndexMarkdownFiles(DirectoryInfo root)
        {
            var mdFilePaths = new List<string>();

            string mdFilePath = Path.Combine(root.FullName, IndexMarkdownFilename);
            if (File.Exists(mdFilePath))
                mdFilePaths.Add(mdFilePath);

            foreach (DirectoryInfo dir in root.GetDirectories())
                mdFilePaths.AddRange(FindIndexMarkdownFiles(dir));

            return mdFilePaths.ToArray();
        }
    }
}
