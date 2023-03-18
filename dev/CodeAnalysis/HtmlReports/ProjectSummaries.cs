using System.Text;

namespace CodeAnalysis.HtmlReports;

public class ProjectSummaries
{
    MultiProjectReport Report { get; }

    public ProjectSummaries(MultiProjectReport report)
    {
        Report = report;
    }

    private string GetContent()
    {
        StringBuilder sb = new();

        sb.AppendLine("<h1>ScottPlot Code Metrics</h1>");

        sb.AppendLine("<h3>All Projects</h3>");

        TableBuilder tb = new();

        tb.AddHeader(new string[] {
            "Project",
            "Lines of Code",
            "Types",
            "Cyclomatic Complexity" });

        foreach (ProjectReport project in Report.Projects)
        {
            tb.AddRow(new string[] {
                project.ProjectName,
                $"{project.LinesOfCode:N0}",
                $"{project.Types:N0}",
                $"{project.CyclomaticComplexity:N0}" });
        }

        sb.AppendLine(tb.GetHtml());

        return sb.ToString();
    }

    public void Save(string htmlFilePath)
    {
        string html = HtmlTemplate.WrapInPico(GetContent(), "ScottPlot Code Metrics");
        string saveAs = Path.GetFullPath(htmlFilePath);
        File.WriteAllText(saveAs, html);
        Console.WriteLine($"Saved: {saveAs}");
    }
}
