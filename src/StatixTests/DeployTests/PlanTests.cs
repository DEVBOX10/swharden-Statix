using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Statix.Deploy;

namespace StatixTests.DeployTests
{
    internal class PlanTests
    {
        [Test]
        public void Test_Plan_AddFile()
        {
            Plan plan = new();
            plan.AddFile(SampleFile.IMAGE_PATH, "/sample/file/file1.jpg");
            Assert.AreEqual(1, plan.FileCount);
        }

        [Test]
        public void Test_Plan_AddFolder()
        {
            Plan plan = new();
            plan.AddFolder(SampleFile.FOLDER_PATH, "/sample/folder/");
            Assert.Greater(plan.FileCount, 1);
        }

        [Test]
        public void Test_Plan_ToJson()
        {
            Plan plan = new();
            plan.AddFolder(SampleFile.FOLDER_PATH, "/sample/folder/");

            var uploader = new MockUploader();
            uploader.RemoteDeploy(plan);

            string json = plan.ToJson();
            Console.WriteLine(json);
            Assert.IsNotNull(json);
            Assert.IsNotEmpty(json);
        }

        [Test]
        public void Test_Plan_FromJson()
        {
            var uploader = new MockUploader();

            Plan plan1 = new();
            plan1.AddFolder(SampleFile.FOLDER_PATH, "/sample/folder/");
            uploader.RemoteDeploy(plan1);
            string json = plan1.ToJson();

            Plan plan2 = new(json);
            plan2.AddFolder(SampleFile.FOLDER_PATH, "/sample/folder/");
            uploader.RemoteDeploy(plan2);
        }
    }
}
