namespace Cookbook_Generator;

public static class Program
{
    public static void Main()
    {
        string cookbookProjectFolder = Path.GetFullPath("../../../../../ScottPlot.Cookbook");
        string outputFolder = Path.GetFullPath("../../../../../ScottPlot.Cookbook/CookbookOutput");

        ScottPlot.Cookbook.RecipeSource[] recipes = ScottPlot.Cookbook.Generator.Generate(
            cookbookProjectFolder, outputFolder, regenerate: true);

        Website.Generate(outputFolder, recipes);

        Console.WriteLine($"COMPLETE: {outputFolder}");
    }
}
