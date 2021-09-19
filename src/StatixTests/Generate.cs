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

        [Test]
        public void Test_Build_SampleSite()
        {
            Statix.Generate.SingleArticlePages(CONTENT, THEME);
        }
    }
}
