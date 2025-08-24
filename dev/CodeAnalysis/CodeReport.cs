﻿using System.Text;

using CodeAnalysis.HtmlReports;

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
}