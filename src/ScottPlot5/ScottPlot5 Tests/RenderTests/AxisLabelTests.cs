namespace ScottPlotTests.RenderTests;

internal class AxisLabelTests
{
    [Test]
    public void Test_AxisLabel_Rotation()
    {
        ScottPlot.Plot plt = new();
        plt.Axes.Bottom.Label.Text = "Horizontal Axis";
        plt.Axes.Bottom.Label.Rotation = 10;
        plt.Axes.Left.Label.Text = "Vertical Axis";
        plt.Axes.Left.Label.Rotation = -90 + 10;
        plt.SaveTestImage();
    }
}
