using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Statix.Deploy
{
    public class Plan
    {
        readonly List<TrackedFile> Files = new List<TrackedFile>();
        public int FileCount => Files.Count;
        public static readonly string FileVersion = "0.0.2";
        public static readonly string StatixVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

        public Plan()
        {

        }

        public Plan(string json)
        {
            AddKnown(json);
        }

        public TrackedFile[] GetTrackedFiles() => Files.ToArray();

        public void AddFile(string localFilePath, string remoteFilePath)
        {
            TrackedFile[] matches = Files.Where(x => x.RemotePath == remoteFilePath).ToArray();

            if (matches.Length == 0)
            {
                Files.Add(TrackedFile.FromLocal(localFilePath, remoteFilePath));
            }
            else
            {
                foreach (var match in matches)
                    match.UpdateLocalInfo(localFilePath);
            }
        }

        public void AddKnown(string remoteFilePath, string remoteHash, DateTime remoteDate)
        {
            TrackedFile[] matches = Files.Where(x => x.RemotePath == remoteFilePath).ToArray();

            if (matches.Length == 0)
            {
                Files.Add(TrackedFile.FromRemote(remoteFilePath, remoteHash, remoteDate));
                return;
            }

            foreach (TrackedFile match in matches)
            {
                match.UpdateRemoteInfo(remoteHash, remoteDate);
            }
        }

        public void AddFolder(string localFolderPath, string remoteFolderPath)
        {
            localFolderPath = Path.GetFullPath(localFolderPath);
            string[] localPaths = Directory.GetFiles(localFolderPath, "*", SearchOption.AllDirectories);
            foreach (string localFile in localPaths)
            {
                string relativeFilePath = localFile.Replace(localFolderPath, "");
                string remoteFilePath = Path.Combine(remoteFolderPath, relativeFilePath).Replace("\\", "/");
                AddFile(localFile, remoteFilePath);
            }
        }

        private static string ToIso8601(DateTime dt) => dt.ToString("o", System.Globalization.CultureInfo.InvariantCulture);

        public string ToJson(bool indented = true)
        {
            using var stream = new MemoryStream();
            var options = new JsonWriterOptions() { Indented = indented };
            using var writer = new Utf8JsonWriter(stream, options);

            writer.WriteStartObject();
            writer.WriteString("statixVersion", StatixVersion);
            writer.WriteString("fileVersion", FileVersion);
            writer.WriteString("syncDate", ToIso8601(DateTime.Now));
            writer.WriteStartArray("files");
            foreach (TrackedFile file in Files)
            {
                writer.WriteStartObject();
                writer.WriteString("path", file.RemotePath);
                writer.WriteString("hash", file.RemoteHash);
                writer.WriteString("date", file.RemoteDate);
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();

            writer.Flush(); // CRITICAL (flush buffer before getting bytes)

            byte[] bytes = stream.ToArray();
            string json = Encoding.UTF8.GetString(bytes);

            return json;
        }

        public void AddKnown(string json)
        {
            using JsonDocument document = JsonDocument.Parse(json);

            string version = document.RootElement.GetProperty("fileVersion").GetString();
            if (version != FileVersion)
                throw new InvalidOperationException($"sync file version ({version}) does not match this version ({FileVersion})");

            foreach (JsonElement el in document.RootElement.GetProperty("files").EnumerateArray())
            {
                string remotePath = el.GetProperty("path").GetString();
                string hash = el.GetProperty("hash").GetString();
                string date = el.GetProperty("date").GetString();
                DateTime dt = DateTime.Parse(date);
                AddKnown(remotePath, hash, dt);
            }
        }
    }
}
