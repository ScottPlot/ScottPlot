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
        if (folder.StartsWith("/src/ScottPlot5/ScottPlot5 Tests/"))
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

    [Test]
    public void Test_ProjectTargetFrameworks_ShouldNotIncludeOutOfSupportVersions()
    {
        // Extend this collection as code supporting new versions is added to this repo.
        // Beware that bleeding edge .NET versions may not be fully supported by GitHub Actions,
        // so tests could pass locally but fail when you go to create a PR.
        HashSet<string> permittedVersions = [
            "net462",
            "netstandard2.0",
            "net8.0",
            "net9.0",
        ];

        Dictionary<string, List<string>> projectsByVersion = [];

        foreach ((string file, string[] lines) in ProjectFileContents)
        {
            foreach (string line in lines)
            {
                if (line.Contains("<TargetFramework"))
                {
                    string[] versions = line
                        .Split(">")[1]
                        .Split('<')[0]
                        .Split(';')
                        .Select(x => x.Split('-')[0])
                        .ToArray();

                    foreach (var version in versions)
                    {
                        if (version.StartsWith('$'))
                            continue;
                        if (!projectsByVersion.ContainsKey(version))
                            projectsByVersion[version] = [];
                        projectsByVersion[version].Add(file);
                    }
                }
            }
        }

        foreach (var version in projectsByVersion.Keys)
        {
            Console.WriteLine($"Version {version}:");
            foreach (var file in projectsByVersion[version])
            {
                Console.WriteLine($"  {Path.GetFileName(file)}");
            }
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

    [Test]
    public void Test_FluentAssertions_PinnedVersion()
    {
        // https://github.com/ScottPlot/ScottPlot/issues/4705

        string packageName = "FluentAssertions";
        string packageVersion = "[6.12.0]";

        foreach ((string file, string[] lines) in NugetProjectFileContents)
        {
            foreach (string line in lines)
            {
                if (line.Contains(packageName))
                {
                    Console.WriteLine(line);
                    if (!line.Contains(packageVersion))
                    {
                        Assert.Fail($"Package '{packageName}' version must be {packageVersion} in {file}");
                    }
                }
            }
        }
    }
}
