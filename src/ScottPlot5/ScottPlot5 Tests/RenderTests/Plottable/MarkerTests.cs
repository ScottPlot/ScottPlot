namespace ScottPlotTests.RenderTests.Plottable;

internal class MarkerTests
{
    [Test]
    public void Test_Marker_Legend()
    {
        Plot plt = new();

        var m = plt.Add.Marker(123, 456);
        m.LegendText = "test";

        plt.ShowLegend();
        plt.Legend.GetItems().Should().HaveCount(1);

        plt.SaveTestImage();
    }
}
