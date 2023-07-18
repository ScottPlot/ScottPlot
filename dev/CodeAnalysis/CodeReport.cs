using CodeAnalysis.HtmlReports;
using System.Linq;
using System.Text;

namespace CodeAnalysis;

public static class CodeReport
{
    public static void Generate(string repoRootPath, string saveAs = "code-report.html")
    {
        ProjectMetrics metrics = new(repoRootPath);

        StringBuilder sb = new();

        sb.AppendLine("<h1 align='center' style='margin-bottom: 0px;'>ScottPlot Code Metrics</h1>");
        sb.AppendLine($"<div align='center'>This project contains {metrics.GetLines()}</div>");
        sb.AppendLine($"<hr style='margin: 50px;' />");

        AddLinesOfCodeSection(sb, metrics);
        AddTodoSection(sb, metrics, "ScottPlot4");
        AddTodoSection(sb, metrics, "ScottPlot5");

        sb.AppendLine($"<div align='center' style='margin-top: 100px;'>Generated {DateTime.Now}</div>");
        sb.AppendLine("<div align='center' style='margin-top: 1em;'>" +
            "<a href='https://github.com/ScottPlot/ScottPlot/tree/main/dev/CodeAnalysis'>" +
            "https://github.com/ScottPlot/ScottPlot/tree/main/dev/CodeAnalysis</a></div>");

        saveAs = Path.GetFullPath(saveAs);
        string html = HtmlTemplate.WrapInBootstrap(sb.ToString(), "ScottPlot Code Metrics");
        Directory.CreateDirectory(Path.GetDirectoryName(saveAs)!);
        File.WriteAllText(saveAs, html);
        Console.WriteLine($"Wrote: {saveAs}");
    }

    private static void AddLinesOfCodeSection(StringBuilder sb, ProjectMetrics metrics)
    {
        sb.AppendLine("<h2 style='margin-bottom: 0px;'>Lines of Code</h2>");

        sb.AppendLine($"<ul style='margin-left: 1em;'>");

        sb.AppendLine($"<li style='margin-top: 1em;'><b>ScottPlot 4: {metrics.GetLines("ScottPlot4")}</b></li>");
        sb.AppendLine($"<ul>");
        sb.AppendLine($"<li>Library: {metrics.GetLines("ScottPlot4", "ScottPlot")}</li>");
        sb.AppendLine($"<li>Tests: {metrics.GetLines("ScottPlot4", "ScottPlot.Tests")}</li>");
        sb.AppendLine($"<li>Cookbook: {metrics.GetLines("ScottPlot4", "ScottPlot.Cookbook")}</li>");
        sb.AppendLine($"<li>Demos: {metrics.GetLines("ScottPlot4", "ScottPlot.Demo")}</li>");
        sb.AppendLine($"</ul>");

        sb.AppendLine($"<li style='margin-top: 1em;'><b>ScottPlot 5: {metrics.GetLines("ScottPlot5")}</b></li>");
        sb.AppendLine($"<ul>");
        sb.AppendLine($"<li>Library: {metrics.GetLines("ScottPlot5", "ScottPlot5")}</li>");
        sb.AppendLine($"<li>Tests: {metrics.GetLines("ScottPlot5", "ScottPlot5 Tests")}</li>");
        sb.AppendLine($"<li>Cookbook: {metrics.GetLines("ScottPlot5", "ScottPlot5 Cookbook")}</li>");
        sb.AppendLine($"<li>Demos: {metrics.GetLines("ScottPlot5", "ScottPlot5 Demos")}</li>");
        sb.AppendLine($"</ul>");

        sb.AppendLine($"</ul>");
    }

    private static void AddTodoSection(StringBuilder sb, ProjectMetrics metrics, string folderFilter)
    {
        List<string> todos = new();
        foreach (string filePath in metrics.Files.Select(x => x.FilePath).Where(x => x.Contains(folderFilter)))
        {
            string[] lines = File.ReadAllLines(filePath);
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains("TO" + "DO:"))
                {
                    string line = lines[i].Split("TO" + "DO:")[1].Trim();

                    string relativeFilePath = filePath.Replace(metrics.FolderPath, "");

                    string fileUrl = "https://github.com/ScottPlot/ScottPlot/tree/main/"
                        + relativeFilePath.Replace("\\", "/")
                        + $"#L{i + 1}";

                    todos.Add($"<a href='{fileUrl}' style='font-family: monospace;'>" +
                        $"{Path.GetFileName(filePath)}:{i + 1}" +
                        $"</a> " +
                        $"<span style='color: green;'>{line}</a>");
                }
            }
        }
        sb.AppendLine($"<h2 style='margin-bottom: .5em;'>{folderFilter} TODOs ({todos.Count})</h2>");
        sb.AppendLine($"<ul style='margin-left: 1em;'>");
        foreach (string todo in todos)
        {
            sb.AppendLine($"<li>{todo}</li>");
        }
        sb.AppendLine($"</ul>");
    }

    private static int Total(IEnumerable<KeyValuePair<string, int>> linesByFile, string? midFolderName = null)
    {
        if (string.IsNullOrEmpty(midFolderName))
            return linesByFile.Select(x => x.Value).Sum();

        return linesByFile
            .Where(x => x.Key.Contains(Path.DirectorySeparatorChar + midFolderName + Path.DirectorySeparatorChar))
            .Select(x => x.Value)
            .Sum();
    }

    static int GetLinesOfCOde(string s) => RemoveEmptyLines(StripComments(s)).Split("\n").Length;

    static string StripComments(string s) =>
        System.Text.RegularExpressions.Regex.Replace(
            input: s,
            pattern: @"(@(?:""[^""]*"")+|""(?:[^""\n\\]+|\\.)*""|'(?:[^'\n\\]+|\\.)*')|//.*|/\*(?s:.*?)\*/",
            replacement: "$1");

    static string RemoveEmptyLines(string s) =>
        string.Join("\n", s.Split("\n").Where(x => x.Trim().Length > 0));
}
