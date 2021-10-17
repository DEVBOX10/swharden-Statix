using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;

namespace Statix.Deploy
{
    public static class SyncJson
    {
        public static string ToJson(SyncFile file, bool indented = true)
        {
            SyncFile[] files = new SyncFile[] { file };
            return ToJson(files, indented);
        }

        public static string ToJson(SyncFile[] files, bool indented = true)
        {
            using var stream = new MemoryStream();
            var options = new JsonWriterOptions() { Indented = indented };
            using var writer = new Utf8JsonWriter(stream, options);

            writer.WriteStartObject();
            writer.WriteString("statixVersion", SyncFile.StatixVersion);
            writer.WriteString("syncVersion", SyncFile.SyncVersion);
            writer.WriteString("syncDate", SyncFile.ToIso8601(DateTime.Now));
            writer.WriteStartArray("files");
            foreach (SyncFile file in files)
            {
                writer.WriteStartObject();
                writer.WriteString("path", file.RemotePath);
                writer.WriteString("hash", file.Hash);
                writer.WriteString("date", SyncFile.ToIso8601(file.Uploaded));
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
            if (version != SyncFile.SyncVersion)
                throw new InvalidOperationException($"sync file version ({version}) does not match this version ({SyncFile.SyncVersion})");

            foreach (JsonElement el in document.RootElement.GetProperty("files").EnumerateArray())
            {
                string remotePath = el.GetProperty("path").GetString();
                string hash = el.GetProperty("hash").GetString();
                string date = el.GetProperty("date").GetString();
                int size = el.GetProperty("size").GetInt32();
                var remoteFile = SyncFile.FromRemoteFile(remotePath, hash, size, date);
                files.Add(remoteFile);
            }

            return files.ToArray();
        }
    }
}
