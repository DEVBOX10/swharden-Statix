using System;
using System.Collections.Generic;
using System.IO;

namespace Statix
{
    public static class Convert
    {
        public static string Text(string markdown)
        {
            return Markdig.Markdown.ToHtml(markdown);
        }

        public static void File(string filePath)
        {
            filePath = Path.GetFullPath(filePath);

            var fi = new FileInfo(filePath);
            if (!fi.Exists)
                throw new ArgumentException($"not a file: {fi}");

            string ext = Path.GetExtension(filePath);
            if (ext != ".md")
                throw new ArgumentException($"must be a .md file: {fi}");

            string md = System.IO.File.ReadAllText(filePath);
            string html = Text(md);

            string inFolder = Path.GetDirectoryName(filePath);
            string inBaseName = Path.GetFileNameWithoutExtension(filePath);
            string outFilePath = Path.Combine(inFolder, inBaseName + ".html");
            System.IO.File.WriteAllText(outFilePath, html);
        }

        public static string[] FindMdFiles(string folderPath)
        {
            DirectoryInfo di = new DirectoryInfo(folderPath);
            return FindMdFiles(di);
        }

        public static string[] FindMdFiles(DirectoryInfo root)
        {
            var mdFilePaths = new List<string>();

            string mdFilePath = Path.Combine(root.FullName, "index.md");
            if (System.IO.File.Exists(mdFilePath))
                mdFilePaths.Add(mdFilePath);

            foreach (DirectoryInfo dir in root.GetDirectories())
                mdFilePaths.AddRange(FindMdFiles(dir));

            return mdFilePaths.ToArray();
        }
    }
}
