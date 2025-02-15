using System.Collections.Generic;

namespace ScottPlotTests.CodeTests;
internal class CsprojTests
{
    readonly static Dictionary<string, string[]> ProjectFileContents = Directory
        .GetFiles(Path.Join(Paths.RepoFolder, "src/ScottPlot5/"), "*.csproj", SearchOption.AllDirectories)
        .ToDictionary(x => x, File.ReadAllLines);

    static Dictionary<string, string[]> NugetProjectFileContents => ProjectFileContents
        .Where(x => IsNugetProject(x.Key))
        .ToDictionary(x => x.Key, x => x.Value);

    static bool IsNugetProject(string csprojFilePath)
    {
        string folder = Path.GetDirectoryName(csprojFilePath)!.Replace(Paths.RepoFolder, "").Replace("\\", "/");
        if (folder == "/src/ScottPlot5/ScottPlot5")
            return true;
        if (folder.StartsWith("/src/ScottPlot5/ScottPlot5 Controls/"))
            return true;
        return false;
    }

    [Test]
    public void Test_Csproj_FilesAreFound()
    {
        ProjectFileContents.Should().NotBeEmpty();
        NugetProjectFileContents.Should().NotBeEmpty();
    }

    [Test]
    public void Test_Csproj_NoFloatingVersions()
    {
        foreach ((string file, string[] lines) in ProjectFileContents)
        {
            foreach (string line in lines)
            {
                if (!line.Contains("PackageReference"))
                    continue;

                if (line.Contains("*"))
                {
                    throw new InvalidDataException(
                        "csproj files must not contain package references with floating version numbers. \n" +
                        $"Offending file: {file} \n" +
                        $"Offending line: {line.Trim()}");
                }
            }
        }
    }

    [Test]
    public void Test_SkiaSharpVersion_IsAllSame()
    {
        Dictionary<string, string> versionsByFile = [];

        foreach ((string file, string[] lines) in ProjectFileContents)
        {
            foreach (string line in lines)
            {
                if (!line.Contains("PackageReference"))
                    continue;

                if (!line.Contains("SkiaSharp"))
                    continue;

                string version = line.Split("Version=")[1].Split('"')[1];

                if (versionsByFile.ContainsKey(file))
                {
                    if (versionsByFile[file] != version)
                        throw new InvalidDataException($"SkiaSharp has conflicting versions in {file}");
                }
                else
                {
                    versionsByFile[file] = version;
                }
            }
        }

        if (versionsByFile.Values.ToHashSet().Count > 1)
        {
            foreach ((string file, string version) in versionsByFile)
            {
                Console.WriteLine($"{version} {file}");
            }

            throw new InvalidDataException("Multiple SkiaSharp versions found!");
        }
    }

    [Test]
    public void Test_Projects_AreSigned()
    {
        foreach ((string file, string[] lines) in ProjectFileContents)
        {
            if (!lines.Where(x => x.Contains("<SignAssembly>")).Any())
                Assert.Fail($"{file} requires 'SignAssembly' to be defined. " +
                    $"See other project files in this repository for more information.");
        }
    }

    private static bool Contains(string search, string[] lines)
    {
        foreach (string line in lines)
        {
            if (line.Contains(search))
                return true;
        }
        return false;
    }

    [Test]
    public void Test_ProjectFiles_HaveMitLicenseExpression()
    {
        // https://learn.microsoft.com/en-us/nuget/create-packages/package-authoring-best-practices#licensing
        foreach ((string file, string[] lines) in NugetProjectFileContents)
        {
            if (!Contains("<PackageLicenseExpression>MIT</PackageLicenseExpression>", lines))
            {
                Assert.Fail($"MIT license expression must be in all NuGet csproj files.\n{file}");
            }
        }
    }

    [Test]
    public void Test_ProjectFiles_HaveCopyright()
    {
        // https://learn.microsoft.com/en-us/nuget/create-packages/package-authoring-best-practices#copyright
        foreach ((string file, string[] lines) in NugetProjectFileContents)
        {
            if (!Contains("<Copyright>Copyright (c) Scott Harden / Harden Technologies, LLC</Copyright>", lines))
            {
                Assert.Fail($"Copyright must be in all NuGet csproj files.\n{file}");
            }
        }
    }
}
