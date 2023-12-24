namespace ScottPlotTests.RenderTests;

internal class LegendTests
{
    [Test]
    public void Test_Legend_Toggle()
    {
        Plot plt = new();
        var sig1 = plt.Add.Signal(Generate.Sin());
        var sig2 = plt.Add.Signal(Generate.Cos());

        sig1.Label = "Sine";
        sig2.Label = "Cosine";

        plt.SaveTestImage(300, 200, "legend-default");

        plt.Legend.IsVisible = true;
        plt.SaveTestImage(300, 200, "legend-enabled");

        plt.Legend.IsVisible = false;
        plt.SaveTestImage(300, 200, "legend-disabled");
    }

    [Test]
    public void Test_Legend_FontStyle()
    {
        Plot plt = new();

        var sig1 = plt.Add.Signal(Generate.Sin());
        var sig2 = plt.Add.Signal(Generate.Cos());

        sig1.Label = "Sine";
        sig2.Label = "Cosine";

        plt.Legend.IsVisible = true;
        plt.Legend.Font.Size = 26;
        plt.Legend.Font.Color = Colors.Magenta;

        plt.SaveTestImage();
    }

    [Test]
    public void Test_Legend_Image()
    {
        Plot plt = new();

        var sig1 = plt.Add.Signal(Generate.Sin());
        var sig2 = plt.Add.Signal(Generate.Cos());

        sig1.Label = "Sine";
        sig2.Label = "Cosine";

        //Image img = plt.GetLegendImage();
        //img.SaveTestImage();
    }

    [Test]
    public void Test_Legend_SvgImage()
    {
        Plot plt = new();

        var sig1 = plt.Add.Signal(Generate.Sin());
        var sig2 = plt.Add.Signal(Generate.Cos());

        sig1.Label = "Sine";
        sig2.Label = "Cosine";

        plt.Legend.IsVisible = true;

        string svgXml = plt.GetLegendSvgXml();
        svgXml.SaveTestString(".svg");
    }
}
