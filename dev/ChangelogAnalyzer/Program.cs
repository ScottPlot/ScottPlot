namespace ChangelogAnalyzer;

public static class Program
{
    public static void Main()
    {
        string changelogFilePath = Path.GetFullPath("../../../../../CHANGELOG.md");
        Changelog log = new(changelogFilePath);
        File.WriteAllText(changelogFilePath, log.GetMarkdown());
    }
}