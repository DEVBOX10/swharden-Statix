using NUnit.Framework;
using System;
using System.IO;

namespace StatixTests.Ftp
{
    internal class SyncState
    {
        public static string REPO_ROOT = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../../../"));
        public static string SAMPLE_IMAGE_PATH = Path.Combine(REPO_ROOT, "sample/content/images/small.jpg");
        public static string SAMPLE_FOLDER_PATH = Path.Combine(REPO_ROOT, "sample/content/");
        public static int SAMPLE_IMAGE_SIZE = 28_818;
        public static string SAMPLE_IMAGE_HASH = "43becb771f2eebcb72196bca905ad3ba";

        [Test]
        public void Test_SyncState_Add()
        {
            var sync = Statix.Deploy.SyncFile.FromLocalFile(SAMPLE_IMAGE_PATH, "/sample/remote/path.jpg");
            string json = sync.ToJson();
            Console.WriteLine(json);

            Assert.IsNotNull(json);
            Assert.IsNotEmpty(json);
        }

        [Test]
        public void Test_SyncState_AddFile()
        {
            var sampleFile = Statix.Deploy.SyncFile.FromLocalFile(SAMPLE_IMAGE_PATH, "/sample/remote/path.jpg");
            Assert.AreEqual(SAMPLE_IMAGE_SIZE, sampleFile.Size);
            Assert.AreEqual(SAMPLE_IMAGE_HASH, sampleFile.Hash);

            string json = sampleFile.ToJson();
            Console.WriteLine(json);

            Assert.IsNotNull(json);
            Assert.IsNotEmpty(json);
        }

        [Test]
        public void Test_SyncState_AddFolder()
        {
            Statix.Deploy.SyncFile[] files = Statix.Deploy.SyncFile.FromLocalFolder(SAMPLE_FOLDER_PATH, "/remote/folder/");
            string json = Statix.Deploy.SyncJson.ToJson(files);
            Console.WriteLine(json);

            Assert.IsNotNull(json);
            Assert.IsNotEmpty(json);
        }

        [Test]
        public void Test_SyncState_Load()
        {
            Statix.Deploy.SyncFile[] sync1Files = Statix.Deploy.SyncFile.FromLocalFolder(SAMPLE_FOLDER_PATH, "/remote/folder/");
            string sync1Json = Statix.Deploy.SyncJson.ToJson(sync1Files);

            string localJsonFilePath = Path.GetFullPath("Test_SyncState_Load.json");
            File.WriteAllText(localJsonFilePath, sync1Json);
            Console.WriteLine(localJsonFilePath);

            string json = File.ReadAllText(localJsonFilePath);
            Statix.Deploy.SyncFile[] sync2Files = Statix.Deploy.SyncJson.FromJson(json);
            foreach (var file in sync2Files)
                Console.WriteLine(file);

            Assert.IsNotNull(sync2Files);
            Assert.IsNotEmpty(sync2Files);
            Assert.AreEqual(sync1Files.Length, sync2Files.Length);
        }
    }
}
