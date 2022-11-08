namespace ScottPlot_Tests.Cookbook.Recipes;

internal class QuickstartRecipes
{
    [Test]
    public void Quickstart()
    {
        Console.WriteLine("works");

        Plot plt = new();
        plt.SavePNG("test.jpg");
    }
}
