using System;
using System.Diagnostics;
using CommandLine;
using ScottPlot.Cookbook;
using System.IO;

namespace CookbookGenerator
{
    internal class Program
    {
        public class CommandLineOptions
        {
            [Option(longName: "cookbook", Required = true, HelpText = "folder containing cookbook csproj file")]
            public string CookbookFolder { get; set; }

            [Option(longName: "imageFolder", Required = true, HelpText = "folder to save images into")]
            public string OutImageFolder { get; set; }

            [Option(longName: "sourceFile", Required = true, HelpText = "file path to save recipes in JSON format")]
            public string OutJsonFile { get; set; }
        }

        static void Main(string[] args)
        {
            if (Debugger.IsAttached)
            {
                string dllFilePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                string outputFolder = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(dllFilePath), "../../../output"));
                if (!Directory.Exists(outputFolder))
                    Directory.CreateDirectory(outputFolder);

                string cookbookFolder = @"C:\Users\scott\Documents\GitHub\ScottPlot\src\ScottPlot4\ScottPlot.Cookbook";
                string outputFolderImages = Path.Combine(outputFolder, "images");
                string outputJsonFile = Path.Combine(outputFolder, "recipes.json");
                GenerateEverything(outputFolderImages, outputJsonFile, cookbookFolder);
            }
            else
            {
                var res = Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(RunOptions);
            }
        }

        static void RunOptions(CommandLineOptions opts)
        {
            GenerateEverything(opts.OutImageFolder, opts.OutJsonFile, opts.CookbookFolder);
        }

        private static void GenerateEverything(string outputImageFolder, string outputJsonFile, string cookbookFolder)
        {
            Stopwatch sw = Stopwatch.StartNew();

            Console.WriteLine($"Generating images: {outputImageFolder}");
            RecipeImages.Generate(outputImageFolder);

            Console.WriteLine($"Generating source: {outputJsonFile}");
            RecipeJson.Generate(cookbookFolder, outputJsonFile);

            Console.WriteLine($"Finished in {sw.Elapsed.TotalSeconds:F3} seconds");
        }
    }
}
