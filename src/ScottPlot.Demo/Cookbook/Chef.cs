using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Cookbook
{
    public static class Chef
    {
        public static void MakeCookbook(string outputFolder = "./output")
        {
            // TODO: need to make the render function save images with the same filename as the code example file writer. This is not trivial!

            outputFolder = System.IO.Path.GetFullPath(outputFolder);
            ResetOutputFolder(outputFolder);

            IPlotDemo[] recipes = Reflection.GetPlots();
            Console.WriteLine($"Found {recipes.Length} recipes");
            RenderAllCookbookDemos(recipes, outputFolder);

            CodeBlock[] codeBlocks = LoadAllCodeBlocks();
            Console.WriteLine($"Found {codeBlocks.Length} code blocks");
            WiteAllCodeBlocks(codeBlocks, outputFolder);

            if (recipes.Length != codeBlocks.Length)
                throw new InvalidOperationException("there should be the same number of codeBlocks as recipes");

        }

        private static void WiteAllCodeBlocks(CodeBlock[] codeBlocks, string outputFolder)
        {
            outputFolder = System.IO.Path.Combine(outputFolder, "code");
            foreach (var cb in codeBlocks)
            {
                string filePath = System.IO.Path.Combine(outputFolder, cb.id + ".cs");
                System.IO.File.WriteAllText(filePath, cb.source);
                Console.WriteLine($"Saved: {filePath}");
            }
        }
        
        private static void ResetOutputFolder(string outputFolder)
        {
            if (System.IO.Directory.Exists(outputFolder))
                System.IO.Directory.Delete(outputFolder, recursive: true);
            System.IO.Directory.CreateDirectory(outputFolder);
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(outputFolder, "images"));
            System.IO.Directory.CreateDirectory(System.IO.Path.Combine(outputFolder, "code"));
        }

        private static void RenderAllCookbookDemos(IPlotDemo[] recipies, string outputFolder)
        {
            outputFolder = System.IO.Path.Combine(outputFolder, "images");
            foreach (IPlotDemo recipe in recipies)
            {
                string imageFilePath = $"{outputFolder}/{recipe.id}.png";
                var plt = new Plot(600, 400);
                recipe.Render(plt);
                plt.SaveFig(imageFilePath);
            }
        }
        
        public static Dictionary<string, string> ReadAllDemoSourceCode(string pathFolder)
        {
            var sourceByFile = new Dictionary<string, string>();

            string[] filePaths = System.IO.Directory.GetFiles(pathFolder, "*.cs", System.IO.SearchOption.AllDirectories);
            foreach (string filePath in filePaths)
            {
                string shortPath = filePath.Replace(pathFolder, "");
                string rawText = System.IO.File.ReadAllText(filePath);
                if (rawText.Replace(" ", "").Contains(":PlotDemo,IPlotDemo") && !shortPath.StartsWith("Cookbook"))
                    sourceByFile.Add(shortPath, rawText);
                else
                    Console.WriteLine(shortPath);
            }

            return sourceByFile;
        }

        private static CodeBlock[] LoadAllCodeBlocks()
        {
            string pathSrc = System.IO.Path.GetFullPath("../../../../src");
            string pathDemoFolder = System.IO.Path.GetFullPath(pathSrc + "/ScottPlot.Demo/");
            Dictionary<string, string> sourceByFile = ReadAllDemoSourceCode(pathDemoFolder);
            Console.WriteLine($"Found {sourceByFile.Count} source blocks");
            return GetCodeBlocks(sourceByFile);
        }

        private static CodeBlock[] GetCodeBlocks(Dictionary<string, string> sourceByFile)
        {
            List<CodeBlock> codeBlocks = new List<CodeBlock>();
            foreach (var d in sourceByFile)
            {
                StringBuilder sb = new StringBuilder();
                string source = d.Value;
                string filePath = d.Key;

                foreach (string line in source.Split('\n'))
                {
                    if (line.Trim().EndsWith("IPlotDemo"))
                    {
                        var block = new CodeBlock(sb.ToString(), filePath);
                        if (block.isValid)
                            codeBlocks.Add(block);
                        sb.Clear();
                    }
                    sb.AppendLine(line);
                }

                var block2 = new CodeBlock(sb.ToString(), filePath);
                if (block2.isValid)
                    codeBlocks.Add(block2);
            }

            return codeBlocks.ToArray();
        }
    }
}
