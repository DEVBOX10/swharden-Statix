using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Statix
{
    public static class Generate
    {
        /// <summary>
        /// Walk a folder tree and wherever index.md is found generate an index.html
        /// </summary>
        public static void SingleArticlePages(string contentFolder, string templateFolder)
        {
            if (!Directory.Exists(contentFolder))
                throw new ArgumentException($"{nameof(contentFolder)} not found: {contentFolder}");

            if (!Directory.Exists(templateFolder))
                throw new ArgumentException($"{nameof(templateFolder)} not found: {templateFolder}");

            string[] mdFilePaths = FindIndexMarkdownFiles(contentFolder);
            string templatePath = Path.Combine(templateFolder, FileName.TEMPLATE_SINGLE_ARTICLE);
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

        private static string[] FindIndexMarkdownFiles(string folderPath) =>
            FindIndexMarkdownFiles(new DirectoryInfo(folderPath));

        private static string[] FindIndexMarkdownFiles(DirectoryInfo root)
        {
            var mdFilePaths = new List<string>();

            string mdFilePath = Path.Combine(root.FullName, FileName.INDEX_MD);
            if (System.IO.File.Exists(mdFilePath))
                mdFilePaths.Add(mdFilePath);

            foreach (DirectoryInfo dir in root.GetDirectories())
                mdFilePaths.AddRange(FindIndexMarkdownFiles(dir));

            return mdFilePaths.ToArray();
        }
    }
}
