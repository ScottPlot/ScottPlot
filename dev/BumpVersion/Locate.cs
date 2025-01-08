internal static class Locate
{
    public static string GetRepoRootFolder()
    {
        string? path = Path.GetFullPath("./");

        while (path is not null)
        {
            string licenseFilePath = Path.Combine(path, "LICENSE");
            if (File.Exists(licenseFilePath))
            {
                Console.WriteLine($"Repository root: {path}");
                return path;
            }
            path = Path.GetDirectoryName(path);
        }

        throw new DirectoryNotFoundException("repository root not located / no license file found");
    }

    public static string[] GetCsprojFilePaths(string repoFolder)
    {
        List<string> csprojPaths = [];

        string coreFolder = Path.Combine(repoFolder, "src/ScottPlot5/ScottPlot5");
        csprojPaths.AddRange(Directory.GetFiles(coreFolder, "*.csproj"));

        foreach (string controlFolder in Directory.GetDirectories(Path.Combine(repoFolder, "src/ScottPlot5/ScottPlot5 Controls")))
        {
            csprojPaths.AddRange(Directory.GetFiles(controlFolder, "*.csproj"));
        }

        return [.. csprojPaths];
    }
}
