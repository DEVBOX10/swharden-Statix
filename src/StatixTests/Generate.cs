using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace StatixTests
{
    class Generate
    {
        private readonly string CONFIG_PATH = Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../../../sample/config.json");

        [Test]
        public void Test_Build_SampleSite()
        {
            Statix.Generate.SingleArticlePages(CONFIG_PATH);
        }
    }
}
