using ScottPlot.Cookbook.XmlDocumentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

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
        private const string XML_DOC_PATH = "../../../../../src/ScottPlot/ScottPlot.xml";

        public static XDocument GetXmlDoc() => XDocument.Load(XML_DOC_PATH);

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

        public void AddVersionWarning(string documentationName) => Add(new VersionWarning(documentationName));

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

        protected void AddDetailedInfo(DocumentedField f)
        {
            AddHeading(f.Name, 3);
            if (!string.IsNullOrWhiteSpace(f.Summary))
                Add($"**Summary:** {f.Summary}");
            Add($"**Name:** `{f.FullName}`");
            Add($"**Type:** `{f.Type}`");
        }

        protected void AddDetailedInfo(DocumentedProperty p)
        {
            AddHeading(p.Name, 3);
            if (!string.IsNullOrWhiteSpace(p.Summary))
                Add($"**Summary:** {p.Summary}");
            Add($"**Name:** `{p.FullName}`");
            Add($"**Type:** `{p.Type}`");
        }

        protected void AddDetailedInfo(DocumentedMethod p)
        {
            AddHeading(p.Name + "()", 3);
            if (!string.IsNullOrWhiteSpace(p.Summary))
                Add($"**Summary:** {p.Summary}");
            Add($"**Name:** `{p.FullName}`");
            Add($"**Signature:** `{p.Signature}`");

            if (p.Parameters.Any())
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"**Parameters:**");
                foreach (var p2 in p.Parameters)
                {
                    string summary = string.IsNullOrWhiteSpace(p2.Summary) ? "" : "- " + p2.Summary;
                    sb.AppendLine($"* `{p2.Type}` **{p2.Name}** {summary}");
                }
                Add(sb.ToString());
            }
        }

        protected string OneLineInfo(DocumentedClass docClass, string baseUrl)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append($"<a href='{baseUrl}'><strong>{docClass.Name}</strong></a>");
            if (!string.IsNullOrWhiteSpace(docClass.Summary))
                sb.Append(" - " + docClass.Summary);
            return sb.ToString();
        }

        protected string OneLineInfo(DocumentedMethod method, string baseUrl)
        {
            string url = Sanitize(method.Name);
            StringBuilder sb = new StringBuilder();
            sb.Append($"<a href='{baseUrl}#{url}'><strong>{method.Name}()</strong></a>");
            if (!string.IsNullOrWhiteSpace(method.Summary))
                sb.Append(" - " + method.Summary);
            return sb.ToString();
        }

        protected string OneLineInfo(DocumentedField field, string baseUrl)
        {
            string url = Sanitize(field.Name);
            StringBuilder sb = new StringBuilder();
            sb.Append($"<a href='{baseUrl}#{url}'><strong>{field.Name}</strong></a>");
            if (!string.IsNullOrWhiteSpace(field.Summary))
                sb.Append(" - " + field.Summary);
            return sb.ToString();
        }

        protected string OneLineInfo(DocumentedProperty property, string baseUrl)
        {
            string url = Sanitize(property.Name);
            StringBuilder sb = new StringBuilder();
            sb.Append($"<a href='{baseUrl}#{url}'><strong>{property.Name}</strong></a>");
            if (!string.IsNullOrWhiteSpace(property.Summary))
                sb.Append(" - " + property.Summary);
            return sb.ToString();
        }
    }
}
