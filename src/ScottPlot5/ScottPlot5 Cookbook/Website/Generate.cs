namespace ScottPlotCookbook.Website;

internal class Generate
{
    [Test]
    public void Generate_Website()
    {
        // generate a JSON file containing all cookbook info built using reflection.
        string json = JsonFile.Generate();

        // Save the json file to disk so it can be used by the demo application to load source code
        string jsonFile = Path.Combine(Paths.OutputFolder, "recipes.json");
        File.WriteAllText(jsonFile, json);

        // Use contents of the JSON file to learn about all recipes and build the whole website
        JsonCookbookInfo cb = new(json);
        GenerateHomePage(cb);
        GenerateCategoryPages(cb);
        GenerateRecipePages(cb);

        // generate flat pages
        SearchPage.Generate(Paths.OutputFolder);
        PalettesPage.Generate(Paths.OutputFolder);
        ColormapsPage.Generate(Paths.OutputFolder);
        ColorsPage.Generate(Paths.OutputFolder);

        Console.WriteLine(Paths.OutputFolder);
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
}
