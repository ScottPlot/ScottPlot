using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Site
{
    public class SiteGenerator
    {
        readonly string ImageExtension = ".png";
        readonly string CodeExtension = ".cs";

        public SiteGenerator(string recipeFolder)
        {
            if (!Directory.Exists(recipeFolder))
                throw new ArgumentException("recipe folder not found");
            recipeFolder = Path.GetFullPath(recipeFolder);

            string[] ids = Directory.GetFiles(recipeFolder, "*.cs")
                                    .Select(x => Path.GetFileNameWithoutExtension(x))
                                    .ToArray();

            StringBuilder sb = new StringBuilder();
            foreach (string id in ids)
                sb.AppendLine(GetHtmlRecipe(recipeFolder, id));

            string html = WrapInBody(sb.ToString());
            string htmlFilePath = Path.Combine(recipeFolder, "all.html");
            File.WriteAllText(htmlFilePath, html);
            Console.WriteLine($"Saved website in: {htmlFilePath}");
        }

        private string WrapInBody(string content)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html><body>");
            sb.AppendLine(content);
            sb.AppendLine("</body></html>");
            return sb.ToString();
        }

        private string GetHtmlRecipe(string recipeFolder, string id)
        {
            StringBuilder sb = new StringBuilder();
            string codeFilePath = Path.Combine(recipeFolder, id + CodeExtension);
            string imageUrl = id + ImageExtension;
            string[] raw = File.ReadAllLines(codeFilePath);
            string title = raw[0].Substring(2).Trim();
            string description = raw[1].Substring(2).Trim();
            string code = string.Join("<br>\n", raw.Skip(2));
            sb.AppendLine($"<div><b>{title}</b></div>");
            sb.AppendLine($"<div><i>{description}</i></div>");
            sb.AppendLine($"<div style='display: inline-block; background-color: #efefef; padding: 5px; margin: 10px;'>"+
                $"<code>{code}</code></div>");
            sb.AppendLine($"<div><img src='{imageUrl}' /></div>");
            sb.AppendLine($"<hr>");
            return sb.ToString();
        }
    }
}
