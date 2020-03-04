using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ScottPlot.Demo.Cookbook
{
    public class Generator
    {
        public Generator(string sourceDir = "../../../../src/", string outputDir = "./output")
        {
            sourceDir = System.IO.Path.GetFullPath(sourceDir);

            if (!System.IO.File.Exists(sourceDir + "/ScottPlotV4.sln"))
                throw new ArgumentException("cookbook generator requires src folder to be 3 folders up");

            if (!System.IO.File.Exists(sourceDir + "/ScottPlot.Demo/ScottPlot.Demo.csproj"))
                throw new ArgumentException("ScottPlot.Demo.csproj cannot be located");

            LoadSourceFiles(sourceDir + "/ScottPlot.Demo");
        }

        private void LoadSourceFiles(string sourceFolder)
        {
            string[] filePaths = System.IO.Directory.GetFiles(sourceFolder, "*.cs", System.IO.SearchOption.AllDirectories);
            Console.WriteLine($"Found {filePaths.Length} source files:");

            List<CodeBlock> demoBlocks = new List<CodeBlock>();
            foreach (var filePath in filePaths)
                demoBlocks.AddRange(GetCodeBlocks(filePath));
        }

        private List<CodeBlock> GetCodeBlocks(string filePath)
        {
            List<string> textBlocks = new List<string>();
            List<string> thisBlock = new List<string>();

            foreach (string rawLine in System.IO.File.ReadLines(filePath))
            {
                string line = rawLine.Trim(new char[] { '\n', '\r' });
                if (line.EndsWith(": IPlotDemo"))
                {
                    textBlocks.Add(String.Join("\n", thisBlock));
                    thisBlock.Clear();
                }
                thisBlock.Add(line);
            }

            List<CodeBlock> codeBlocks = new List<CodeBlock>();
            foreach (var textBlock in textBlocks)
            {
                string firstLine = textBlock.Split('\n')[0];
                if (firstLine.Contains(": IPlotDemo"))
                    codeBlocks.Add(new CodeBlock(textBlock, filePath));
            }

            return codeBlocks;
        }
    }
}
