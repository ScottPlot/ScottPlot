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

        readonly IPlotDemo[] recipes = Reflection.GetPlotsInOrder();

        public ReportGenerator(int width = 600, int height = 400, string sourceFolder = null, string outputFolder = "./output")
        {
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
                var plt = new Plot(width, height);
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
            StringBuilder html = new StringBuilder();
            StringBuilder mdTOC = new StringBuilder();
            StringBuilder htmlTOC = new StringBuilder();

            string recipeTemplate = System.IO.File.ReadAllText(sourceCodeFolder + "/Template/recipe.html");

            foreach (IPlotDemo recipe in recipes)
            {
                string title = $"{recipe.categoryMajor}/{recipe.categoryMinor} - {recipe.name}";
                string sourceCode = recipe.GetSourceCode(sourceCodeFolder);
                string description = (recipe.description is null) ? "no description provided..." : recipe.description;

                mdTOC.AppendLine($"* [{title}](#{recipe.id})");
                htmlTOC.AppendLine($"<li><a href='#{recipe.id}'>{title}</a></li>");

                md.AppendLine($"## {title}\n\n");
                md.AppendLine($"{description}\n\n");
                md.AppendLine($"```cs\n{sourceCode}\n```\n\n");
                md.AppendLine($"![](images/{recipe.id}.png)\n\n");

                string htmlRecipe = recipeTemplate.ToString();
                htmlRecipe = htmlRecipe.Replace("~ID~", recipe.id);
                htmlRecipe = htmlRecipe.Replace("~TITLE~", recipe.name);
                htmlRecipe = htmlRecipe.Replace("~DESCRIPTION~", recipe.description);
                htmlRecipe = htmlRecipe.Replace("~SOURCE~", $"{recipe.sourceFile} ({recipe.categoryClass})");
                htmlRecipe = htmlRecipe.Replace("~CODE~", sourceCode);
                html.AppendLine(htmlRecipe);
            }

            md.Insert(0, $"# ScottPlot {Tools.GetVersionString()} Cookbook\n\n" + $"_Generated on {DateTime.Now.ToString("D")} at {DateTime.Now.ToString("t")}_\n\n" + mdTOC.ToString() + "\n\n---\n\n");
            System.IO.File.WriteAllText(outputFolder + "/readme.md", md.ToString());

            string htmlString = System.IO.File.ReadAllText(sourceCodeFolder + "/Template/page.html");
            htmlString = htmlString.Replace("~TOC~", htmlTOC.ToString());
            htmlString = htmlString.Replace("~CONTENT~", html.ToString());
            htmlString = htmlString.Replace("~VERSION~", Tools.GetVersionString());
            htmlString = htmlString.Replace("~DATE~", DateTime.Now.ToString("D"));
            htmlString = htmlString.Replace("~TIME~", DateTime.Now.ToString("t"));

            System.IO.File.WriteAllText(outputFolder + "/index.html", htmlString);
        }
    }
}
