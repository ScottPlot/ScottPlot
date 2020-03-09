using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Demo.Cookbook
{
    public class Chef
    {
        public readonly string outputFolder;
        public readonly string sourceCodeFolder;
        int width;
        int height;
        readonly IPlotDemo[] recipes = Reflection.GetPlotsInOrder();

        public Chef(string sourceCodeFolder, string outputFolder = "./output", int width = 600, int height = 400)
        {
            this.outputFolder = System.IO.Path.GetFullPath(outputFolder);
            this.sourceCodeFolder = System.IO.Path.GetFullPath(sourceCodeFolder);
            this.width = width;
            this.height = height;

            if (!System.IO.Directory.Exists(sourceCodeFolder))
                throw new ArgumentException("cookbook source folder does not exist: " + this.sourceCodeFolder);
            if (!System.IO.File.Exists(sourceCodeFolder+ "/IPlotDemo.cs"))
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

            foreach (IPlotDemo recipe in recipes)
            {
                string title = $"{recipe.categoryMajor}/{recipe.categoryMinor} - {recipe.name}";
                string sourceCode = recipe.GetSourceCode(sourceCodeFolder);
                string description = (recipe.description is null) ? "no description provided..." : recipe.description;

                md.AppendLine($"## {title}\n\n");
                md.AppendLine($"{description}\n\n");
                md.AppendLine($"```cs\n{sourceCode}\n```\n\n");
                md.AppendLine($"![](images/{recipe.id}.png)\n\n");
                mdTOC.AppendLine($"* [{title}](#{recipe.id})");

                html.AppendLine($"<div class='title'><a style='color: black;' id='{recipe.id}' href='#{recipe.id}'>{title}</a></div>\n\n");
                html.AppendLine($"<div class='description'>{description}</div>");
                html.AppendLine($"<div class='description2'>{recipe.sourceFile} ({recipe.categoryClass}):</div>");
                html.AppendLine($"<pre class='prettyprint lang - cs' style='padding: 10px; margin: 0px; background: #f6f8fa; border: 0px solid white;'>{sourceCode}</pre>");
                html.AppendLine($"<div align='center' style='margin: 10px;'><img src='images/{recipe.id}.png'></div>");
                html.AppendLine("<div style='margin: 20px;'>&nbsp;</div>");
                htmlTOC.AppendLine($"<li><a href='#{recipe.id}'>{title}</a></li>");
            }

            md.Insert(0, $"# ScottPlot {Tools.GetVersionString()} Cookbook\n\n" + $"_Generated on {DateTime.Now.ToString("D")} at {DateTime.Now.ToString("t")}_\n\n" + mdTOC.ToString() + "\n\n---\n\n");
            System.IO.File.WriteAllText(outputFolder + "/readme.md", md.ToString());

            string style = @"
            <style>
                body { font-family: -apple-system,BlinkMacSystemFont,Segoe UI,Helvetica,Arial,sans-serif,Apple Color Emoji,Segoe UI Emoji; }
                a { text-decoration: none; color: blue; }
                a:hover { text-decoration: underline; }
                li { margin-left: 15px; }
                article { width: 900px; margin: auto; }
                .title {border-bottom: 1px solid #eaecef; font-size: 150%; font-weight: 600;}
				.subtitle {margin-bottom: 10px; font-style: italic;}
                .description{margin-top: 10px; margin-bottom: 10px;}
                .description2{margin-top: 5px; font-size: 70%; color: lightgray; font-family: consolas, monospace, sans-serif;}
                hr { margin: 30px; border: 0px solid #eaecef;}
            </style>";

            string htmlSyntaxHighlighter = "<script src='https://cdn.jsdelivr.net/gh/google/code-prettify@master/loader/run_prettify.js'></script>";
            string analytics = @"
                <!-- Global site tag (gtag.js) - Google Analytics -->
                <script async src='https://www.googletagmanager.com/gtag/js?id=UA-560719-1'></script>
                <script>
                  window.dataLayer = window.dataLayer || [];
                  function gtag(){dataLayer.push(arguments);}
                  gtag('js', new Date());
                  gtag('config', 'UA-560719-1');
                </script>";

            htmlTOC.Append("<hr>");
            html.Insert(0, $"<div style='margin: 5px;'>{htmlTOC.ToString()}</div>");
            html.Insert(0, $"<div class='title'>ScottPlot {Tools.GetVersionString()} Cookbook</div><div class='subtitle'>Generated on {DateTime.Now.ToString("D")} at {DateTime.Now.ToString("t")}</div>");
            html.Insert(0, $"<html><head>{htmlSyntaxHighlighter}{style}{analytics}</head><body><article>");
            html.AppendLine("</article></body><html>");

            System.IO.File.WriteAllText(outputFolder + "/index.html", html.ToString());
        }
    }
}
