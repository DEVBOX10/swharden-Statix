using NUnit.Framework;
using System;
using System.IO;

namespace StatixTests.Ftp
{
    internal class UploadManagerTests
    {
        public static string REPO_ROOT = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../../../"));
        public static string SAMPLE_FOLDER_PATH = Path.Combine(REPO_ROOT, "sample/content/");

        public static string SAMPLE_IMAGE_PATH = Path.Combine(REPO_ROOT, "sample/content/images/small.jpg");
        public static string SAMPLE_IMAGE_HASH = "43becb771f2eebcb72196bca905ad3ba";

        [Test]
        public void Test_UploadManager_Folder()
        {
            var um = new Statix.Deploy.UploadPlan();
            um.AddFolder(SAMPLE_FOLDER_PATH, "/remote/path/");

            foreach (var uploadAction in um.GetActions())
                Console.WriteLine(uploadAction);
        }

        [Test]
        public void Test_UploadManager_File_CreateIfUntracked()
        {
            var um = new Statix.Deploy.UploadPlan();

            var action = um.AddFile(SAMPLE_IMAGE_PATH, "/remote/path/sample.jpg");
            Assert.AreEqual(Statix.Deploy.SyncAction.Create, action);

            foreach (var uploadAction in um.GetActions())
                Console.WriteLine(uploadAction);
        }

        [Test]
        public void Test_UploadManager_File_SkipIfHashUnchanged()
        {
            var um = new Statix.Deploy.UploadPlan();
            um.AddKnownFile("/remote/path/sample.jpg", SAMPLE_IMAGE_HASH, -1, DateTime.UtcNow.ToString());

            var action = um.AddFile(SAMPLE_IMAGE_PATH, "/remote/path/sample.jpg");
            Assert.AreEqual(Statix.Deploy.SyncAction.Skip, action);

            foreach (var uploadAction in um.GetActions())
                Console.WriteLine(uploadAction);
        }

        [Test]
        public void Test_UploadManager_File_ReplaceIfHashChanged()
        {
            var um = new Statix.Deploy.UploadPlan();
            um.AddKnownFile("/remote/path/sample.jpg", "differentHash123", -1, DateTime.UtcNow.ToString());

            var action = um.AddFile(SAMPLE_IMAGE_PATH, "/remote/path/sample.jpg");
            Assert.AreEqual(Statix.Deploy.SyncAction.Replace, action);

            foreach (var uploadAction in um.GetActions())
                Console.WriteLine(uploadAction);
        }
    }
}
