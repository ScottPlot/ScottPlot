using System.Text;

namespace ChangelogAnalyzer;

public class Changelog
{
    readonly List<VersionNotes> Versions = [];

    public Changelog(string filePath)
    {
        string[] lines = File.ReadAllLines(filePath);

        VersionNotes? currentVersion = null;

        foreach (string line in lines)
        {
            if (VersionNotes.IsFirstLine(line))
            {
                currentVersion = new();
                Versions.Add(currentVersion);
            }
            currentVersion?.AddLine(line);
        }
    }

    public override string ToString()
    {
        return $"Changelog with {Versions.Count} versions.";
    }

    public string GetMarkdown()
    {
        StringBuilder sb = new();
        foreach(var version in Versions)
        {
            sb.AppendLine(version.GetMarkdown());
        }
        return sb.ToString();
    }
}
