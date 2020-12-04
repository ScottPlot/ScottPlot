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

            DivStart("messageBox");
            AddHTML($"<br>- You are viewing the <a href='./'>ScottPlot {Plot.Version} Cookbook</a>");
            AddHTML($"<br>- Newer versions of ScottPlot may be available");
            AddHTML($"<br>- Additional documentation can be found on the <a href='https://swharden.com/scottplot'>ScottPlot Website</a>");
            AddHTML($"<br>- If you enjoy ScottPlot <a href='https://github.com/swharden/scottplot'>give us a star</a>!");
            DivEnd();
        }

        public static string Sanitize(string s) => s.ToLower().Replace(" ", "_").Replace(":", "");
        public static string[] Sanitize(string[] s) => s.Select(x => Sanitize(x)).ToArray();
        public void AddDiv(string html) => SB.AppendLine($"<div>{html}</div>");
        public void AddDiv(string html, string divClass) => SB.AppendLine($"<div class='{divClass}'>{html}</div>");
        public void AddHTML(string html) => SB.AppendLine(html);
        public void DivStart(string divClass) => SB.AppendLine($"<div class='{divClass}'>");
        public void DivEnd() => SB.AppendLine("</div>");
        public void UlStart() => SB.AppendLine($"<ul>");
        public void UlEnd() => SB.AppendLine("</ul>");
        public void Li(string html) => SB.AppendLine($"<li>{html}</li>");

        public void AddCode(string code)
        {
            DivStart("codeBlock");
            SB.AppendLine($"<code class='prettyprint cs'>{code}</code>");
            DivEnd();
        }

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
            string pageTitle = string.IsNullOrWhiteSpace(title) ?
                "ScottPlot Cookbook" :
                $"ScottPlot Cookbook: {title}";

            string htmlTitle = string.IsNullOrWhiteSpace(title) ?
                $"<a href='./'>ScottPlot Cookbook</a>" :
                $"<a href='./'>ScottPlot Cookbook</a><br>{title}";

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<html><head>");
            sb.AppendLine($"<title>{pageTitle}</title>");
            sb.AppendLine("<link rel='stylesheet' type='text/css' href='style.css'>");
            sb.AppendLine("<script src='https://cdn.jsdelivr.net/gh/google/code-prettify@master/loader/run_prettify.js'></script>");
            sb.AppendLine("</head><body><div class='content'>");
            sb.AppendLine($"<div class='title'>{htmlTitle}</div>");
            sb.AppendLine(content);
            sb.AppendLine("&nbsp;</div></body></html>");
            return sb.ToString();
        }
    }
}
