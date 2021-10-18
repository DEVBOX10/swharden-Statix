using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Statix.Deploy;

namespace StatixTests.DeployTests
{
    internal class FtpUploaderTests
    {
        private static (string user, string pass) GetLoginCreds()
        {
            // load secrets into environment variables
            var config = new ConfigurationBuilder().AddUserSecrets<FtpUploaderTests>().Build();
            foreach (var child in config.GetChildren())
                Environment.SetEnvironmentVariable(child.Key, child.Value);

            // read secrets out of environment variables
            string? user = Environment.GetEnvironmentVariable("SANDBOX_FTP_USERNAME");
            string? pass = Environment.GetEnvironmentVariable("SANDBOX_FTP_PASSWORD");

            // throw if secrets not loaded
            if (user is null)
                throw new InvalidOperationException("environment variables do not contain username");
            if (pass is null)
                throw new InvalidOperationException("environment variables do not contain password");

            return (user, pass);
        }

        [Test]
        public void Test_Uploader()
        {
            // https://swharden.com/dev/sandbox/sample/folder/image.jpg

            Plan plan = new();
            plan.AddFile(SampleFile.IMAGE_PATH, "/sample/folder/image.jpg");

            (string user, string pass) = GetLoginCreds();
            using var uploader = new FtpUploader("swharden.com", user, pass);

            uploader.RemoteDeploy(plan);
        }
    }
}
