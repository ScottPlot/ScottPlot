namespace ScottPlotTests.RenderTests;

class MultiPlotTests
{
    [Test]
    public void Test_Multiplot_Default()
    {
        Multiplot mp = new();

        for (int i = 0; i < 3; i++)
        {
            Plot plot = new();
            var sig = plot.Add.Signal(Generate.Sin());
            sig.Color = Colormap.Default.GetColor(i, 3);
            sig.LineWidth = 5;
            mp.AddPlot(plot);
        }

        mp.SaveTestImage();
    }
}
