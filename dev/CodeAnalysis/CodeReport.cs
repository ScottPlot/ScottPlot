using CodeAnalysis.HtmlReports;
using System.Linq;
using System.Text;

namespace CodeAnalysis;

public static class CodeReport
{
    public static void Generate(string repoRootPath, string saveAs = "code-report.html")
    {
        string[] csFilePaths = Directory.GetFiles(repoRootPath, "*.cs", SearchOption.AllDirectories)
            .Where(x => !x.EndsWith("Designer.cs"))
            .Where(x => !x.Contains(Path.DirectorySeparatorChar + "obj" + Path.DirectorySeparatorChar))
            .Where(x => !x.EndsWith("AssemblyInfo.cs"))
            .OrderBy(x => x)
            .ToArray();

        Dictionary<string, int> linesByFile = csFilePaths.ToDictionary(x => x, x => File.ReadAllLines(x).Length);

        StringBuilder sb = new();

        sb.AppendLine("<h1 align='center' style='margin-bottom: 0px;'>ScottPlot Code Metrics</h1>");
        sb.AppendLine($"<div align='center'>Together, all projects contain {Total(linesByFile):N0} total lines of code</div>");
        sb.AppendLine($"<hr style='margin: 50px;' />");

        var sp4 = linesByFile.Where(x => x.Key.Contains(Path.DirectorySeparatorChar + "ScottPlot4" + Path.DirectorySeparatorChar));
        sb.AppendLine("<h2 style='margin-bottom: 0px;'>ScottPlot 4</h2>");
        sb.AppendLine($"<ul>");
        sb.AppendLine($"<li>Core Library: {Total(sp4, "ScottPlot4"):N0} lines</li>");
        sb.AppendLine($"<li>Tests: {Total(sp4, "ScottPlot.Tests"):N0} lines</li>");
        sb.AppendLine($"<li>Cookbook: {Total(sp4, "ScottPlot.Cookbook"):N0} lines</li>");
        sb.AppendLine($"<li>Demos: {Total(sp4, "ScottPlot.Demo"):N0} lines</li>");
        sb.AppendLine($"<li>All: {Total(sp4):N0}</li>");
        sb.AppendLine($"</ul>");

        var sp5 = linesByFile.Where(x => x.Key.Contains(Path.DirectorySeparatorChar + "ScottPlot5" + Path.DirectorySeparatorChar));
        sb.AppendLine("<h2 style='margin-bottom: 0px;'>ScottPlot 5</h2>");
        sb.AppendLine($"<ul>");
        sb.AppendLine($"<li>Core Library: {Total(sp5, "ScottPlot5"):N0}</li>");
        sb.AppendLine($"<li>Tests: {Total(sp5, "ScottPlot5 Tests"):N0} lines</li>");
        sb.AppendLine($"<li>Cookbook: {Total(sp5, "ScottPlot5 Cookbook"):N0} lines</li>");
        sb.AppendLine($"<li>Demos: {Total(sp5, "ScottPlot5 Demos"):N0} lines</li>");
        sb.AppendLine($"<li>All: {Total(sp5):N0}</li>");
        sb.AppendLine($"</ul>");

        var shared = linesByFile.Where(x => x.Key.Contains(Path.DirectorySeparatorChar + "Shared" + Path.DirectorySeparatorChar));
        sb.AppendLine("<h2 style='margin-bottom: 0px;'>Shared Code</h2>");
        sb.AppendLine($"<ul>");
        sb.AppendLine($"<li>All: {Total(shared):N0} lines</li>");
        sb.AppendLine($"</ul>");

        sb.AppendLine($"<div align='center' style='margin-top: 100px;'>Generated {DateTime.Now}</div>");

        /*
        foreach ((string filePath, int lineCount) in linesByFile)
            sb.AppendLine($"<div>{filePath} {lineCount}</div>");
        */

        saveAs = Path.GetFullPath(saveAs);
        string html = HtmlTemplate.WrapInPico(sb.ToString());
        Directory.CreateDirectory(Path.GetDirectoryName(saveAs)!);
        File.WriteAllText(saveAs, html);
        Console.WriteLine($"Wrote: {saveAs}");
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
}
