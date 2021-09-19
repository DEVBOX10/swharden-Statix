using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using CommandLine;

namespace Statix
{
    class Program
    {
        private class CommandLineOptions
        {
            [Option(longName: "content", Required = true, HelpText = "path of the root website (containing markdown files)")]
            public string Content { get; set; }
            public DirectoryInfo ContentDirectory => new DirectoryInfo(Content);

            [Option(longName: "theme", Required = true, HelpText = "path of the theme folder (containing HTML templates)")]
            public string Theme { get; set; }
            public DirectoryInfo ThemeDirectory => new DirectoryInfo(Theme);
        }

        static void Main(string[] args)
        {
            if (Debugger.IsAttached)
            {
                string exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string exeFolderPath = Path.GetDirectoryName(exePath);
                CommandLineOptions opts = new CommandLineOptions()
                {
                    Content = Path.GetFullPath(exeFolderPath + "/../../../../../sample/content"),
                    Theme = Path.GetFullPath(exeFolderPath + "/../../../../../sample/themes/statixdemo"),
                };
                RunOptions(opts);
                return;
            }

            Parser.Default.ParseArguments<CommandLineOptions>(args)
              .WithParsed(RunOptions);
        }

        static void RunOptions(CommandLineOptions opts)
        {
            Generate.SingleArticlePages(opts.ContentDirectory, opts.ThemeDirectory);
        }
    }
}
