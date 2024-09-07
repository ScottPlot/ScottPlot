using System.Diagnostics;
using System.Text;

string pathSP5 = Path.GetFullPath("../../../../../src/ScottPlot5");

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