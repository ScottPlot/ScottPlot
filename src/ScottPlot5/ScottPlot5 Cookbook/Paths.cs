namespace ScottPlotCookbook;

public static class Paths
{
    public static readonly string OutputFolder = Path.Combine(RepoFolder, "dev/www/cookbook/5.0");
    public static readonly string OutputImageFolder = Path.Combine(RepoFolder, "dev/www/cookbook/5.0/images");
    public static readonly string RecipeSourceFolder = Path.Combine(RepoFolder, "src/ScottPlot5/ScottPlot5 Cookbook/Recipes");

    private static string RepoFolder
    {
        get
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
}
