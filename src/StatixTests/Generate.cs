using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace StatixTests
{
    class Generate
    {
        private readonly string CONTENT_FOLDER = Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../../../sample/content");
        private readonly string TEMPLATE_FOLDER = Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../../../sample/template");

        [Test]
        public void Test_Build_SampleSite()
        {
            Statix.Generate.SingleArticlePages(CONTENT_FOLDER, TEMPLATE_FOLDER);
        }
    }
}
