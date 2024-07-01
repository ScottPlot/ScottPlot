namespace ScottPlotTests;

public static class Paths
{
    public readonly static string OutputFolder = GetOutputFolder();
    public readonly static string RepoFolder = GetRepoFolder();

    private static string GetOutputFolder()
    {
        return Path.Combine(TestContext.CurrentContext.TestDirectory, "test-images");
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
}
