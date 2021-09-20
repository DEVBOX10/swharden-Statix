using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace StatixTests
{
    class Plugins
    {
        [TestCase("  ![](magic.jpg)  ", true, "magic.jpg")]
        [TestCase("  ![](maGic.JPEG)  ", true, "maGic.JPEG")]
        [TestCase("  ![](magic.jpg)  ", true, "magic.jpg")]
        [TestCase("asdf ![](maGic.JPEG) asdf ", false, null)]
        [TestCase("![desc](magic.jpg)", false, null)]
        public void Test_Magic_Detection(string md, bool isMagic, string url)
        {
            Assert.AreEqual(isMagic, Statix.Plugin.IMarkdownPlugin.IsMagicLine(md));

            if (isMagic)
                Assert.AreEqual(url, Statix.Plugin.IMarkdownPlugin.MagicUrl(md));
        }
    }
}
