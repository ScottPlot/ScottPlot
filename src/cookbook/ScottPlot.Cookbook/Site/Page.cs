using System;
using System.IO;
using System.Linq;
using System.Text;

namespace ScottPlot.Cookbook.Site
{
    /// <summary>
    /// This object represents a single flat-file web page
    /// </summary>
    public class Page
    {
        protected readonly string ExtCode = ".cs";
        protected readonly string ExtPage = ".html";
        protected readonly string ExtImage = ".png";
        protected readonly string ExtThumb = "_thumb.jpg";
        protected readonly string SiteFolder;
        protected readonly string ResourceFolder;
        protected readonly StringBuilder SB = new StringBuilder();

        public Page(string cookbookSiteFolder, string sourceFolder)
        {
            SiteFolder = Path.GetFullPath(cookbookSiteFolder);
            ResourceFolder = Path.GetFullPath(Path.Combine(sourceFolder, "Resources"));
        }

        public static string Sanitize(string s) => s.ToLower().Replace(" ", "_").Replace(":", "");
        public static string[] Sanitize(string[] s) => s.Select(x => Sanitize(x)).ToArray();
        public void AddDiv(string html) => SB.AppendLine($"<div>{html}</div>");
        public void AddDiv(string html, string divClass) => SB.AppendLine($"<div class='{divClass}'>{html}</div>");
        public void AddHTML(string html) => SB.AppendLine(html);
        public void DivStart(string divClass = null) => SB.AppendLine($"<div class='{divClass}'>");
        public void DivEnd() => SB.AppendLine("</div>");
        public void UlStart() => SB.AppendLine($"<ul>");
        public void UlEnd() => SB.AppendLine("</ul>");
        public void Li(string html) => SB.AppendLine($"<li>{html}</li>");

        public void AddCode(string code)
        {
            SB.AppendLine($"<pre class='prettyprint cs'>{code}</pre>");
        }

        public void SaveAs(string fileName, string title)
        {
            if (!fileName.EndsWith(ExtPage))
                fileName = Sanitize(fileName) + ExtPage;
            fileName = Path.GetFileName(fileName);
            string filePath = Path.Combine(SiteFolder, fileName);
            File.WriteAllText(filePath, ApplyTemplate(title));
            Console.WriteLine($"Saved: {filePath}");
        }

        private string ApplyTemplate(string title)
        {
            string html = File.ReadAllText(Path.Combine(ResourceFolder, "Template.html"));

            // shows in the head area
            string pageTitle = string.IsNullOrWhiteSpace(title) ?
                "ScottPlot Cookbook" :
                $"ScottPlot Cookbook: {title}";
            html = html.Replace("{{title}}", pageTitle);

            // shows at the top of the page
            string htmlTitle = string.IsNullOrWhiteSpace(title) ?
                $"ScottPlot Cookbook" :
                $"<a href='./index.html' style='color: black;'>ScottPlot Cookbook</a>: {title}";
            string warning = "" +
                "\n<blockquote>" +
                "<b>⚠️</b> <strong>Documentation is version-specific:</strong> " +
                "This page was generated for <code>ScottPlot 1.2.3-beta</code><br> " +
                "Additional documentation and more version-specific cookbooks are on the " +
                "<a href='https://swharden.com/scottplot' style='font-weight: 600;'>ScottPlot Website</a>" +
                "</blockquote>\n";

            // assemble the article
            string content = $"<h1>{htmlTitle}</h1>" + warning + SB.ToString();
            html = html.Replace("{{content}}", content);

            html = html.Replace("{{version}}", $"ScottPlot {Plot.Version}");
            html = html.Replace("{{date}}", DateTime.Now.ToString("MMMM dd, yyyy"));

            return html;
        }
    }
}
