using FluentFTP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Statix.Deploy
{
    public class FtpConnection : IDisposable
    {
        readonly FtpClient Client;

        public FtpConnection(string host, string username, string password)
        {
            Client = new FtpClient(host, username, password)
            {
                EncryptionMode = FtpEncryptionMode.Explicit,
                ValidateAnyCertificate = true
            };

            Client.Connect();
            Console.WriteLine("Connected");
        }

        public void Dispose()
        {
            Client.Disconnect();
            Console.WriteLine("Disconnected");
            Client.Dispose();
        }

        public void Upload(UploadAction[] actions)
        {
            Console.WriteLine($"Executing {actions.Length} actions...");
            foreach (var action in actions)
            {
                if (action.SyncAction == SyncAction.Skip)
                    continue;

                Console.WriteLine($"Uploading: {action.LocalFile.RemotePath}");
                byte[] bytes = File.ReadAllBytes(action.LocalFile.LocalPath);
                Client.Upload(bytes, action.LocalFile.RemotePath, existsMode: FtpRemoteExists.Overwrite, createRemoteDir: true);
            }
        }
    }
}
