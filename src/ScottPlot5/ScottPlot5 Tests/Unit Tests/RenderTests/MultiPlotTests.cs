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
        Plot plot1 = new();
        var sig1 = plot1.Add.Signal(Generate.Sin());
        sig1.Color = Colors.Blue;
        sig1.LineWidth = 5;

        Plot plot2 = new();
        var sig2 = plot2.Add.Signal(Generate.Cos());
        sig2.Color = Colors.Red;
        sig2.LineWidth = 5;

        MultiPlot mp = new();
        mp.AddSubplot(plot1, 0, 2, 0, 1);
        mp.AddSubplot(plot2, 1, 2, 0, 1);
        mp.SaveTestImage();
    }
}
