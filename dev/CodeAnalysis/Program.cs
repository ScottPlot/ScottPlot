using CodeAnalysis;

string repoRoot = Path.GetFullPath(Path.Join(AppDomain.CurrentDomain.BaseDirectory, "../../../../../"));

/*
string sourceFolder = Path.Combine(repoRoot, "src");
if (!Directory.Exists(sourceFolder))
    throw new DirectoryNotFoundException(sourceFolder);
string[] metricsFiles = Directory.GetFiles(sourceFolder, "*.Metrics.xml", SearchOption.AllDirectories);

MultiProjectReport report = new(metricsFiles);
Console.WriteLine(report);

CodeAnalysis.HtmlReports.ProjectSummaries s = new(report);
s.Save("summaries.html");
*/

string saveAs = Path.Combine(repoRoot, "dev/www/metrics/index.html");
CodeReport.Generate(repoRoot, saveAs);