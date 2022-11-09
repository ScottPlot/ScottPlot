using NUnit.Framework.Internal;
using SkiaSharp;

namespace ScottPlot_Tests.Cookbook;

public abstract class RecipeBase : IRecipe
{
    public Plot myPlot = new();
    public int Width = 400;
    public int Height = 300;

    public abstract string Title { get; }
    public abstract string Description { get; }
    public abstract RecipeCategory Category { get; }

    [TearDown]
    public void TearDown()
    {
        SaveRecipeImage();
    }

    [Test]
    public abstract void Recipe();

    public string GetTestFolder()
    {
        string path = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "cookbook"));
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        return path;
    }

    public void SaveRecipeImage()
    {
        System.Diagnostics.StackTrace stackTrace = new();
        string callingMethod = stackTrace.GetFrame(1)!.GetMethod()!.Name;
        string fileName = $"{callingMethod}.png";
        string filePath = Path.Combine(GetTestFolder(), fileName);
        myPlot.SavePng(filePath, Width, Height);
        TestContext.WriteLine($"{filePath}");
    }
}
