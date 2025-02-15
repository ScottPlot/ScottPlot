namespace ScottPlotCookbook.Website;

internal abstract class PageBase
{
    protected StringBuilder SB = new();

    public void Save(string folder, string title, string description, string filename, string url, string[]? frontmatter)
    {
        Directory.CreateDirectory(folder);

        if (!url.EndsWith("/"))
            url += "/";

        StringBuilder sbfm = new();
        sbfm.AppendLine("---");
        sbfm.AppendLine($"Title: {title}");
        sbfm.AppendLine($"Description: {description}");
        sbfm.AppendLine($"URL: {url}");
        if (frontmatter is not null)
        {
            foreach (string line in frontmatter)
            {
                sbfm.AppendLine(line);
            }
        }
        sbfm.AppendLine($"Date: {DateTime.UtcNow:yyyy-MM-dd}");
        sbfm.AppendLine($"Version: {ScottPlot.Version.LongString}");
        sbfm.AppendLine($"Version: {ScottPlot.Version.LongString}");
        sbfm.AppendLine($"SearchUrl: \"/cookbook/5.0/search/\"");
        sbfm.AppendLine($"ShowEditLink: false");
        sbfm.AppendLine("---");
        sbfm.AppendLine();

        string md = sbfm.ToString() + SB.ToString();

        string saveAs = Path.Combine(folder, filename);
        File.WriteAllText(saveAs, md);
    }
}
