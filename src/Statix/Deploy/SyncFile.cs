using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

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

        private static readonly string SyncVersion = "0.0.1";
        private static readonly string StatixVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public SyncFile(string localPath, string remotePath)
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

        public SyncFile(string remotePath, string hash, int size, string date)
        {
            RemotePath = remotePath;
            Hash = hash;
            Size = size;
            Uploaded = DateTime.Parse(date);
        }

        public override string ToString()
        {
            return HasBeenUploaded
                ? $"{RemotePath} ({Size / 1e3:N} kB) uploaded {UploadAge} ag"
                : $"{RemotePath} ({Size / 1e3:N} kB) NOT UPLOADED";
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

        public static string ToJson(SyncFile[] files, bool indented = true)
        {
            static string Iso8601(DateTime dt) => dt.ToString("o", CultureInfo.InvariantCulture);

            using var stream = new MemoryStream();
            var options = new JsonWriterOptions() { Indented = indented };
            using var writer = new Utf8JsonWriter(stream, options);

            writer.WriteStartObject();
            writer.WriteString("statixVersion", StatixVersion);
            writer.WriteString("syncVersion", SyncVersion);
            writer.WriteString("syncDate", Iso8601(DateTime.Now));
            writer.WriteStartArray("files");
            foreach (SyncFile file in files)
            {
                writer.WriteStartObject();
                writer.WriteString("path", file.RemotePath);
                writer.WriteString("hash", file.Hash);
                writer.WriteString("date", Iso8601(file.Uploaded));
                writer.WriteNumber("size", file.Size);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();

            writer.Flush(); // CRITICAL (flush buffer before getting bytes)

            byte[] bytes = stream.ToArray();
            string json = Encoding.UTF8.GetString(bytes);

            return json;
        }

        public static SyncFile[] FromJson(string json)
        {
            List<SyncFile> files = new List<SyncFile>();
            using JsonDocument document = JsonDocument.Parse(json);

            string version = document.RootElement.GetProperty("syncVersion").GetString();
            if (version != SyncVersion)
                throw new InvalidOperationException($"sync file version ({version}) does not match this version ({SyncVersion})");

            foreach (JsonElement el in document.RootElement.GetProperty("files").EnumerateArray())
            {
                string remotePath = el.GetProperty("path").GetString();
                string hash = el.GetProperty("hash").GetString();
                string date = el.GetProperty("date").GetString();
                int size = el.GetProperty("size").GetInt32();
                files.Add(new SyncFile(remotePath, hash, size, date));
            }

            return files.ToArray();
        }
    }
}
