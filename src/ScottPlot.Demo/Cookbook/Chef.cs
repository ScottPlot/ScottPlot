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
                md.AppendLine($"## {recipe.name}\n\n");
                md.AppendLine($"**Description:** {recipe.description}\n\n");
                md.AppendLine($"**Source code:** {recipe.categoryMajor}.{recipe.categoryMinor} ({recipe.categoryClass})\n\n");
                md.AppendLine($"```cs\n{recipe.GetSourceCode(sourceCodeFolder)}\n```\n\n");
                md.AppendLine($"![](images/{recipe.id}.png)\n\n");
            }

            System.IO.File.WriteAllText(outputFolder + "/index.html", html.ToString());
            System.IO.File.WriteAllText(outputFolder + "/readme.md", md.ToString());
        }
    }
}
