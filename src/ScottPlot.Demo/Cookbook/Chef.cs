using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Cookbook
{
    public static class Chef
    {
        public static void MakeCookbook(string sourceCodeFolder, string outputFolder = "./output")
        {
            outputFolder = System.IO.Path.GetFullPath(outputFolder);
            Console.WriteLine($"Generating cookbook in: {outputFolder}");
            ClearFolders(outputFolder);
            RenderAndSaveImages(outputFolder);
            CreateReport(sourceCodeFolder, outputFolder);
        }

        private static void ClearFolders(string outputFolder)
        {
            if (!System.IO.Directory.Exists(outputFolder))
                System.IO.Directory.CreateDirectory(outputFolder);
            foreach (string filePathToDelete in System.IO.Directory.GetFiles(outputFolder, "*.*"))
                System.IO.File.Delete(filePathToDelete);

            string imageFolder = System.IO.Path.Combine(outputFolder, "images");
            if (!System.IO.Directory.Exists(imageFolder))
                System.IO.Directory.CreateDirectory(imageFolder);
            foreach (string filePathToDelete in System.IO.Directory.GetFiles(imageFolder, "*.*"))
                System.IO.File.Delete(filePathToDelete);
        }

        private static void RenderAndSaveImages(string outputFolder)
        {
            foreach (IPlotDemo recipe in Reflection.GetPlots())
            {
                var plt = new Plot(600, 400);
                recipe.Render(plt);
                plt.SaveFig($"{outputFolder}/images/{recipe.id}.png");
            }
        }

        private static void CreateReport(string sourceCodeFolder, string outputFolder)
        {
            StringBuilder md = new StringBuilder();
            StringBuilder html = new StringBuilder();

            foreach (IPlotDemo recipe in Reflection.GetPlots())
            {
                string title = $"{recipe.categoryMajor}/{recipe.categoryMinor} - {recipe.name}";
                string sourceCode = $"// Source code is from {recipe.sourceFile} ({recipe.categoryClass})\n\n{recipe.GetSourceCode(sourceCodeFolder)}";

                md.AppendLine($"## {title}\n\n");
                md.AppendLine($"{recipe.description}\n\n");
                md.AppendLine($"```cs\n{sourceCode}\n```\n\n");
                md.AppendLine($"![](images/{recipe.id}.png)\n\n");

                html.AppendLine($"<div style='font-size: 150%; font-weight: bold;'><a style='color: black;' id='{recipe.id}' href='#{recipe.id}'>{title}</a></div>\n\n");
                html.AppendLine($"<div style='padding: 10px;'>{recipe.description}</div>");
                html.AppendLine($"<pre style='background-color: #f6f8fa; padding: 10px;'>{sourceCode}</pre>");
                html.AppendLine($"<img src='images/{recipe.id}.png'>");
                html.AppendLine("<div style='margin: 20px;'>&nbsp;</div>");
            }

            System.IO.File.WriteAllText(outputFolder + "/readme.md", md.ToString());

            string style = @"
                body { font-family: sans-serif; }
                a { text-decoration: none; }
                a:hover { text-decoration: underline; }
            ";

            html.Insert(0, $"<html><head><style>{style}</style></head><body>");
            html.AppendLine("</body><html>");

            System.IO.File.WriteAllText(outputFolder + "/index.html", html.ToString());
        }
    }
}
