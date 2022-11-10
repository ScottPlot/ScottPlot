using System.Text;

namespace ScottPlotCookbook.Generation;

internal class MarkdownPage
{
    private readonly StringBuilder SB = new();

    public void AppendLine(string text)
    {
        SB.AppendLine(text);
    }

    public void AddParagraph(string text)
    {
        SB.AppendLine("\n" + text + "\n");
    }

    public void AddHeading(string text, int level)
    {
        SB.AppendLine("\n" + new string('#', level) + text + "\n");
    }

    public void AddImage(string url)
    {
        SB.AppendLine($"\n![]({url})\n");
    }

    public void Export()
    {
        string saveAs = Path.Combine(Output.OutputFolder, "index.md");
        File.WriteAllText(saveAs, SB.ToString());
        TestContext.WriteLine(saveAs);
    }
}
