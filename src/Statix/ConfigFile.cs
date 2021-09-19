using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.IO;

namespace Statix
{
    public class ConfigFile
    {
        public string Theme { get; set; }
        public DirectoryInfo ThemeDirectory => new DirectoryInfo(Path.GetFullPath(Theme));
        public string Content { get; set; }
        public DirectoryInfo ContentDirectory => new DirectoryInfo(Path.GetFullPath(Content));

        public ConfigFile()
        {

        }

        public ConfigFile(string path)
        {
            path = Path.GetFullPath(path);
            string configFolder = Path.GetDirectoryName(path);

            if (!File.Exists(path))
                throw new ArgumentException($"config file does not exist: {path}");

            string json = File.ReadAllText(path);
            using JsonDocument document = JsonDocument.Parse(json);

            Theme = document.RootElement.GetProperty("theme").GetString();
            Theme = Path.Combine(configFolder, Theme);
            Theme = Path.GetFullPath(Theme);

            Content = document.RootElement.GetProperty("content").GetString();
            Content = Path.Combine(configFolder, Content);
            Content = Path.GetFullPath(Content);
        }

        public void Save(string path) => throw new NotImplementedException();
    }
}
