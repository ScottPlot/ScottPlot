using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Site
{
    /// <summary>
    /// This class reads content of the recipe folder to generate HTML from IDs.
    /// It does not store structures (like what IDs go on what pages).
    /// </summary>
    public class SiteGenerator
    {
        readonly string ImageExtension = ".png";
        readonly string CodeExtension = ".cs";
        readonly string RecipeFolder;

        public SiteGenerator(string recipeFolder)
        {
            if (!Directory.Exists(recipeFolder))
                throw new ArgumentException("recipe folder not found");
            RecipeFolder = Path.GetFullPath(recipeFolder);
        }

        /// <summary>
        /// Sanitize text to a url-friendly string
        /// </summary>
        public string Sanitize(string s) => s.ToLower().Replace(" ", "_").Replace(":", "");

        /// <summary>
        /// Create a webpage containing just the recipe IDs specified
        /// </summary>
        public void MakeCookbookPage(string[] ids, string title)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string id in ids)
                sb.AppendLine(GetHtmlRecipe(RecipeFolder, id));
            string html = WrapInBody(sb.ToString(), title);
            string htmlFilePath = Path.Combine(RecipeFolder, Sanitize(title) + ".html");
            File.WriteAllText(htmlFilePath, html);
            Console.WriteLine($"Saved: {htmlFilePath}");
        }

        /// <summary>
        /// Return HTML (title, code, and image) for a single recipe ID
        /// </summary>
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
            sb.AppendLine($"<div style='display: inline-block; background-color: #efefef; padding: 5px; margin: 10px;'>" +
                $"<code>{code}</code></div>");
            sb.AppendLine($"<div><img src='{imageUrl}' /></div>");
            sb.AppendLine($"<hr>");
            return sb.ToString();
        }

        /// <summary>
        /// Turn a chunk of HTML into a webpage by wrapping in HTML and BODY tags
        /// </summary>
        private string WrapInBody(string content, string title)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html><body>");
            sb.AppendLine($"<h1>{title}</h1>");
            sb.AppendLine(content);
            sb.AppendLine("</body></html>");
            return sb.ToString();
        }
    }
}
