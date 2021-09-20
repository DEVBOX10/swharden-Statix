using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace StatixTests
{
    class Plugins
    {
        [Test]
        public void Test_Magic_Markdown()
        {
            string md = "  ![](magic.jpg)  ";
            Assert.True(Statix.Plugin.IMarkdownPlugin.IsMagicLine(md));
            Assert.AreEqual("magic.jpg", Statix.Plugin.IMarkdownPlugin.MagicUrl(md));
        }

        [Test]
        public void Test_Clickable_Images()
        {
            string[] linesIn = new string[] { "![](magic.jpg)" };
            string[] linesOut = new Statix.Plugin.ClickableImages().Apply(linesIn);
            Assert.That(linesOut[0].Contains("<a href='magic.jpg'"));
            Assert.That(linesOut[0].Contains("<img src='magic.jpg'"));
        }
    }
}
