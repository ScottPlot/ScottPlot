using System;
using System.IO;
using System.Linq;

namespace ScottPlot.Cookbook;

public static class Generator
{
    public static void GenerateImagesAndJson(string cookbookProjectFolder)
    {
        string outputFolder = Path.Combine(cookbookProjectFolder, "CookbookOutput");

        Console.WriteLine("Deleting old cookbook files...");
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

        Console.WriteLine($"IMAGES SAVED IN: {outputFolder}");
    }

    public static void GenerateWebsite(string cookbookProjectFolder)
    {
        string outputFolder = Path.Combine(cookbookProjectFolder, "CookbookOutput");
        string jsonFilePath = Path.Combine(outputFolder, "recipes.json");

        Console.WriteLine($"Reading JSON...");
        RecipeSource[] recipes = RecipeJson.GetRecipes(jsonFilePath).Values.Select(x => x).ToArray();

        Console.WriteLine($"Generating website...");
        Website.Generate(outputFolder, recipes);

        Console.WriteLine($"WEBSITE SAVED IN: {outputFolder}");
    }
}
