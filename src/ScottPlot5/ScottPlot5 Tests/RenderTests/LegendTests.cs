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

        plt.Legend(true);
        plt.SaveTestImage(300, 200, "legend-enabled");

        plt.Legend(false);
        plt.SaveTestImage(300, 200, "legend-disabled");
    }
}
