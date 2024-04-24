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

    [Test]
    public void Test_All_Markers()
    {
        Plot plt = new();

        MarkerShape[] markerShapes = Enum.GetValues<MarkerShape>().ToArray();
        ScottPlot.Palettes.Category20 palette = new();

        for (int i = 0; i < markerShapes.Length; i++)
        {
            var mp = plt.Add.Marker(x: 0, y: -i);
            mp.MarkerStyle.Shape = markerShapes[i];
            mp.MarkerStyle.Size = 20;
            mp.MarkerStyle.LineWidth = 2;
            mp.MarkerStyle.LineColor = palette.GetColor(i);
            mp.MarkerStyle.FillColor = palette.GetColor(i).WithAlpha(.5);

            var txt = plt.Add.Text(markerShapes[i].ToString(), 1, -i);
            txt.LabelAlignment = Alignment.MiddleLeft;
            txt.LabelFontSize = 22;
        }

        plt.Axes.SetLimitsX(-1, 5);
        plt.HideGrid();
        plt.Layout.Frameless();
        plt.SaveTestImage(400, 800);
    }
}
