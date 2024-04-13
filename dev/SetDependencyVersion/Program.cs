/* This application reads PackageReference elements in all csproj files in the code base
 * to evaluate version consistency. This is a manual alternative to central package management:
 * https://devblogs.microsoft.com/nuget/introducing-central-package-management/
 * https://github.com/ScottPlot/ScottPlot/issues/1311
 */

ShowPackageVersions("Avalonia");
ShowPackageVersions("SkiaSharp");

static void ShowPackageVersions(string packageName)
{
    Dictionary<string, string?> versionsByProject = GetVersionsByProject(packageName);
    HashSet<string> uniqueVersions = versionsByProject.Where(x => x.Value is not null).Select(x => x.Value!).ToHashSet();

    if (uniqueVersions.Count == 1)
    {
        Console.WriteLine($"All {versionsByProject.Count} projects have the same {packageName} version: {uniqueVersions.First()}");
    }
    else
    {
        Console.WriteLine("WARNING: multiple versions seen:");
        foreach ((string proj, string? version) in versionsByProject)
        {
            if (version is null)
                continue;
            Console.WriteLine($"{version}\t{proj}");
        }
    }
}

static Dictionary<string, string?> GetVersionsByProject(string packageName)
{
    Dictionary<string, string?> versionsByProject = new();

    foreach (string csprojFile in GetCsprojFiles())
    {
        versionsByProject[csprojFile] = GetVersion(csprojFile, packageName);
    }

    return versionsByProject;
}

static string GetAttribute(string line, string name)
{
    string[] parts = line.Split(" ");
    foreach (string part in parts)
    {
        if (!part.Contains("="))
            continue;

        if (!part.StartsWith(name + "="))
            continue;

        string value = part.Split("=")[1];
        value = value.Trim('"');
        return value;
    }

    throw new InvalidOperationException("attribute not found");
}

static string? GetVersion(string csprojFile, string packageName)
{
    List<string> versions = new();

    string[] lines = File.ReadAllLines(csprojFile);

    for (int i = 0; i < lines.Length; i++)
    {
        string line = lines[i];
        if (line.Contains("PackageReference") && line.Contains(packageName))
        {
            string version = GetAttribute(line, "Version");
            versions.Add(version);
        }
    }

    if (versions.Count == 0)
        return null;

    HashSet<string> uniqueVersions = new(versions.Where(x => x is not null));
    if (uniqueVersions.Count > 1)
    {
        Console.WriteLine($"WARNING: multiple versions found");
        Console.WriteLine($"         {csprojFile}");
        Console.WriteLine($"         {string.Join(", ", uniqueVersions)}");
    }

    return versions.First();
}

static string GetRepoFolder(string subFolder = "src")
{
    string path = AppContext.BaseDirectory;

    while (!string.IsNullOrWhiteSpace(path))
    {
        string licensePath = Path.Combine(path, "LICENSE");
        if (File.Exists(licensePath))
            break;

        path = Path.GetDirectoryName(path)!;

        if (string.IsNullOrWhiteSpace(path))
        {
            throw new FileNotFoundException("license file not located");
        }
    }

    path = Path.Join(path, subFolder);
    if (!Directory.Exists(path))
        throw new DirectoryNotFoundException(path);

    return path;
}

static string[] GetCsprojFiles(string subFolder = "src")
{
    return Directory.GetFiles(GetRepoFolder(subFolder), "*.csproj", SearchOption.AllDirectories);
}