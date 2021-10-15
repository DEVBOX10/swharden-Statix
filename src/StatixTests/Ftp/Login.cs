using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;

namespace StatixTests.Ftp
{
    [TestFixture]
    [Ignore("Skip tests requiring FTP connection")]
    internal class Login
    {
        [OneTimeSetUp]
        public static void LoadEnvVars()
        {
            var config = new ConfigurationBuilder().AddUserSecrets<Login>().Build();
            foreach (var child in config.GetChildren())
            {
                Environment.SetEnvironmentVariable(child.Key, child.Value);
            }
        }

        [Test]
        public static void Test_Connection_Success()
        {
            string username = Environment.GetEnvironmentVariable("SANDBOX_FTP_USERNAME") ?? throw new NullReferenceException();
            string password = Environment.GetEnvironmentVariable("SANDBOX_FTP_PASSWORD") ?? throw new NullReferenceException();

            if (username is null)
                throw new InvalidOperationException("environment variables do not contain username");

            if (password is null)
                throw new InvalidOperationException("environment variables do not contain password");

            var f = new Statix.Deploy.FtpConnection("swharden.com", username, password);
            Console.WriteLine(f);
        }
    }
}
