using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Statix
{
    public static class Generate
    {
        public static void SingleArticlePages(string configFilePath)
        {
            var c = new ConfigFile(configFilePath);
            SingleArticlePages(c.ContentDirectory, c.ThemeDirectory);
        }

        private static void SingleArticlePages(DirectoryInfo contentDirectory, DirectoryInfo themeDirectory)
        {
            Console.WriteLine($"Searching content: {contentDirectory.FullName}");
            Console.WriteLine($"Using theme: {themeDirectory.FullName}");

            string[] mdFilePaths = FindIndexMarkdownFiles(contentDirectory);
            string templatePath = Path.Combine(themeDirectory.FullName, FileName.TEMPLATE_SINGLE_ARTICLE);
            string template = File.ReadAllText(templatePath);

            foreach (string mdFilePath in mdFilePaths)
            {
                string md = File.ReadAllText(mdFilePath);
                SingleArticlePage pg = new SingleArticlePage(md, template);
                string outPath = Path.Combine(Path.GetDirectoryName(mdFilePath), FileName.INDEX_HTML);
                Console.WriteLine(outPath);
                File.WriteAllText(outPath, pg.HTML);
            }
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
