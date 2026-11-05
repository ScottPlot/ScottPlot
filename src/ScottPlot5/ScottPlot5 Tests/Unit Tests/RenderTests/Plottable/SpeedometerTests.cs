namespace ScottPlotTests.RenderTests.Plottable;

internal class SpeedometerTests
{
    [Test]
    public void Test_Speedometer_Render()
    {
        Plot plt = new();
        var speedometer = plt.Add.Speedometer(72);
        speedometer.Label = "km/h";
        plt.SaveTestImage();
    }

    [Test]
    public void Test_Speedometer_EdgeCases_RenderWithoutThrowing()
    {
        Plot plt = new();

        var noValue = plt.Add.Speedometer(double.NaN);
        noValue.FractionRect = FractionRect.Column(0, 4);

        var atRest = plt.Add.Speedometer(0, min: 0, max: 0);
        atRest.FractionRect = FractionRect.Column(1, 4);

        var noColors = plt.Add.Speedometer(5, min: 0, max: 10);
        noColors.Colors = [];
        noColors.FractionRect = FractionRect.Column(2, 4);

        var noBar = plt.Add.Speedometer(5, min: 0, max: 10);
        noBar.ShowValueBar = false;
        noBar.Hatch = new ScottPlot.Hatches.Glitter();
        noBar.FractionRect = FractionRect.Column(3, 4);

        plt.Should().RenderInMemoryWithoutThrowing();
        plt.Should().RenderInMemoryWithoutThrowing(40, 30);
    }

    [Test]
    public void Test_Speedometer_ExpandToFit_EndsOnRoundNumbers()
    {
        Plot plt = new();
        var speedometer = plt.Add.Speedometer(0, min: 0, max: 0);
        speedometer.ExpandToFitValue = true;

        speedometer.Value = 279_123;
        Assert.That(speedometer.Max, Is.EqualTo(300_000));

        speedometer.Value = 100;
        Assert.That(speedometer.Max, Is.EqualTo(300_000), "the scale never shrinks");

        speedometer.Value = 5_400;
        Assert.That(speedometer.Max, Is.EqualTo(300_000));

        Plot plt2 = new();
        var small = plt2.Add.Speedometer(0, min: 0, max: 0);
        small.ExpandToFitValue = true;
        small.Value = 5_400;
        Assert.Multiple(() =>
        {
            Assert.That(small.Max, Is.EqualTo(7_500));
            Assert.That(small.Min, Is.EqualTo(0));
        });
    }

    [Test]
    public void Test_Speedometer_ExpandToFit_DisabledByDefault()
    {
        Plot plt = new();
        var speedometer = plt.Add.Speedometer(0, min: 0, max: 100);
        speedometer.Value = 250;
        Assert.That(speedometer.Max, Is.EqualTo(100));
    }
}
