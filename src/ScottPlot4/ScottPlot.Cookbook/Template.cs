using System;
using System.IO;
using System.Text;

namespace ScottPlot.Cookbook;

internal static class Template
{
    public static void CreateMarkdownPage(string mdFilePath, string body, string title, string description, string url = "", string[] frontmatter = null)
    {
        StringBuilder sb = new();
        sb.AppendLine("---");
        sb.AppendLine($"Title: \"{title}\"");
        sb.AppendLine($"Description: \"{description}\"");
        sb.AppendLine($"Date: {DateTime.Now:yyyy-MM-dd}");
        sb.AppendLine($"Version: {ScottPlot.Version.LongString}");
        if (!string.IsNullOrEmpty(url))
            sb.AppendLine($"URL: {url}");
        if (frontmatter is not null)
        {
            foreach (string s in frontmatter)
            {
                sb.AppendLine(s);
            }
        }
        sb.AppendLine("---");
        sb.AppendLine("");
        sb.AppendLine(body);

        mdFilePath = Path.GetFullPath(mdFilePath);
        File.WriteAllText(mdFilePath, sb.ToString());
    }
}
