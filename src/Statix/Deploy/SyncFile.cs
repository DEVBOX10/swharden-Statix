using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Statix.Deploy
{
    public class SyncFile
    {
        public readonly string LocalPath;
        public readonly string RemotePath;
        public readonly int Size;
        public readonly string Hash;
        public readonly DateTime Uploaded = new DateTime();
        public bool HasBeenUploaded => Uploaded.Year > 1;
        public TimeSpan UploadAge => DateTime.UtcNow - Uploaded;

        public static readonly string SyncVersion = "0.0.1";
        public static readonly string StatixVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        private SyncFile(string localPath, string remotePath)
        {
            if (!File.Exists(localPath))
                throw new FileNotFoundException(localPath);

            if (remotePath.Contains("\\"))
                throw new ArgumentException("remote path may not contain backslashes");

            localPath = Path.GetFullPath(localPath).Replace("\\", "/");

            if (localPath.EndsWith("/"))
                throw new ArgumentException($"{nameof(localPath)} must be a file (no trailing slash)");

            if (remotePath.EndsWith("/"))
                throw new ArgumentException($"{nameof(remotePath)} must be a file (no trailing slash)");

            LocalPath = localPath;
            RemotePath = remotePath;
            Size = (int)GetSize(localPath);
            Hash = GetMD5(localPath);
        }

        private SyncFile(string remotePath, string hash, int size, string date)
        {
            if (remotePath.EndsWith("/"))
                throw new ArgumentException($"{nameof(remotePath)} must be a file (no trailing slash)");

            RemotePath = remotePath;
            Hash = hash;
            Size = size;
            Uploaded = DateTime.Parse(date);
        }

        public static string ToIso8601(DateTime dt) => dt.ToString("o", System.Globalization.CultureInfo.InvariantCulture);

        public static SyncFile FromRemoteFile(string remotePath, string hash, int size, string date)
        {
            return new SyncFile(remotePath, hash, size, date);
        }

        public static SyncFile FromLocalFile(string localPath, string remotePath)
        {
            return new SyncFile(localPath, remotePath);
        }

        public static SyncFile[] FromLocalFolder(string localFolder, string remoteFolder)
        {
            List<SyncFile> syncFiles = new List<SyncFile>();

            localFolder = Path.GetFullPath(localFolder);
            var localFiles = Directory.GetFiles(localFolder, "*", SearchOption.AllDirectories);
            foreach (string localFile in localFiles)
            {
                string remoteFile = localFile.Replace(localFolder, remoteFolder).Replace("\\", "/");
                var sf = new SyncFile(localFile, remoteFile);
                syncFiles.Add(sf);
            }

            return syncFiles.ToArray();
        }

        public override string ToString()
        {
            string age = UploadAge.ToString().Split(".")[0] + " ago";
            if (UploadAge.Seconds < 1)
                age = "just now";

            return HasBeenUploaded
                ? $"{RemotePath} ({Size / 1e3:N} kB) uploaded {age}"
                : $"{RemotePath} ({Size / 1e3:N} kB) NOT UPLOADED";
        }

        public string ToJson(bool indented = true)
        {
            return SyncJson.ToJson(this, indented);
        }

        private static long GetSize(string filePath)
        {
            return new FileInfo(filePath).Length;
        }

        private static string GetMD5(string filePath)
        {
            var md5 = System.Security.Cryptography.MD5.Create();
            using var stream = File.OpenRead(filePath);
            string[] nibbles = md5.ComputeHash(stream).Select(x => x.ToString("x2")).ToArray();
            return string.Join("", nibbles);
        }
    }
}
