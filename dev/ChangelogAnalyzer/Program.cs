using Markdig;

namespace ChangelogAnalyzer;

public static class Program
{
    public static void Main()
    {
        string changelogFilePath = Path.GetFullPath("../../../../../CHANGELOG.md");
        Changelog log = new(changelogFilePath);
        File.WriteAllText(changelogFilePath, log.GetMarkdown());

        string htmlFilePath = Path.GetFullPath("changelog.html");
        File.WriteAllText(htmlFilePath, log.GetHtml());
        Console.WriteLine(htmlFilePath);
    }
}