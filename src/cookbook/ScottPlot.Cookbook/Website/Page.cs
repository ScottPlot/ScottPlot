using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ScottPlot.Cookbook.Website
{
    /// <summary>
    /// This class represents a markdown page that will become a single page on the website.
    /// At present it only exports Markdown, but HTML could be added to IPageElement to support HTML too.
    /// </summary>
    public class Page
    {
        private readonly List<IPageElement> Elements = new List<IPageElement>();
        public string Title;
        public string Description;

        public static string Sanitize(string s)
        {
            s = s.ToLower().Replace(" ", "-").Replace(":", "");
            s = Regex.Replace(s, "[^a-zA-Z0-9-_.]+", "", RegexOptions.Compiled);
            return s;
        }

        private void Add(IPageElement element) =>
            Elements.Add(element);

        public void Add(string rawText) =>
            Add(new RawText(rawText));

        public void AddHeading(string text, int level = 1) =>
            Add(new Heading(text, level));

        public void AddCodeBlock(string code, string language) =>
            Add(new CodeBlock(code, language));

        public void AddImage(string url, bool linkToItself = true, bool center = false) =>
            Add(new Image(url, linkToItself, center));

        public void AddThumbnailImage(string urlImage, string urlThumbnail, bool center = false) =>
            Add(new Thumbnail(urlImage, urlThumbnail, center));

        public void AddSpacer() => Add(new Spacer());

        public void AddVersionWarning() => Add(new VersionWarning());

        public void AddRecipeCard(IRecipe recipe) => Add(new RecipeCard(recipe));

        public string GetMarkdown(bool header = true)
        {
            StringBuilder sb = new StringBuilder();

            if (header)
            {
                sb.AppendLine("---");
                sb.AppendLine($"title: {Title}");
                sb.AppendLine($"description: {Description}");
                sb.AppendLine($"date: {DateTime.Now:u}");
                sb.AppendLine("---");
                sb.AppendLine();
            }

            foreach (var elem in Elements)
            {
                sb.AppendLine(elem.Markdown);
                sb.AppendLine();
            }

            string md = sb.ToString();
            md = md.Replace("\r\n", "\n");
            while (md.Contains("\n\n\n"))
                md = md.Replace("\n\n\n", "\n\n");

            return md;
        }

        public void SaveMarkdown(string filePath) => File.WriteAllText(filePath, GetMarkdown());
    }
}
