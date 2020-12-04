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
        protected readonly string SiteFolder;
        protected readonly StringBuilder SB = new StringBuilder();

        public Page(string cookbookSiteFolder)
        {
            SiteFolder = Path.GetFullPath(cookbookSiteFolder);
        }

        public static string Sanitize(string s) => s.ToLower().Replace(" ", "_").Replace(":", "");

        public static string[] Sanitize(string[] s) => s.Select(x => Sanitize(x)).ToArray();

        public void AddDiv(string html) => SB.AppendLine($"<div>{html}</div>");

        public void AddHTML(string html) => SB.AppendLine(html);

        public void SaveAs(string fileName, string title)
        {
            if (!fileName.EndsWith(ExtPage))
                fileName = Sanitize(fileName) + ExtPage;
            fileName = Path.GetFileName(fileName);
            string filePath = Path.Combine(SiteFolder, fileName);
            string html = WrapInBody(SB.ToString(), title);
            File.WriteAllText(filePath, html);
            Console.WriteLine($"Saved: {filePath}");
        }

        private string WrapInBody(string content, string title)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html><head>");
            sb.AppendLine("<script src='https://cdn.jsdelivr.net/gh/google/code-prettify@master/loader/run_prettify.js'></script>");
            sb.AppendLine("</head><body>");
            sb.AppendLine($"<h1>{title}</h1>");
            sb.AppendLine(content);
            sb.AppendLine("</body></html>");
            return sb.ToString();
        }
    }
}
