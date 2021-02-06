using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ScottPlot.Cookbook.Website
{
    public class Page
    {
        private readonly List<IPageElement> Elements = new List<IPageElement>();

        /// <summary>
        /// This controls the meta title
        /// </summary>
        public string Title;

        /// <summary>
        /// This controls the meta description
        /// </summary>
        public string Description;

        public readonly string ScottPlotVersion = Plot.Version;

        public Page() { }

        private void Add(IPageElement element) =>
            Elements.Add(element);

        public void AddCodeBlock(string code, string language) => Add(new CodeBlock(code, language));

        public void AddHeading(string text, int level = 1) =>
            Add(new Heading(text, level));

        public void AddHtml(string html) => Add(new RawHtml(html));

        public void StartUl() => Add(new RawHtml("<ul>"));
        public void EndUl() => Add(new RawHtml("</ul>"));
        public void AddLi(string text) => Add(new ListItem(text));

        public void AddParagraph(string text) => Add(new Paragraph(text));

        public void AddImage(string url, bool linkToItself = true, bool center = false) =>
            Add(new Image(url, linkToItself, center));

        public void AddThumbnailImage(string urlImage, string urlThumbnail, bool center = false) =>
            Add(new Image(urlImage, urlThumbnail, center));

        public void AddSpacer() => Add(new Spacer());

        public void AddRecipeCard(IRecipe recipe) => Add(new RecipeCard(recipe));

        public string GetHtml(string template)
        {
            string html = template;
            html = html.Replace("{{title}}", Title);
            html = html.Replace("{{description}}", Description);
            html = html.Replace("{{timestamp}}", $"{DateTime.Now:u}");
            html = html.Replace("{{date}}", $"{DateTime.Now:D}");
            html = html.Replace("{{dotnetVersion}}", Environment.Version.ToString());
            html = html.Replace("{{version}}", ScottPlotVersion);
            html = html.Replace("{{content}}", string.Join("\n", Elements.Select(x => x.Html)));
            return html;
        }

        public void SaveHtml(string filePath, string template = "{{content}}") =>
            File.WriteAllText(filePath, GetHtml(template));

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

        public static string Sanitize(string s)
        {
            s = s.ToLower().Replace(" ", "_").Replace(":", "");
            s = Regex.Replace(s, "[^a-zA-Z0-9_.]+", "", RegexOptions.Compiled);
            return s;
        }

        public static string GetCategoryUrl(IRecipe recipe) => $"cookbook-{Sanitize(recipe.Category)}.html";
        public static string GetRecipeUrl(IRecipe recipe) => $"cookbook-{Sanitize(recipe.Category)}.html#{Sanitize(recipe.Title)}";
        public static string GetImageUrl(IRecipe recipe) => $"images/{Sanitize(recipe.ID)}.png";
        public static string GetThumbnailUrl(IRecipe recipe) => $"images/{Sanitize(recipe.ID)}_thumb.jpg";
    }
}
