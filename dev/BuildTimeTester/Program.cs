using System.Diagnostics;
using System.Text;

string pathSP5 = GetScottPlot5Path();

string[] mainProjectPath = [Path.Join(pathSP5, "ScottPlot5")];
string[] controlProjectPaths = Directory.GetDirectories(Path.Join(pathSP5, "ScottPlot5 Controls"));
string[] sandboxProjectPaths = Directory.GetDirectories(Path.Join(pathSP5, "ScottPlot5 Sandbox"));
string[] allPaths = [.. mainProjectPath, .. controlProjectPaths, .. sandboxProjectPaths];

StringBuilder sb = new();
sb.AppendLine("Project | Build Time (sec)");
sb.AppendLine("--- | ---");

Stopwatch sw = Stopwatch.StartNew();
foreach (string projectPath in allPaths)
{
    Console.WriteLine();
    Console.WriteLine(projectPath);
    TimeSpan elapsed = Dotnet("build --no-incremental", projectPath);
    sb.AppendLine($"{Path.GetFileName(projectPath)} | {elapsed.TotalSeconds:N2}");
}

Console.WriteLine($"\n\n{sb}\n{sw.Elapsed}\n\n");

static string GetScottPlot5Path()
{
    string path = Path.GetFullPath("./");
    for (int i = 0; i < 10; i++)
    {
        if (Directory.GetDirectories(path).Where(x => Path.GetFileName(x) == "src").Any())
            return Path.Combine(path, "src/ScottPlot5");

        path = Path.GetDirectoryName(path) ?? throw new DirectoryNotFoundException();
    }
    throw new DirectoryNotFoundException();
}

static TimeSpan Dotnet(string verb, string projectFolderPath)
{
    string command = $"""dotnet {verb} "{projectFolderPath}" """;

    ProcessStartInfo info = new()
    {
        FileName = "cmd.exe",
        Arguments = $"/C {command}",
        RedirectStandardOutput = false,
        UseShellExecute = false,
        CreateNoWindow = false,
    };

    Process process = new() { StartInfo = info };

    Stopwatch stopwatch = new();
    stopwatch.Start();
    process.Start();
    process.WaitForExit();
    stopwatch.Stop();
    return stopwatch.Elapsed;
}