using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Statix.Deploy
{
    public class FtpConnection
    {
        public FtpConnection(string host, string username, string password)
        {
            using var client = new FtpClient(host, username, password);
            client.EncryptionMode = FtpEncryptionMode.Explicit;
            client.ValidateAnyCertificate = true;
            client.Connect();
        }
    }
}
