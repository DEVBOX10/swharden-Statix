using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace StatixTests
{
    class Generate
    {
        private readonly DirectoryInfo CONTENT = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../../../sample/content"));
        private readonly DirectoryInfo THEME = new DirectoryInfo(Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../../../sample/themes/statixdemo"));
        private readonly string SOURCE_URL = "https://github.com/swharden/Statix/tree/main/sample/content";
        [Test]
        public void Test_Build_SampleSite()
        {
            Statix.Generate.SingleArticlePages(CONTENT, THEME, SOURCE_URL);
        }
    }
}
