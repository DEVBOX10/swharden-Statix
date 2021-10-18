using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Statix.Deploy
{
    public class TrackedFile
    {
        public string LocalPath { get; private set; }
        public string RemotePath { get; private set; }
        public string LocalHash { get; private set; }
        public string RemoteHash { get; private set; }
        public DateTime RemoteDate { get; private set; }
        public bool ExistsLocally => LocalHash != null;
        public bool ExistsRemotely => RemoteHash != null;

        private TrackedFile(string remoteFilePath)
        {
            RemotePath = remoteFilePath.Replace("\\", "/");
        }

        public static TrackedFile FromLocal(string localFilePath, string remoteFilePath)
        {
            return new TrackedFile(remoteFilePath)
            {
                LocalPath = Path.GetFullPath(localFilePath),
                LocalHash = GetMD5(localFilePath),
            };
        }

        public static TrackedFile FromRemote(string remoteFilePath, string remoteHash, DateTime remoteDate)
        {
            return new TrackedFile(remoteFilePath)
            {
                RemoteHash = remoteHash,
                RemoteDate = remoteDate,
            };
        }

        /// <summary>
        /// Call when a LOCAL file was added but now a REMOTE one is seen
        /// </summary>
        public void UpdateRemoteInfo(string remoteHash, DateTime remoteDate)
        {
            RemoteHash = remoteHash;
            RemoteDate = remoteDate;
        }

        /// <summary>
        /// Call when a REMOTE file was added but now a LOCAL one is seen
        /// </summary>
        public void UpdateLocalInfo(string localFilePath)
        {
            LocalPath = Path.GetFullPath(localFilePath);
            LocalHash = GetMD5(localFilePath);
        }

        public void HasBeenUploaded()
        {
            RemoteHash = LocalHash;
            RemoteDate = DateTime.UtcNow;
        }

        public void HasBeenDeleted()
        {
            LocalPath = null;
            RemotePath = null;
            LocalHash = null;
            RemoteHash = null;
        }

        public override string ToString()
        {
            return $"{LocalPath} -> {RemotePath}";
        }

        private static string GetMD5(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            var md5 = System.Security.Cryptography.MD5.Create();
            using var stream = File.OpenRead(filePath);
            string[] nibbles = md5.ComputeHash(stream).Select(x => x.ToString("x2")).ToArray();
            return string.Join("", nibbles);
        }
    }
}