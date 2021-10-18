using FluentFTP;
using System;
using System.IO;

namespace Statix.Deploy
{
    public class FtpUploader : UploaderBase
    {
        readonly FtpClient Client;

        public FtpUploader(string host, string username, string password)
        {
            Console.WriteLine("FTP: Connecting...");
            Client = new FtpClient(host, username, password)
            {
                EncryptionMode = FtpEncryptionMode.Explicit,
                ValidateAnyCertificate = true
            };
            Client.Connect();
            Console.WriteLine("FTP: Connected");
        }

        public override void Dispose()
        {
            Client.Disconnect();
            Console.WriteLine("FTP: Disconnected");
            Client.Dispose();
        }

        private void Upload(string localFilePath, string remotePath)
        {
            byte[] bytes = File.ReadAllBytes(localFilePath);
            Client.Upload(bytes, remotePath, existsMode: FtpRemoteExists.Overwrite, createRemoteDir: true);
        }

        private void Delete(string remotePath)
        {
            Client.DeleteFile(remotePath);
        }

        protected override void RemoteCreate(TrackedFile file)
        {
            Console.WriteLine($"Creating: {file.RemotePath}");
            Upload(file.LocalPath, file.RemotePath);
            file.HasBeenUploaded();
        }

        protected override void RemoteReplace(TrackedFile file)
        {
            Console.WriteLine($"Replacing: {file.RemotePath}");
            Upload(file.LocalPath, file.RemotePath);
            file.HasBeenUploaded();
        }

        protected override void RemoteDelete(TrackedFile file)
        {
            Console.WriteLine($"Deleting: {file.RemotePath}");
            Delete(file.RemotePath);
            file.HasBeenDeleted();
        }

        protected override void RemoteSkip(TrackedFile file)
        {
            Console.WriteLine($"Unchanged: {file.RemotePath}");
        }
    }
}
