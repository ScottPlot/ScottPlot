using CodeAnalysis;

string repoRoot = Path.GetFullPath(Path.Join(AppDomain.CurrentDomain.BaseDirectory, "../../../../../"));

string saveAs = Path.Combine(repoRoot, "dev/www/metrics/index.html");
CodeReport.Generate(repoRoot, saveAs);