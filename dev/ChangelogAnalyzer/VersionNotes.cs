using System.Text;

namespace ChangelogAnalyzer;

public class VersionNotes
{
    List<string> TitleLines = [];
    List<string> PublishedLines = [];
    List<string> FeatureLines = [];

    public static bool IsFirstLine(string line)
    {
        return line.StartsWith('#');
    }

    public void AddLine(string line)
    {
        if (line.Trim().Length == 0)
            return;

        if (line.StartsWith('#'))
        {
            AddTitleLine(line);
        }
        else if (line.Contains("Published on "))
        {
            AddPublishedLine(line);
        }
        else
        {
            AddFeatureLine(line);
        }
    }

    private void AddPublishedLine(string line)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(line);
        PublishedLines.Add(line);
    }

    private void AddTitleLine(string line)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(line);
        TitleLines.Add(line);
    }

    private void AddFeatureLine(string line)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(line);
        FeatureLines.Add(line);
    }

    public string GetMarkdown()
    {
        if (TitleLines.Count != 1)
            throw new InvalidOperationException("must have one title line");

        if (PublishedLines.Count > 1)
            throw new InvalidOperationException("must have one published line");

        if (FeatureLines.Count == 0)
            throw new InvalidOperationException("must have at least one feature line");

        StringBuilder sb = new();
        sb.AppendLine(TitleLines.First());

        if (PublishedLines.Count > 0)
        {
            sb.AppendLine(PublishedLines.First());
        }

        foreach (var featureLine in FeatureLines)
        {
            sb.AppendLine(featureLine);
        }

        return sb.ToString();
    }
}
