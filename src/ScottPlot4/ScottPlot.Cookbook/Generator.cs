using ScottPlot.Cookbook;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace ScottPlot.Cookbook;

public static class Generator
{
    public static RecipeSource[] Generate(string cookbookProjectFolder, string outputFolder, bool regenerate = false)
    {
        string jsonFilePath = Path.Combine(outputFolder, "recipes.json");
        string imageFolderPath = Path.Combine(outputFolder, "images");

        if (regenerate)
        {

            if (Directory.Exists(imageFolderPath))
                Directory.Delete(imageFolderPath, recursive: true);
            Directory.CreateDirectory(imageFolderPath);

            Console.WriteLine($"Generating Images: {imageFolderPath}");
            RecipeImages.Generate(imageFolderPath);

            Console.WriteLine($"Generating JSON ...");
            RecipeJson.Generate(cookbookProjectFolder, jsonFilePath);
        }

        Console.WriteLine($"Reading JSON ...");
        return RecipeJson.GetRecipes(new FileInfo(jsonFilePath)).Values.Select(x => x).ToArray();
    }
}
