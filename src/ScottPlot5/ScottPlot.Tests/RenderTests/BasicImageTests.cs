namespace ScottPlot.Tests.RenderTests;

internal class BasicImageTests
{
    [Test]
    public void Test_Render_Image()
    {
        ScottPlot.Plottables.DebugPoint MarkOrigin = new();
        MarkOrigin.Position = new(0, 0);
        MarkOrigin.Color = SKColors.Magenta;

        ScottPlot.Plottables.DebugPoint MarkPositive = new();
        MarkPositive.Position = new(5, 5);
        MarkPositive.Color = SKColors.Green;

        ScottPlot.Plottables.DebugPoint MarkRandom = new();
        MarkRandom.Position = new(-7, -4);
        MarkRandom.Color = SKColors.LightBlue;

        ScottPlot.Plot plt = new();
        plt.Add(MarkOrigin);
        plt.Add(MarkPositive);
        plt.Add(MarkRandom);

        string imagePath = Path.GetFullPath("test.png");
        plt.SaveImage(600, 400, imagePath);
        Console.WriteLine(imagePath);
    }
}
