using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Linq;

namespace Statix.Deploy
{
    public class SyncManager
    {
        private readonly List<SyncFile> Files = new List<SyncFile>();
        public int FileCount => Files.Count;

        public SyncManager()
        {

        }

        public SyncFile AddFile(string localFile, string remoteFile)
        {
            SyncFile file = new SyncFile(localFile, remoteFile);
            Files.Add(file);
            return file;
        }

        public SyncFile[] AddFolder(string localFolder, string remoteFolder)
        {
            if (!Directory.Exists(localFolder))
                throw new DirectoryNotFoundException(localFolder);

            if (!remoteFolder.EndsWith("/"))
                throw new ArgumentException($"{nameof(remoteFolder)} must be a folder name with a trailing slash");

            List<SyncFile> syncFiles = new List<SyncFile>();

            localFolder = Path.GetFullPath(localFolder);
            var localFiles = Directory.GetFiles(localFolder, "*", SearchOption.AllDirectories);
            foreach (string localFile in localFiles)
            {
                string remoteFile = localFile.Replace(localFolder, remoteFolder).Replace("\\", "/");
                SyncFile file = AddFile(localFile, remoteFile);
                syncFiles.Add(file);
            }

            return syncFiles.ToArray();
        }

        public string ToJson(bool indented = true)
        {
            return SyncFile.ToJson(Files.ToArray(), indented);
        }
    }
}
