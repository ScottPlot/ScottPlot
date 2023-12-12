using NUnit.Framework.Internal;

namespace ScottPlotCookbook.Recipes;

#pragma warning disable IDE1006 // Ignore lowercase public variable names

/// <summary>
/// Individual recipes can inherit this so they double as <see cref="ScottPlotCookbook.Recipe"/> and
/// test cases that have a function decorated with the <see cref="Test"/> attribute.
/// </summary>
public abstract class RecipeBase : Recipe
{
    // keep this lowercase because it should be lowercase in recipe source code
    public Plot myPlot { get; private set; } = new();

    /// <summary>
    /// This function is called by code interacting with <see cref="ScottPlotCookbook.Recipe"/>
    /// </summary>
    public override void Execute(Plot plot)
    {
        myPlot = plot;
        Execute();
    }

    /// <summary>
    /// This function is called from within the test system
    /// </summary>
    [Test]
    public abstract void Execute();

    // TODO: create test to assert true for all tests
    public bool RecipeHasTestAttribute => GetType().IsDefined(typeof(Test), false);

    private RecipePageBase GetPage()
    {
        Type declaringType = GetType().DeclaringType
            ?? throw new NullReferenceException();

        RecipePageBase page = Activator.CreateInstance(declaringType) as RecipePageBase
            ?? throw new NullReferenceException();

        return page;
    }

    [TearDown]
    public void SaveRecipeImage()
    {
        string outputFolder = Path.Combine(Cookbook.OutputFolder, "images");
        string saveAs = Path.Combine(outputFolder, ImageFilename); // TODO: filename should contain category name
        Directory.CreateDirectory(outputFolder);
        myPlot.SavePng(saveAs, Cookbook.ImageWidth, Cookbook.ImageHeight);
        TestContext.WriteLine($"{saveAs}");
    }
}
