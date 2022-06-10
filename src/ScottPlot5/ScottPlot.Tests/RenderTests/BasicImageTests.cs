namespace ScottPlot.Tests.RenderTests;

internal class BasicImageTests
{
    [Test]
    public void Test_Render_Image()
    {
        Plot plt = new();
        
        plt.Add(new Plottables.DebugGrid());
        plt.Add(new Plottables.DebugPoint(2, 3, SKColors.Magenta));
        plt.Add(new Plottables.DebugPoint(-7, -4, SKColors.LightGreen));

        TestTools.SaveImage(plt);
    }

    [Test]
    public void Test_Render_MousePan()
    {
        Plot plt = new();

        plt.Add(new Plottables.DebugGrid());
        plt.Add(new Plottables.DebugPoint(2, 3, SKColors.Magenta));
        plt.Add(new Plottables.DebugPoint(-7, -4, SKColors.LightGreen));

        CoordinateRect limits = plt.GetAxisLimits();
        TestTools.SaveImage(plt, subName: "1");

        plt.SetAxisLimits(limits.WithPan(2, 3));
        TestTools.SaveImage(plt, subName: "2");
    }
}
