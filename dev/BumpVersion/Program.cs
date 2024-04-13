using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

/* This program increments version numbers in all ScottPlot NuGet package csproj files */
if (args.Length != 1)
    throw new ArgumentException("argument required: path to folder with a csproj file");

string srcPath = Path.GetFullPath(args[0]);

if (!Directory.Exists(srcPath))
    throw new ArgumentException($"folder does not exist: {srcPath}");

string[] csFilePaths = Directory.GetFiles(srcPath, "*.csproj");

if (!csFilePaths.Any())
    throw new ArgumentException($"folder has no csproj files: {srcPath}");

if (csFilePaths.Length > 1)
{
    csFilePaths = csFilePaths
        .Where(x => Path.GetFileName(x)
        .EndsWith("NUGET.csproj"))
        .ToArray();

    if (csFilePaths.Length == 0)
        throw new ArgumentException("more than 1 csproj file (and none ending with NUGET.csproj)");

    if (csFilePaths.Length > 1)
        throw new ArgumentException("more than 1 NUGET.csproj file");
}

if (csFilePaths.Length > 1)
    throw new ArgumentException($"folder has more than 1 csproj file: {srcPath}");

string csprojPath = csFilePaths.Single();

Console.Write($"Bumping: {Path.GetFileName(csprojPath)} ");

// get current version
csprojPath = Path.GetFullPath(csprojPath);
XDocument doc = XDocument.Load(csprojPath);
string versionString = doc.Element("Project")!.Element("PropertyGroup")!.Element("Version")!.Value;
string? previewName = versionString.Contains("-") ? versionString.Split("-")[1] : null;
versionString = versionString.Split("-")[0];
Version oldVersion = new(versionString);

// bump it
Version newVersion = new(oldVersion.Major, oldVersion.Minor, oldVersion.Build + 1);
Console.Write($"{oldVersion} -> {newVersion} ");
string newVersionString = newVersion.ToString();
string newVersionStringLong = newVersionString + ".0";
if (previewName is not null)
{
    Console.Write($"({previewName})");
    newVersionString += "-" + previewName;
    newVersionStringLong += "-" + previewName;
}

Console.WriteLine();

// save output
string[] lines = File.ReadAllLines(csprojPath);
for (int i = 0; i < lines.Length; i++)
{
    ReplaceElement(lines, "Version", newVersionString);
    ReplaceElement(lines, "AssemblyVersion", newVersionStringLong);
    ReplaceElement(lines, "FileVersion", newVersionStringLong);
}
File.WriteAllLines(csprojPath, lines);

static void ReplaceElement(string[] lines, string element, string value)
{
    for (int i = 0; i < lines.Length; i++)
    {
        if (lines[i].Contains($"<{element}>"))
        {
            string spaces = new(' ', lines[i].IndexOf("<"));
            lines[i] = $"{spaces}<{element}>{value}</{element}>";
            return;
        }
    }
}