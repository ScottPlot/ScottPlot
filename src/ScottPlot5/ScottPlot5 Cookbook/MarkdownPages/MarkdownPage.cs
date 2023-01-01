namespace ScottPlotCookbook.MarkdownPages;

internal abstract class MarkdownPage
{
    protected StringBuilder SB = new();

    public void AddVersionInformation()
    {
        SB.AppendLine();
        SB.AppendLine("<div class=\"alert alert-warning\" role=\"alert\">");
        SB.AppendLine($"<strong>⚠️ WARNING:</strong> " +
            $"This page describes <code>ScottPlot {ScottPlot.Version.VersionString}</code>, " +
            $"a preview version of ScottPlot available on NuGet. " +
            $"This package is not recommended for production use, and the API may change in future releases. " +
            $"Visit the <a href='/cookbook/4.1/'>ScottPlot 4.1 Cookbook</a> for information about the current stable version of ScottPlot.");
        SB.AppendLine("</div>");
        SB.AppendLine();
    }

    public void Save(string folder, string title, string description, string filename, string url)
    {
        // TODO: add version details

        string frontMatter = $"---\ntitle: {title}\ndescription: {description}\nurl: {url}\ndate: {DateTime.UtcNow}\n---\n\n";
        string md = frontMatter + SB.ToString();

        string saveAs = Path.Combine(folder, filename);
        File.WriteAllText(saveAs, md);
        TestContext.WriteLine(saveAs);
    }
}
