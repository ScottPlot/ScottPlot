using System;
using System.Diagnostics;
using CommandLine;

namespace CookbookGenerator
{
    internal class Program
    {
        public class CommandLineOptions
        {
            [Option(longName: "cookbookFolder", Required = true, HelpText = "folder containing cookbook csproj file")]
            public string CookbookFolder { get; set; }

            [Option(longName: "saveImages", Required = true, HelpText = "folder to save images into")]
            public string SaveImages { get; set; }

            [Option(longName: "saveSource", Required = true, HelpText = "folder to save source code into")]
            public string SaveSource { get; set; }
        }

        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(RunOptions);
        }

        static void RunOptions(CommandLineOptions opts)
        {
            Stopwatch sw = Stopwatch.StartNew();
            var chef = new ScottPlot.Cookbook.Chef();
            chef.CreateCookbookImages(opts.SaveImages);
            chef.CreateCookbookSource(opts.CookbookFolder, opts.SaveSource);
            Console.WriteLine($"Finished in {sw.Elapsed.TotalSeconds:F3} seconds");
        }
    }
}
