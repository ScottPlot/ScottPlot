using System;
using System.IO;
using System.Linq;

namespace ScottPlot.Cookbook;

public static class Generator
{
    public static void ExecuteAllRecipesAndGenerateWebsite(string cookbookProjectFolder)
    {
        Console.WriteLine("Deleting old cookbook files...");
        string outputFolder = Path.Combine(cookbookProjectFolder, "CookbookOutput");
        if (Directory.Exists(outputFolder))
            Directory.Delete(outputFolder, recursive: true);
        Directory.CreateDirectory(outputFolder);

        string jsonFilePath = Path.Combine(outputFolder, "recipes.json");
        string imageFolderPath = Path.Combine(outputFolder, "images");
        Directory.CreateDirectory(imageFolderPath);

        Console.WriteLine($"Generating Images...");
        RecipeImages.Generate(imageFolderPath);

        Console.WriteLine($"Generating JSON...");
        string json = RecipeJson.Generate(cookbookProjectFolder);
        File.WriteAllText(jsonFilePath, json);

        Console.WriteLine($"Reading JSON...");
        RecipeSource[] recipes = RecipeJson.GetRecipes(jsonFilePath).Values.Select(x => x).ToArray();

        Console.WriteLine($"Generating website...");
        Website.Generate(outputFolder, recipes);

        Console.WriteLine($"SAVED IN: {outputFolder}");
    }
}
