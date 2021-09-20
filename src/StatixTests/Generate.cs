using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace StatixTests
{
    class Generate
    {
        [Test]
        public void Test_Build_SampleSite()
        {
            Statix.Generate.SingleArticlePages(
                contentDirectory: new DirectoryInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../../../sample/content")),
                themeDirectory: new DirectoryInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../../../sample/themes/statixdemo")),
                sourceUrlBase: "https://github.com/swharden/Statix/tree/main/sample/content",
                siteUrlBase: "http://localhost:8080");
        }
    }
}
