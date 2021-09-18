using NUnit.Framework;

namespace StatixTests
{
    public class Tests
    {
        [Test]
        public void Test_MdConversion_Works()
        {
            string mdInput = "this *is* a test";
            string htmlOutput = Statix.Convert.ToHtml(mdInput);

            Assert.AreEqual("<p>this <em>is</em> a test</p>\n", htmlOutput);
        }
    }
}