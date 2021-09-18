using System;
using NUnit.Framework;

namespace StatixTests
{
    public class Tests
    {
        public static readonly string CONTENT_FOLDER = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../../../sample/content/");

        [Test]
        public void Test_MdConversion_Text()
        {
            string mdInput = "this *is* a test";
            string htmlOutput = Statix.Convert.Text(mdInput);

            Assert.AreEqual("<p>this <em>is</em> a test</p>\n", htmlOutput);
        }

        [Test]
        public void Test_MdConversion_Files()
        {
            var paths = Statix.Convert.FindMdFiles(CONTENT_FOLDER);
            Assert.IsNotNull(paths);
            Assert.IsNotEmpty(paths);

            foreach (var path in paths)
            {
                Console.WriteLine(path);
                Statix.Convert.File(path);
            }
        }
    }
}