using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using System;

namespace StatixTests.Ftp
{
    internal class Login
    {
        [OneTimeSetUp]
        public void LoadEnvVars()
        {
            var config = new ConfigurationBuilder().AddUserSecrets<Login>().Build();
            int loaded = 0;
            foreach (var child in config.GetChildren())
            {
                Environment.SetEnvironmentVariable(child.Key, child.Value);
                Console.WriteLine($"Loaded environment varaible: {child.Key}");
                loaded += 1;
            }
            Console.WriteLine($"Loaded {loaded} user secrets into environment variables.");
        }

        [Test]
        public void Test_Login_Success()
        {
            string username = Environment.GetEnvironmentVariable("SANDBOX_FTP_USERNAME");
            string password = Environment.GetEnvironmentVariable("SANDBOX_FTP_PASSWORD");

            if (username is null)
                throw new InvalidOperationException("environment variables do not contain username");

            if (password is null)
                throw new InvalidOperationException("environment variables do not contain password");

            Console.WriteLine("obtained user/pass from environment varaiables");
        }
    }
}
