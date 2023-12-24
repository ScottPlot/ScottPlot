namespace ScottPlotCookbook.Website;

internal class Generate
{
    [Test]
    public void Generate_Website()
    {
        string json = GenerateJsonFile();
        JsonCookbookInfo cb = new(json);

        GenerateHomePage(cb);
        GenerateCategoryPages(cb);
        GenerateRecipePages(cb);
        GenerateSearchPage();

        Console.WriteLine(Paths.OutputFolder);
    }

    private static string GenerateJsonFile()
    {
        string json = JsonFile.Generate();
        string jsonFile = Path.Combine(Paths.OutputFolder, "recipes.json");
        File.WriteAllText(jsonFile, json);
        return json;
    }

    private static void GenerateHomePage(JsonCookbookInfo cb)
    {
        new FrontPage(cb).Generate(Paths.OutputFolder);
    }

    private static void GenerateCategoryPages(JsonCookbookInfo cb)
    {
        foreach (var category in cb.Categories)
        {
            new CategoryPage(cb, category).Generate(Paths.OutputFolder);
        }
    }

    private static void GenerateRecipePages(JsonCookbookInfo cb)
    {
        foreach (string category in cb.Categories.Select(x => x.Name))
        {
            foreach (var recipe in cb.Recipes)
            {
                new RecipePage(recipe).Generate(Paths.OutputFolder);
            }
        }
    }

    private static void GenerateSearchPage()
    {
        new SearchPage().Generate(Paths.OutputFolder);
    }
}
