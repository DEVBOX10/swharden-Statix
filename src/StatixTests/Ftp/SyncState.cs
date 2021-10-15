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
        public void Test_SyncState_ToJson()
        {
            var sync = new Statix.Deploy.SyncManager();

            string json = sync.ToJson();
            Console.WriteLine(json);

            Assert.IsNotNull(json);
            Assert.IsNotEmpty(json);
        }

        [Test]
        public void Test_SyncState_Add()
        {
            var sync = new Statix.Deploy.SyncManager();
            sync.AddFile(SAMPLE_IMAGE_PATH, "/sample/remote/path.jpg");

            string json = sync.ToJson();
            Console.WriteLine(json);

            Assert.IsNotNull(json);
            Assert.IsNotEmpty(json);
        }

        [Test]
        public void Test_SyncState_AddFile()
        {
            var sync = new Statix.Deploy.SyncManager();
            var sampleFile = sync.AddFile(SAMPLE_IMAGE_PATH, "/some/remote/path.jpg");
            Assert.AreEqual(SAMPLE_IMAGE_SIZE, sampleFile.Size);
            Assert.AreEqual(SAMPLE_IMAGE_HASH, sampleFile.Hash);

            string json = sync.ToJson();
            Console.WriteLine(json);

            Assert.IsNotNull(json);
            Assert.IsNotEmpty(json);
        }

        [Test]
        public void Test_SyncState_AddFolder()
        {
            var sync = new Statix.Deploy.SyncManager();
            sync.AddFolder(SAMPLE_FOLDER_PATH, "/remote/folder/");

            string json = sync.ToJson();
            Console.WriteLine(json);

            Assert.IsNotNull(json);
            Assert.IsNotEmpty(json);
        }

        [Test]
        public void Test_SyncState_Load()
        {
            var sync1 = new Statix.Deploy.SyncManager();
            sync1.AddFolder(SAMPLE_FOLDER_PATH, "/remote/folder/");

            string localJsonFilePath = Path.GetFullPath("Test_SyncState_Load.json");
            File.WriteAllText(localJsonFilePath, sync1.ToJson());
            Console.WriteLine(localJsonFilePath);

            string json = File.ReadAllText(localJsonFilePath);
            Statix.Deploy.SyncFile[] files = Statix.Deploy.SyncFile.FromJson(json);
            foreach (var file in files)
                Console.WriteLine(file);

            Assert.IsNotNull(files);
            Assert.IsNotEmpty(files);
            Assert.AreEqual(sync1.FileCount, files.Length);        }
    }
}
