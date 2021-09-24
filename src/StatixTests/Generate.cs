using NUnit.Framework;
using System.IO;

namespace StatixTests
{
    class Generate
    {
        [Test]
        public void Test_Build_SampleSite()
        {
            var ssg = new Statix.Generator(
                contentFolder: Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../../../sample/content"),
                themeFolder: Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../../../sample/themes/statixdemo"),
                sourceUrl: "https://github.com/swharden/Statix/tree/main/sample/content",
                rootUrl: "http://localhost:8080/sample-site");

            ssg.Generate();
        }
    }
}
