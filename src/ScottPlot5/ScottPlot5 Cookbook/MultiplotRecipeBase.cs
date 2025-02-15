namespace ScottPlotCookbook;

#pragma warning disable IDE1006 // Ignore lowercase public variable names

public abstract class MultiplotRecipeBase
{
    public static int ImageWidth { get; set; } = 400;

    public static int ImageHeight { get; set; } = 400;
    public abstract string Name { get; }
    public abstract string Description { get; }

    // keep this lowercase because it should be lowercase in recipe source code
    public Multiplot multiplot { get; private set; } = new();

    /// <summary>
    /// This function is called from within the test system
    /// </summary>
    [Test]
    public abstract void Execute();

    [SetUp]
    public void ResetRandomNumberGenerator()
    {
        Generate.RandomData.Seed(0);
    }

    [TearDown]
    public void SaveRecipeImage()
    {
        string recipeClassName = GetType().Name;
        string imageFilename = $"{recipeClassName}.png";
        string saveAs = Path.Combine(Paths.OutputImageFolder, imageFilename);
        Directory.CreateDirectory(Paths.OutputImageFolder);
        multiplot.SavePng(saveAs, ImageWidth, ImageHeight);
        Console.WriteLine($"{saveAs}");
    }
}
