namespace ScottPlotTests.RenderTests;

internal class AxisLabelTests
{
    [Test]
    public void Test_AxisLabel_Rotation()
    {
        ScottPlot.Plot plt = new();
        plt.BottomAxis.Label.Text = "Horizontal Axis";
        plt.BottomAxis.Label.Rotation = 10;
        plt.LeftAxis.Label.Text = "Vertical Axis";
        plt.LeftAxis.Label.Rotation = -90 + 10;
        plt.SaveTestImage();
    }
}
