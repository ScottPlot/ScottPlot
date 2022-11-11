using FluentAssertions;
using System.Text;

namespace ScottPlotCookbook;

internal static class SourceReading
{
    public static List<string> GetRecipeSourceFilePaths()
    {
        List<string> paths = new();

        if (!Directory.Exists(Cookbook.RecipeSourceFolder))
            throw new DirectoryNotFoundException(Cookbook.RecipeSourceFolder);

        paths.AddRange(Directory.GetFiles(Cookbook.RecipeSourceFolder, "*.cs"));

        foreach (string subFolder in Directory.GetDirectories(Cookbook.RecipeSourceFolder))
        {

            paths.AddRange(Directory.GetFiles(subFolder, "*.cs"));
        }

        if (!paths.Any())
            throw new InvalidOperationException("no source files found");

        return paths;
    }

    public static List<RecipeSource> GetRecipeSources()
    {
        List<RecipeSource> sources = new();

        string recipeStartSignal = "        {";
        string recipeOverSignal = "        }";

        foreach (string path in GetRecipeSourceFilePaths())
        {
            string[] rawLines = File.ReadAllLines(path);
            string pageNameSafe = string.Empty;
            string recipeNameSafe = string.Empty;
            StringBuilder sourceBeingExtended = new();
            bool InRecipe = false;

            foreach (string line in rawLines)
            {
                if (line.StartsWith("        PageName = "))
                {
                    pageNameSafe = UrlTools.UrlSafe(line.Split('"')[1]);
                    continue;
                }

                if (line.StartsWith("        public override string Name ="))
                {
                    recipeNameSafe = UrlTools.UrlSafe(line.Split('"')[1]);
                    continue;
                }

                if (!InRecipe && line.StartsWith(recipeStartSignal))
                {
                    InRecipe = true;
                    continue;
                }

                if (InRecipe && line.StartsWith(recipeOverSignal))
                {
                    string source = string.Join(Environment.NewLine, sourceBeingExtended);
                    sources.Add(new RecipeSource(pageNameSafe, recipeNameSafe, source));
                    InRecipe = false;
                    sourceBeingExtended.Clear();
                    continue;
                }

                bool lineIsWhitespace = line.Trim().Length == 0;
                if (InRecipe)
                    sourceBeingExtended.AppendLine(lineIsWhitespace ? "" : line.Substring(8));
            }
        }

        return sources;
    }

    [Test]
    public static void Test_Recipe_Sources_Found()
    {
        List<RecipeSource> sources = GetRecipeSources();

        sources.Should().NotBeEmpty();
        sources.Should().HaveCount(Cookbook.GetRecipes().Count);

        foreach (RecipeSource source in sources)
            TestContext.WriteLine($"{source.PageName}/{source.RecipeName} ({source.SourceCode.Length} characters)");
    }
}
