using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Statix
{
    public static class Generate
    {
        public static readonly string FILENAME_INDEX_HTML = "index.html";
        public static readonly string FILENAME_INDEX_MD = "index.md";

        public static void SingleArticlePages(DirectoryInfo contentDirectory, DirectoryInfo themeDirectory, string sourceUrlBase)
        {
            Plugin.IMarkdownPlugin[] markdownPlugins =
            {
                new Plugin.ClickableImages(),
                new Plugin.YouTube(),
                new Plugin.TableOfContents(),
            };

            Plugin.IHtmlPlugin[] htmlPlugins =
            {
                new Plugin.HeadingAnchors(),
            };

            Stopwatch sw = Stopwatch.StartNew();

            Console.WriteLine($"Loading templates for theme: {themeDirectory.FullName}");
            Page.SingleArticle page = new Page.SingleArticle(themeDirectory);

            Console.WriteLine($"Searching directory tree: {contentDirectory.FullName}");
            string[] mdFilePaths = FindIndexMarkdownFiles(contentDirectory);

            for (int i = 0; i < mdFilePaths.Length; i++)
            {
                // read markdown file
                string mdFilePath = mdFilePaths[i];
                Console.WriteLine($"[{i + 1}/{mdFilePaths.Length}] {mdFilePath}");
                string[] mdLines = File.ReadAllLines(mdFilePath);

                // parse the header and remove it from the markdown lines
                Header header = new Header(mdLines);
                mdLines = mdLines[header.FirstContentLine..];

                // apply markdown plugins
                int[] linesWithoutCode = Plugin.IMarkdownPlugin.GetLinesWithoutCode(mdLines);
                foreach (Plugin.IMarkdownPlugin p in markdownPlugins)
                    mdLines = p.Apply(mdLines, linesWithoutCode);

                // convert to HTML
                string md = string.Join('\n', mdLines);
                string html = Markdig.Markdown.ToHtml(md);

                // apply HTML plugins
                string[] htmlLines = html.Split("\n");
                foreach (var p in htmlPlugins)
                    htmlLines = p.Apply(htmlLines);
                html = string.Join("\n", htmlLines);

                // wrap the article in the page (applying the template)
                string relativePath = mdFilePath.Replace(contentDirectory.FullName, "");
                string sourceUrl = sourceUrlBase.Trim('/') + "/" + relativePath.Trim('/');
                html = page.GetHtml(header.Title, header.Description, html, sourceUrl);

                string outPath = Path.Combine(Path.GetDirectoryName(mdFilePath), FILENAME_INDEX_HTML);
                File.WriteAllText(outPath, html);
            }

            Console.WriteLine($"Generated {mdFilePaths.Length} pages in {sw.Elapsed.TotalMilliseconds:F3} milliseconds.");
        }

        private static string[] FindIndexMarkdownFiles(DirectoryInfo root)
        {
            var mdFilePaths = new List<string>();

            string mdFilePath = Path.Combine(root.FullName, FILENAME_INDEX_MD);
            if (File.Exists(mdFilePath))
                mdFilePaths.Add(mdFilePath);

            foreach (DirectoryInfo dir in root.GetDirectories())
                mdFilePaths.AddRange(FindIndexMarkdownFiles(dir));

            return mdFilePaths.ToArray();
        }
    }
}
