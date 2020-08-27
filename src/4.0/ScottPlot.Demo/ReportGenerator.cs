using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo
{
    public class ReportGenerator
    {
        public readonly string outputFolder;
        public readonly string sourceCodeFolder;
        public readonly int width;
        public readonly int height;

        public readonly IPlotDemo[] recipes;

        public ReportGenerator(int width = 600, int height = 400, string sourceFolder = null, string outputFolder = "./output", bool useDLL = false)
        {

            recipes = Reflection.GetPlotsInOrder(useDLL);

            if (sourceFolder is null)
                sourceCodeFolder = Reflection.FindDemoSourceFolder();
            else
                sourceCodeFolder = sourceFolder;

            if (sourceCodeFolder is null)
                throw new ArgumentException("can't locate source code");

            this.outputFolder = System.IO.Path.GetFullPath(outputFolder);
            this.width = width;
            this.height = height;

            if (!System.IO.Directory.Exists(sourceCodeFolder))
                throw new ArgumentException("cookbook source folder does not exist: " + this.sourceCodeFolder);
            if (!System.IO.File.Exists(sourceCodeFolder + "/IPlotDemo.cs"))
                throw new ArgumentException("IPlotDemo.cs cannot be found in: " + this.sourceCodeFolder);
        }

        /// <summary>
        /// Perform all steps in sequence to clear, create, and index the cookbook.
        /// </summary>
        public void MakeCookbookAllAtOnce()
        {
            ClearFolders();
            foreach (IPlotDemo recipe in Reflection.GetPlots())
                CreateImage(recipe);
            MakeReports();
        }

        /// <summary>
        /// Erase previous cookbook and create empty folder structure
        /// </summary>
        public void ClearFolders()
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

        /// <summary>
        /// Render the given demo plot and save it as a PNG file
        /// </summary>
        public void CreateImage(IPlotDemo recipe)
        {
            string imageFilePath = $"{outputFolder}/images/{recipe.id}.png";

            if (recipe is IBitmapDemo bmpDemo)
            {
                System.Drawing.Bitmap bmp = bmpDemo.Render(width, height);
                bmp.Save(imageFilePath, System.Drawing.Imaging.ImageFormat.Png);
            }
            else
            {
                Plot plt;
                if (recipe.categoryMinor == "AllColormaps" || recipe.categoryMinor == "CustomColormap")
                {
                    plt = new Plot(width, 100);
                }
                else
                {
                    plt = new Plot(width, height);
                }
                recipe.Render(plt);
                plt.SaveFig(imageFilePath);
            }
        }

        /// <summary>
        /// Create HTML and Markdown cookbook pages
        /// </summary>
        public void MakeReports()
        {
            StringBuilder md = new StringBuilder();

            string lastMajorCategory = "";
            foreach (IPlotDemo recipe in recipes)
            {
                string title = $"{recipe.categoryMajor}: {recipe.categoryMinor} - {recipe.name}";
                string sourceCode = recipe.GetSourceCode(sourceCodeFolder);
                sourceCode = $"// Code from {recipe.sourceFile}\n{sourceCode}";
                string description = (recipe.description is null) ? "no description provided..." : recipe.description;

                if (recipe.categoryMajor != lastMajorCategory)
                {
                    lastMajorCategory = recipe.categoryMajor;
                    md.AppendLine($"## {recipe.categoryMajor}\n\n");
                }

                md.AppendLine($"### {title}\n\n");
                md.AppendLine($"{description}\n\n");
                md.AppendLine($"```cs\n{sourceCode}\n```\n\n");
                md.AppendLine($"![](images/{recipe.id}.png)\n\n");
            }

            md.Insert(0, $"## Table of Contents\n\n![](TOC)\n\n");
            md.Insert(0, $"![](cookbookNote.md)\n\n");
            md.Insert(0, $"_Generated on {DateTime.Now.ToString("D")} at {DateTime.Now.ToString("t")}_\n\n");
            md.Insert(0, $"# ScottPlot {Tools.GetVersionString()} Cookbook\n\n");

            System.IO.File.WriteAllText(outputFolder + "/readme.md", md.ToString());
        }
    }
}
