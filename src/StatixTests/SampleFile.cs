using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatixTests
{
    internal static class SampleFile
    {
        public static string REPO_ROOT = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../../../"));

        public static string FOLDER_PATH = Path.Combine(REPO_ROOT, "sample/content/");

        public static string IMAGE_PATH = Path.Combine(REPO_ROOT, "sample/content/images/small.jpg");
        public static string IMAGE_HASH = "43becb771f2eebcb72196bca905ad3ba";
    }
}
