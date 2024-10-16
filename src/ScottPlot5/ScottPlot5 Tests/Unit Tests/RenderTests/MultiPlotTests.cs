namespace ScottPlotTests.RenderTests;

class MultiPlotTests
{
    private static Plot MakePlot(string label)
    {
        Plot plot = new();
        plot.Add.Signal(Generate.Sin());
        plot.Add.Signal(Generate.Cos());
        var an = plot.Add.Annotation(label, Alignment.MiddleCenter);
        an.LabelFontSize = 100;
        an.LabelBackgroundColor = Colors.Transparent;
        an.LabelShadowColor = Colors.Transparent;
        an.LabelBorderColor = Colors.Transparent;
        an.LabelBold = true;
        an.LabelFontColor = Colors.Black.WithAlpha(.5);
        return plot;
    }


    [Test]
    public void Test_Multiplot_Basic()
    {
        MultiPlot mp = new();
        mp.AddSubplot(MakePlot("one"), 0, 2, 0, 1);
        mp.AddSubplot(MakePlot("two"), 1, 2, 0, 1);
        mp.SaveTestImage();
    }
}
