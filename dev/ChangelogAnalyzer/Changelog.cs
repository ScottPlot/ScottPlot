using Markdig;
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

    public string GetHtml()
    {
        string template =
            """
            <!doctype html>
            <html lang="en">
              <head>
                <meta charset="utf-8">
                <meta name="viewport" content="width=device-width, initial-scale=1">
                <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-QWTKZyjpPEjISv5WaRU9OFeRpok6YctnYmDr5pNlyT2bRjXh0JMhjY6hW+ALEwIH" crossorigin="anonymous">
              </head>
              <body>
                <div class="container my-5">
                  {{BODY}}
                </div>
              </body>
            </html>
            """;

        string html = Markdown.ToHtml(GetMarkdown());

        return template.Replace("{{BODY}}", html);
    }
}
