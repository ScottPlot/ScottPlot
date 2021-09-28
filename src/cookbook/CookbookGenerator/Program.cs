using System;
using System.Diagnostics;
using CommandLine;
using ScottPlot.Cookbook;
using System.IO;
using System.Text;
using System.Text.Json;

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
            if (Debugger.IsAttached)
            {
                string dllFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string outputFolder = Path.Combine(Path.GetDirectoryName(dllFilePath), "output");
                if (!Directory.Exists(outputFolder))
                    Directory.CreateDirectory(outputFolder);
                string cookbookFolder = Path.GetFullPath(Path.Combine(outputFolder, "../../../../../ScottPlot.Cookbook"));
                string outputFolderImages = Path.Combine(outputFolder, "images");
                string outputFolderSource = Path.Combine(outputFolder, "source");
                GenerateEverything(outputFolderImages, outputFolderSource, cookbookFolder);
            }
            else
            {
                var res = Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(RunOptions);
            }
        }

        static void RunOptions(CommandLineOptions opts)
        {
            GenerateEverything(opts.SaveImages, opts.SaveSource, opts.CookbookFolder);
        }

        private static void GenerateEverything(string outputImageFolder, string outputCodeFolder, string cookbookFolder)
        {
            Stopwatch sw = Stopwatch.StartNew();

            Console.WriteLine($"Generating images: {outputImageFolder}");
            Chef.CreateCookbookImages(outputImageFolder);

            Console.WriteLine($"Generating source: {outputCodeFolder}");
            Chef.CreateRecipesJson(cookbookFolder, Path.Combine(outputCodeFolder, "recipes.json"));

            Console.WriteLine($"Finished in {sw.Elapsed.TotalSeconds:F3} seconds");
        }
    }
}
