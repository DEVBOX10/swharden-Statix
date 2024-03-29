﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.IO;
using CommandLine;

namespace StatixBuilder
{
    class Program
    {
        private class CommandLineOptions
        {
            [Option(longName: "content", Required = true, HelpText = "path of the root website (containing markdown files)")]
            public string Content { get; set; }

            [Option(longName: "theme", Required = true, HelpText = "path of the theme folder (containing HTML templates)")]
            public string Theme { get; set; }

            [Option(longName: "urlSource", Required = true, HelpText = "URL of the content source code")]
            public string SourceUrl { get; set; }

            [Option(longName: "urlSite", Required = true, HelpText = "URL of the content on the web")]
            public string SiteUrl { get; set; }
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
                    SourceUrl = "https://github.com/swharden/Statix/tree/main/sample/content",
                    SiteUrl = "http://localhost:8080/sample-site",
                };
                RunOptions(opts);
                return;
            }

            Parser.Default.ParseArguments<CommandLineOptions>(args)
              .WithParsed(RunOptions)
              .WithNotParsed(HandleParseError);
        }

        static void RunOptions(CommandLineOptions opts)
        {
            var ssg = new Statix.Generator(
                contentFolder: opts.Content,
                themeFolder: opts.Theme,
                sourceUrl: opts.SourceUrl,
                rootUrl: opts.SiteUrl);

            ssg.Generate();
        }

        static void HandleParseError(IEnumerable<Error> errs)
        {
            throw new InvalidOperationException("command parsing error");
        }
    }
}
