using NUnit.Framework.Internal;
using ScottPlotCookbook.HtmlPages;

namespace ScottPlotCookbook;

/// <summary>
/// Individual recipes can inherit this so they double as <see cref="IRecipe"/> and
/// test cases that have a function decorated with the <see cref="Test"/> attribute.
/// </summary>
public abstract class RecipeTestBase : IRecipe
{
    public Plot MyPlot { get; private set; } = new();
    private int Width = 400;
    private int Height = 300;

    public abstract string Name { get; }
    public abstract string Description { get; }

    /// <summary>
    /// This function is called by code interacting with <see cref="IRecipe"/>
    /// </summary>
    public void Recipe(Plot plot)
    {
        MyPlot = plot;
        Recipe();
    }

    /// <summary>
    /// This function is called from within the test system
    /// </summary>
    [Test]
    public abstract void Recipe();

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
        string fileUrl = UrlTools.GetImageUrl(GetPage(), this);
        string saveAs = Path.Combine(Cookbook.OutputFolder, fileUrl);
        MyPlot.SavePng(saveAs, Width, Height);
        TestContext.WriteLine($"{saveAs}");
    }
}
