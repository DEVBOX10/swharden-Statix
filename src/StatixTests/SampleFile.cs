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
        public static string REPO_ROOT
        {
            get
            {
                string localPath = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../../../"));
                if (Directory.Exists(localPath))
                    return localPath;
                else
                    Console.WriteLine($"repo root is not: {localPath}");

                string cloudRunnerRoot = "/home/runner/work/Statix/Statix";
                if (Directory.Exists(cloudRunnerRoot))
                    return cloudRunnerRoot;
                else
                    Console.WriteLine($"repo root is not: {cloudRunnerRoot}");

                throw new InvalidOperationException("cannot locate repo root");
            }
        }

        public static string FOLDER_PATH => Path.Combine(REPO_ROOT, "sample/content/");

        public static string IMAGE_PATH = Path.Combine(REPO_ROOT, "sample/content/images/small.jpg");
        public static string IMAGE_HASH = "43becb771f2eebcb72196bca905ad3ba";
    }
}
