using System.Diagnostics;
using System.Runtime.InteropServices;

if (Debugger.IsAttached)
{
    args = [Path.GetFullPath("../../../../../src/ScottPlot5/ScottPlot5 Sandbox/Sandbox.Blazor.WebAssembly/bin/Release/net8.0/publish/wwwroot")];
}

string wwwroot = args[0];

if (!Directory.Exists(wwwroot))
    throw new DirectoryNotFoundException(wwwroot);

string indexFilePath = Path.Combine(wwwroot, "index.html");
if (!File.Exists(indexFilePath))
    throw new FileNotFoundException(indexFilePath);

string backupFilePath = Path.Combine(wwwroot, "index.html.backup");
if (!File.Exists(backupFilePath))
{
    Console.WriteLine("Creating backup...");
    File.Copy(indexFilePath, backupFilePath);
}
else
{
    Console.WriteLine("Restoring backup...");
    File.Copy(backupFilePath, indexFilePath, true);
}

string[] lines = File.ReadAllLines(backupFilePath);
for (int i = 0; i < lines.Length; i++)
{
    string line = lines[i];

    if (line.Contains("<base"))
    {
        Console.WriteLine("Fixing base URL");
        line = line.Replace("href=\"/\"", "href=\"https://swharden.com/tmp/sp/blazor/\"");

        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine(line);
        Console.ForegroundColor = ConsoleColor.Gray;
    }

    lines[i] = line;
}

string[] extensionsToDelete = ["*.gz", "*.br"];

foreach (string ext in extensionsToDelete)
{
    string[] files = Directory.GetFiles(wwwroot, ext, SearchOption.AllDirectories);
    Console.WriteLine($"Deleting {files.Length} {ext} files");
    foreach (string file in files)
    {
        File.Delete(file);
    }
}

File.WriteAllLines(indexFilePath, lines);
Console.WriteLine($"Wrote: {indexFilePath}");