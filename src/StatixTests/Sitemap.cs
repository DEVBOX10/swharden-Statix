using System;
using NUnit.Framework;

namespace StatixTests
{
    public class Sitemap
    {
        private readonly Statix.Sitemap.SitemapBuilder SM = new();

        [SetUp]
        public void Setup()
        {
            SM.Add("https://swharden.com/1.html");
            SM.Add("https://swharden.com/2.html");
            SM.Add("https://swharden.com/3.html");

            var url = new Statix.Sitemap.Url()
            {
                Location = "https://swharden.com/custom",
                ChangeFreq = Statix.Sitemap.ChangeFreq.Daily,
                Modified = DateTime.Now,
                Priority = 42,
            };

            SM.Add(url);
        }

        [Test]
        public void Test_Output_Xml()
        {
            string xml = SM.GetXML();
            Console.WriteLine(xml);

            Assert.IsNotNull(xml);
            Assert.Greater(xml.Length, 50);
            Assert.That(xml.Contains("utf-"));
            Assert.That(xml.StartsWith("<?xml "));
        }

        [Test]
        public void Test_Output_Text()
        {
            string txt = SM.GetText();
            Console.WriteLine(txt);

            Assert.IsNotNull(txt);
            Assert.Greater(txt.Length, 50);
        }

        [Test]
        public void Test_Scan_Filesystem()
        {
            var sm = new Statix.Sitemap.SitemapBuilder();
            sm.AddScan(SampleFile.FOLDER_PATH, "https://swharden.com/sample/content/");
            Console.WriteLine(sm.GetText());
        }
    }
}