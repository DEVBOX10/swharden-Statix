using System;
using System.Collections.Generic;
using System.Text;

namespace Statix
{
    /// <summary>
    /// An article holds HTML information from a single Markdown source.
    /// Articles have frontmatter headers and typically one HTML page displays a single article.
    /// Blog-style websites may show multiple articles on one page and use pagination to organize articles.
    /// </summary>
    public class Article
    {
        public readonly Header Header;
        public readonly Content Content;

        public Article(string markdown)
        {
            string[] mdLines = markdown.Split("\n");
            Header = new Header(mdLines);
            Content = new Content(mdLines);
        }
    }
}
