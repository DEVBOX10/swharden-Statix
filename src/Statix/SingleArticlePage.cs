using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace Statix
{
    public class SingleArticlePage
    {
        public readonly string HTML;

        public SingleArticlePage(string md, string template)
        {
            Stopwatch sw = Stopwatch.StartNew();
            Article article = new Article(md);
            HTML = template.Replace("{{ ", "{{").Replace(" }}", "}}");
            HTML = HTML.Replace("{{content}}", article.Content.HTML);
            HTML = HTML.Replace("{{title}}", article.Header.Title);
            HTML = HTML.Replace("{{description}}", article.Header.Description);
            HTML = HTML.Replace("{{date}}", article.Header.Date);
            HTML = HTML.Replace("{{tags}}", string.Join(", ", article.Header.Tags));
            HTML = HTML.Replace("{{buildTimeMilliseconds}}", $"{sw.Elapsed.TotalMilliseconds:F3}");
        }
    }
}
