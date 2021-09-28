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
                Parser.Default.ParseArguments<CommandLineOptions>(args).WithParsed(RunOptions);
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
            CreateMultiFileCookbookSource(cookbookFolder, outputCodeFolder);
            CreateRecipesJson(cookbookFolder, Path.Combine(outputCodeFolder, "recipes.json"));

            Console.WriteLine($"Finished in {sw.Elapsed.TotalSeconds:F3} seconds");
        }

        private static void CreateMultiFileCookbookSource(string sourcePath, string outputPath)
        {
            outputPath = Path.GetFullPath(outputPath);
            if (!Directory.Exists(outputPath))
                Directory.CreateDirectory(outputPath);

            RecipeSource[] sources = SourceParsing.GetRecipeSources(sourcePath, 600, 400);

            foreach (RecipeSource recipe in sources)
            {
                StringBuilder sb = new();
                sb.AppendLine("/// ID: " + recipe.ID);
                sb.AppendLine("/// TITLE: " + recipe.Title);
                sb.AppendLine("/// CATEGORY: " + recipe.Category);
                sb.AppendLine("/// DESCRIPTION: " + recipe.Description);
                sb.Append(recipe.Code);

                string filePath = Path.Combine(outputPath, recipe.ID.ToLower() + ".cs");
                File.WriteAllText(filePath, sb.ToString());
                Debug.WriteLine($"Saved: {filePath}");
            }
        }

        private static void CreateRecipesJson(string cookbookFolder, string saveFilePath)
        {
            RecipeSource[] recipes = SourceParsing.GetRecipeSources(cookbookFolder, 600, 400);

            using var stream = File.OpenWrite(saveFilePath);
            var options = new JsonWriterOptions() { Indented = true };
            using var writer = new Utf8JsonWriter(stream, options);

            writer.WriteStartObject();
            writer.WriteString("version", ScottPlot.Plot.Version);
            writer.WriteString("generated", DateTime.UtcNow);

            writer.WriteStartArray("recipes");
            foreach (RecipeSource recipe in recipes)
            {
                writer.WriteStartObject();
                writer.WriteString("id", recipe.ID.ToLower());
                writer.WriteString("category", recipe.Category);
                writer.WriteString("title", recipe.Title);
                writer.WriteString("description", recipe.Description);
                writer.WriteString("code", recipe.Code.Replace("\r", ""));
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
    }
}
