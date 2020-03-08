using System;
using System.Collections.Generic;
using System.Linq;
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
            IPlotDemo[] recipes = Reflection.GetPlotsInOrder();
            CreateReport(recipes, sourceCodeFolder, outputFolder);
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

        private static void CreateReport(IPlotDemo[] recipes, string sourceCodeFolder, string outputFolder)
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
                html.AppendLine($"<div align='center'><img src='images/{recipe.id}.png'></div>");
                html.AppendLine("<div style='margin: 20px;'>&nbsp;</div>");
                htmlTOC.AppendLine($"<li><a href='#{recipe.id}'>{title}</a></li>");
            }

            md.Insert(0, $"# ScottPlot {Tools.GetVersionString()} Cookbook\n\n" + $"_Generated on {DateTime.Now.ToString("D")} at {DateTime.Now.ToString("t")}_\n\n" + mdTOC.ToString() + "\n\n---\n\n");
            System.IO.File.WriteAllText(outputFolder + "/readme.md", md.ToString());

            string style = @"
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
            ";

            htmlTOC.Append("<hr>");
            html.Insert(0, $"<div style='margin: 5px;'>{htmlTOC.ToString()}</div>");
            html.Insert(0, $"<div class='title'>ScottPlot {Tools.GetVersionString()} Cookbook</div><div class='subtitle'>Generated on {DateTime.Now.ToString("D")} at {DateTime.Now.ToString("t")}</div>");
            html.Insert(0, $"<html><head><script src='https://cdn.jsdelivr.net/gh/google/code-prettify@master/loader/run_prettify.js'></script><style>{style}</style></head><body><article>");
            html.AppendLine("</article></body><html>");

            System.IO.File.WriteAllText(outputFolder + "/index.html", html.ToString());
        }
    }
}
