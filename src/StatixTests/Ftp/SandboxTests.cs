using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;
using System.IO;

namespace StatixTests.Ftp
{
    [TestFixture]
    [Ignore("Skip tests requiring FTP connection")]
    internal class SandboxTests
    {
        public static string REPO_ROOT = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "../../../../../"));
        public static string SAMPLE_FOLDER_PATH = Path.Combine(REPO_ROOT, "sample/content/");

        public static string SAMPLE_IMAGE_PATH = Path.Combine(REPO_ROOT, "sample/content/images/small.jpg");
        public static string SAMPLE_IMAGE_HASH = "43becb771f2eebcb72196bca905ad3ba";

        static string FTP_USER => Environment.GetEnvironmentVariable("SANDBOX_FTP_USERNAME") ?? throw new NullReferenceException();
        static string FTP_PASS => Environment.GetEnvironmentVariable("SANDBOX_FTP_PASSWORD") ?? throw new NullReferenceException();

        [OneTimeSetUp]
        public static void LoadEnvVars()
        {
            var config = new ConfigurationBuilder().AddUserSecrets<SandboxTests>().Build();
            foreach (var child in config.GetChildren())
            {
                Environment.SetEnvironmentVariable(child.Key, child.Value);
            }

            if (Environment.GetEnvironmentVariable("SANDBOX_FTP_USERNAME") is null)
                throw new InvalidOperationException("environment variables do not contain username");

            if (Environment.GetEnvironmentVariable("SANDBOX_FTP_PASSWORD") is null)
                throw new InvalidOperationException("environment variables do not contain password");

        }

        [Test]
        public static void Test_Connection_Success()
        {
            using var f = new Statix.Deploy.FtpConnection("swharden.com", FTP_USER, FTP_PASS);
            Console.WriteLine(f);
        }

        [Test]
        public static void Test_Upload()
        {

            var file1 = Statix.Deploy.SyncFile.FromLocalFile(SAMPLE_IMAGE_PATH, "/sample.jpg");
            Statix.Deploy.UploadAction action1 = new Statix.Deploy.UploadAction(file1, Statix.Deploy.SyncAction.Replace);

            var file2 = Statix.Deploy.SyncFile.FromLocalFile(SAMPLE_IMAGE_PATH, "/nested/folder/sample.jpg");
            Statix.Deploy.UploadAction action2 = new Statix.Deploy.UploadAction(file2, Statix.Deploy.SyncAction.Replace);

            Statix.Deploy.UploadAction[] actions = new Statix.Deploy.UploadAction[] { action1, action2 };

            using var ftp = new Statix.Deploy.FtpConnection("swharden.com", FTP_USER, FTP_PASS);
            ftp.Upload(actions);
        }
    }
}
