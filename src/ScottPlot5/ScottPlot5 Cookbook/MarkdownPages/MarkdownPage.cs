namespace ScottPlotCookbook.MarkdownPages;

internal abstract class MarkdownPage
{
    protected StringBuilder SB = new();

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
