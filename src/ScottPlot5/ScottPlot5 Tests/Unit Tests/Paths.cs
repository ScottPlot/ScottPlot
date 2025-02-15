namespace ScottPlotTests;

public static class Paths
{
    public readonly static string OutputFolder = GetOutputFolder();
    public readonly static string RepoFolder = GetRepoFolder();

    private static string GetOutputFolder()
    {
        return Path.Combine(TestContext.CurrentContext.TestDirectory, "test-images");
    }

    public static string GetScottPlotXmlFilePath()
    {
        string buildFolder = Path.Combine(Paths.RepoFolder, @"src/ScottPlot5/ScottPlot5/bin");
        string[] files = Directory.GetFiles(buildFolder, "ScottPlot.xml", SearchOption.AllDirectories);
        return files.Any()
            ? files.First()
            : throw new FileNotFoundException("ScottPlot.xml not found in build folder");
    }

    private static string GetRepoFolder()
    {
        string defaultFolder = Path.GetFullPath(TestContext.CurrentContext.TestDirectory); ;
        string? repoFolder = defaultFolder;
        while (repoFolder is not null)
        {
            if (File.Exists(Path.Combine(repoFolder, "LICENSE")))
            {
                return repoFolder;
            }
            else
            {
                repoFolder = Path.GetDirectoryName(repoFolder);
            }
        }

        throw new InvalidOperationException($"repository folder not found in any folder above {defaultFolder}");
    }

    public static string[] GetTtfFilePaths()
    {
        string ttfFolder = Path.Combine(RepoFolder, @"src/ScottPlot5/ScottPlot5 Demos/ScottPlot5 WinForms Demo/Fonts");
        return Directory.GetFiles(ttfFolder, "*.ttf", SearchOption.AllDirectories);
    }
}
