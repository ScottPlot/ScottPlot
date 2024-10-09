using System.Collections.Generic;

namespace ScottPlotTests.CodeTests;
internal class CsprojTests
{
    readonly static Dictionary<string, string[]> CsprojFiles = Directory
        .GetFiles(Path.Join(Paths.RepoFolder, "src/ScottPlot5/"), "*.csproj", SearchOption.AllDirectories)
        .ToDictionary(x => x, x => File.ReadAllLines(x));

    [Test]
    public void Test_Csproj_NoFloatingVersions()
    {
        foreach ((string file, string[] lines) in CsprojFiles)
        {
            foreach (string line in lines)
            {
                if (!line.Contains("PackageReference"))
                    continue;

                if (line.Contains("*"))
                {
                    throw new InvalidDataException(
                        "csproj files must not contain pacakge references with floating version numbers. \n" +
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

        foreach ((string file, string[] lines) in CsprojFiles)
        {
            foreach (string line in lines)
            {
                if (!line.Contains("PackageReference"))
                    continue;

                if (!line.Contains("SkiaSharp"))
                    continue;

                string version = line.Split("Version=")[1].Split('"')[1];
                versionsByFile[file] = version;
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
        foreach ((string file, string[] lines) in CsprojFiles)
        {
            if (!lines.Where(x => x.Contains("<SignAssembly>")).Any())
                Assert.Fail($"{file} requires 'SignAssembly' to be defined. " +
                    $"See other project files in this repository for more information.");
        }
    }
}
