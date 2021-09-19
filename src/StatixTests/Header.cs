using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace StatixTests
{
    class Header
    {
        [Test]
        public void Test_Header_Parsing()
        {
            var h = new Statix.Header(SampleText.SampleMarkdown1);
            Assert.AreEqual("sample title", h.Title);
            Assert.AreEqual("sample description", h.Description);
            Assert.AreEqual("1985-09-24 01:23:45", h.Date);
            Assert.AreEqual(new string[] { "tag1", "tag2", "tag3" }, h.Tags);
        }
    }
}
