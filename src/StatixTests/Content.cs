using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace StatixTests
{
    class Content
    {
        [Test]
        public void Test_Content_Parsing()
        {
            var c = new Statix.Content(SampleText.SampleMarkdown1);
            Assert.IsNotNull(c.Markdown);
            Assert.IsNotNull(c.HTML);
            Assert.That(c.Markdown.Contains("not in the header"));
            Assert.That(c.HTML.Contains("not in the header"));
        }

        [Test]
        public void Test_Markdown_Conversion()
        {
            var c = new Statix.Content("this *is* a test");
            Assert.AreEqual("<p>this <em>is</em> a test</p>\n", c.HTML);
        }
    }
}
