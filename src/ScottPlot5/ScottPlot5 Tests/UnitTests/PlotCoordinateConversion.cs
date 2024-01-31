namespace ScottPlotTests.UnitTests;

internal class PlotCoordinateConversion
{
    [Test]
    public void Test_Plot_Pixel_To_Coords_And_Back()
    {
        Plot plt = new();
        plt.Axes.SetLimits(-5, 25, 10, 30);
        plt.SaveTestImage();

        Pixel initialPx = new(55.0F, 333.0F);
        Coordinates coordinates = plt.GetCoordinates(initialPx);
        Pixel convertedPx = plt.GetPixel(coordinates);

        convertedPx.X.Should().Be(initialPx.X);
        convertedPx.Y.Should().Be(initialPx.Y);
    }

    [Test]
    public void Test_Plot_Pixel_To_Coords_And_Back_Scaled()
    {
        Plot plt = new();
        plt.Axes.SetLimits(1, 2, -15, -5);
        plt.ScaleFactor = 2.5F;     // Change from default of 1.0F
        plt.SaveTestImage();

        Pixel initialPx = new(329.0F, 200.0F);
        Coordinates coordinates = plt.GetCoordinates(initialPx);
        Pixel convertedPx = plt.GetPixel(coordinates);

        convertedPx.X.Should().Be(initialPx.X);
        convertedPx.Y.Should().Be(initialPx.Y);
    }
}
