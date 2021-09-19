using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Statix
{
    public static class Generate
    {
        public static void SingleArticlePages(DirectoryInfo contentDirectory, DirectoryInfo themeDirectory)
        {
            Stopwatch sw = Stopwatch.StartNew();

            Console.WriteLine($"Loading templates for theme: {themeDirectory.FullName}");
            string templatePath = Path.Combine(themeDirectory.FullName, FileName.TEMPLATE_SINGLE_ARTICLE);
            string template = File.ReadAllText(templatePath);

            Console.WriteLine($"Searching directory tree: {contentDirectory.FullName}");
            string[] mdFilePaths = FindIndexMarkdownFiles(contentDirectory);

            for (int i = 0; i < mdFilePaths.Length; i++)
            {
                string mdFilePath = mdFilePaths[i];
                string md = File.ReadAllText(mdFilePath);
                Console.WriteLine($"[{i + 1}/{mdFilePaths.Length}] {mdFilePath}");
                SingleArticlePage pg = new SingleArticlePage(md, template);
                string outPath = Path.Combine(Path.GetDirectoryName(mdFilePath), FileName.INDEX_HTML);
                File.WriteAllText(outPath, pg.HTML);
            }

            Console.WriteLine($"Generated {mdFilePaths.Length} pages in {sw.Elapsed.TotalMilliseconds:F3} milliseconds.");
        }

        private static string[] FindIndexMarkdownFiles(DirectoryInfo root)
        {
            var mdFilePaths = new List<string>();

            string mdFilePath = Path.Combine(root.FullName, FileName.INDEX_MD);
            if (File.Exists(mdFilePath))
                mdFilePaths.Add(mdFilePath);

            foreach (DirectoryInfo dir in root.GetDirectories())
                mdFilePaths.AddRange(FindIndexMarkdownFiles(dir));

            return mdFilePaths.ToArray();
        }
    }
}
