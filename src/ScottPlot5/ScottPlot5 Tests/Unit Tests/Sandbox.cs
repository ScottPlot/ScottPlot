namespace ScottPlotTests;

internal class Sandbox
{
    [Test]
    public void Test_Sandbox()
    {
        Plot plt = new();
        plt.Axes.Left.Label.Text = "Vertical Axis";
        plt.Axes.Left.MinimumSize = 100;
        plt.SaveTestImage();
    }
}
