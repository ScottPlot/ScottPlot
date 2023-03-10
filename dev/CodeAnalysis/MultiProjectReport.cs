namespace CodeAnalysis;

public class MultiProjectReport
{
    public Metrics[] Metrics { get; }
    public ProjectReport[] Projects { get; }

    public MultiProjectReport(string[] metricsFilePaths)
    {
        Projects = metricsFilePaths.Select(x => new ProjectReport(x)).ToArray();
        Metrics = Projects.SelectMany(x => x.Metrics).ToArray();
    }

    public override string ToString()
    {
        return $"Report spanning {Projects.Length} projects and {Metrics.Length} analyzed types";
    }
}
