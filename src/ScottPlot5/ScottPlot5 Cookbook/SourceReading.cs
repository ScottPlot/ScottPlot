namespace ScottPlotCookbook;

internal static class SourceReading
{
    // TODO: this should be a source database that is built and queried 

    private static List<string> GetRecipeSourceFilePaths()
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

    private static List<RecipeInfo> GetRecipeSources()
    {
        const int indentationCharacters = 12;

        List<RecipeInfo> recipes = Query.GetRecipes();

        string recipeStartSignal = "        {";
        string recipeOverSignal = "        }";

        foreach (string path in GetRecipeSourceFilePaths())
        {
            string[] rawLines = File.ReadAllLines(path);
            string cagegoryNameSafe = string.Empty;
            string recipeNameSafe = string.Empty;
            StringBuilder sourceBeingExtended = new();
            bool InRecipe = false;

            foreach (string line in rawLines)
            {
                if (line.StartsWith("        PageName = "))
                {
                    cagegoryNameSafe = line.Split('"')[1];
                    cagegoryNameSafe = UrlTools.UrlSafe(cagegoryNameSafe);
                    continue;
                }

                if (line.StartsWith("        public override string Name ="))
                {
                    recipeNameSafe = line.Split('"')[1];
                    continue;
                }

                if (!InRecipe && line.StartsWith(recipeStartSignal))
                {
                    InRecipe = true;
                    continue;
                }

                if (InRecipe && line.StartsWith(recipeOverSignal))
                {
                    string imageFilename = $"{UrlTools.UrlSafe(recipeNameSafe)}.png";
                    string prefix = "ScottPlot.Plot myPlot = new();";
                    string suffix = $"myPlot.SavePng(\"{imageFilename}\");";
                    string sourceLines = string.Join(Environment.NewLine, sourceBeingExtended).Trim();
                    string source = $"{prefix}\n\n{sourceLines}\n\n{suffix}";

                    RecipeInfo ri = recipes
                        .Where(x => x.CategoryFolderName == cagegoryNameSafe)
                        .Where(x => x.ImageFilename == imageFilename)
                        .Single();

                    ri.AddSource(source);

                    InRecipe = false;
                    sourceBeingExtended.Clear();
                    continue;
                }

                bool lineIsWhitespace = line.Trim().Length == 0;
                if (InRecipe)
                    sourceBeingExtended.AppendLine(lineIsWhitespace ? "" : line.Substring(indentationCharacters));
            }
        }

        return recipes;
    }

    // TODO: refactor toward this
    public static List<WebRecipe> GetWebRecipes()
    {
        List<WebRecipe> recipes = new();

        Dictionary<string, Chapter> categoryChapters = Cookbook.GetCategoryPages()
            .ToDictionary(k => k.PageDetails.PageName, x => x.PageDetails.Chapter);

        foreach (RecipeInfo ri in GetRecipeSources())
        {
            string chapter = categoryChapters[ri.Category].ToString();
            WebRecipe wr = new(chapter, ri.Category, ri.Name, ri.Description, ri.SourceCode);
            recipes.Add(wr);
        }

        return recipes;
    }
}
