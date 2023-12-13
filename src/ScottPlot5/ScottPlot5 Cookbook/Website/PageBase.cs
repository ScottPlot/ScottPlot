namespace ScottPlotCookbook.Website;

internal abstract class PageBase
{
    protected StringBuilder SB = new();

    public void AddVersionInformation()
    {
        string alertHtml = "\n\n" +
            "<div class='alert alert-warning' role='alert'>" +
            "<h4 class='alert-heading py-0 my-0'>" +
            $"⚠️ ScottPlot {ScottPlot.Version.VersionString} is a preview package" +
            "</h4>" +
            "<hr />" +
            "<p class='mb-0'>" +
            "<span class='fw-semibold'>This page describes a beta release of ScottPlot.</span> " +
            "It is available on NuGet as a preview package, but its API is not stable " +
            "and it is not recommended for production use. " +
            "See the <a href='https://scottplot.net/versions/'>ScottPlot Versions</a> page for more information. " +
            "</p>" +
            "</div>" +
            "\n\n";

        SB.AppendLine(alertHtml);
    }

    public void Save(string folder, string title, string description, string filename, string url, string[]? frontmatter)
    {
        Directory.CreateDirectory(folder);

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
        sbfm.AppendLine($"Date: {DateTime.UtcNow}");
        sbfm.AppendLine($"Version: {ScottPlot.Version.LongString}");
        sbfm.AppendLine("---");
        sbfm.AppendLine();

        string md = sbfm.ToString() + SB.ToString();

        string saveAs = Path.Combine(folder, filename);
        File.WriteAllText(saveAs, md);
    }
}
