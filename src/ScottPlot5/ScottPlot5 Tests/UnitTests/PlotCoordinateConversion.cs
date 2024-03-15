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
        plt.ScaleFactor = 2.5;      // Change from default of 1.0
        plt.SaveTestImage();

        Pixel initialPx = new(329.0F, 200.0F);
        Coordinates coordinates = plt.GetCoordinates(initialPx);
        Pixel convertedPx = plt.GetPixel(coordinates);

        convertedPx.X.Should().Be(initialPx.X);
        convertedPx.Y.Should().Be(initialPx.Y);
    }

    [TestCase(1.0)]
    [TestCase(2.0)]
    [TestCase(0.5)]
    public void Test_GetCoordinateRectFromPixels(double scaleFactor)
    {
        double xCoordMin = -20;
        double xCoordMax = 20;
        double yCoordMin = -10;
        double yCoordMax = 10;
        double widthCoord = xCoordMax - xCoordMin;
        double heightCoord = yCoordMax - yCoordMin;

        double widthPx = 800;
        double heightPx = 400;

        Plot plot = new()
        {
            ScaleFactor = (float)scaleFactor
        };
        plot.Axes.SetLimits(xCoordMin, xCoordMax, yCoordMin, yCoordMax);
        plot.Layout.Frameless();

        // Render the plot
        plot.SaveTestImage((int)widthPx, (int)heightPx);

        double xPx = 200;
        double yPx = 300;
        double radius = 40;
        Pixel px = new((float)xPx, (float)yPx);

        Coordinates rightTopCoords = plot.GetCoordinates(
                (float)(xPx + radius), (float)(yPx - radius));
        Coordinates leftBotCoords = plot.GetCoordinates(
                (float)(xPx - radius), (float)(yPx + radius));

        CoordinateRect rect1 = plot.GetCoordinateRect((float)xPx, (float)yPx,
                (float)radius);
        CoordinateRect rect2 = plot.GetCoordinateRect(px, (float)radius);

        // Expected values
        double left = xCoordMin + (xPx - radius) * widthCoord / widthPx;
        double right = xCoordMin + (xPx + radius) * widthCoord / widthPx;
        double top = yCoordMax - (yPx - radius) * heightCoord / heightPx;
        double bottom = yCoordMax - (yPx + radius) * heightCoord / heightPx;

        Assert.Multiple(() =>
        {
            Assert.That(rect1.Left, Is.EqualTo(left), "rect1 left");
            Assert.That(rect1.Right, Is.EqualTo(right), "rect1 right");
            Assert.That(rect1.Top, Is.EqualTo(top), "rect1 top");
            Assert.That(rect1.Bottom, Is.EqualTo(bottom), "rect1 bottom");

            Assert.That(rect2.Left, Is.EqualTo(left), "rect2 left");
            Assert.That(rect2.Right, Is.EqualTo(right), "rect2 right");
            Assert.That(rect2.Top, Is.EqualTo(top), "rect2 top");
            Assert.That(rect2.Bottom, Is.EqualTo(bottom), "rect2 bottom");
        });
    }

    [TestCase(1.0)]
    [TestCase(2.0)]
    [TestCase(0.5)]
    public void Test_GetCoordinateRectVsGetCoordinates(double scaleFactor)
    {
        Plot plot = new()
        {
            ScaleFactor = (float)scaleFactor
        };
        plot.Axes.SetLimits(5, 10, -20, -10);

        // Render the plot
        plot.SaveTestImage(400, 600);

        float xPx = 100;
        float yPx = 50;
        float radius = 20;

        Coordinates rightTopCoords = plot.GetCoordinates(xPx + radius, yPx - radius);
        Coordinates leftBotCoords = plot.GetCoordinates(xPx - radius, yPx + radius);
        CoordinateRect rect = plot.GetCoordinateRect(xPx, yPx, radius);

        Assert.Multiple(() =>
        {
            Assert.That(rect.Left, Is.EqualTo(leftBotCoords.X), "left");
            Assert.That(rect.Right, Is.EqualTo(rightTopCoords.X), "right");
            Assert.That(rect.Top, Is.EqualTo(rightTopCoords.Y), "top");
            Assert.That(rect.Bottom, Is.EqualTo(leftBotCoords.Y), "bottom");
        });
    }

    [TestCase(1.0)]
    [TestCase(2.0)]
    [TestCase(0.5)]
    public void Test_GetCoordinateRectFromCoords(double scaleFactor)
    {
        double xCoordMin = 0;
        double xCoordMax = 10;
        double yCoordMin = 0;
        double yCoordMax = 20;
        double widthCoord = xCoordMax - xCoordMin;
        double heightCoord = yCoordMax - yCoordMin;

        double widthPx = 400;
        double heightPx = 600;

        Plot plot = new()
        {
            ScaleFactor = (float)scaleFactor
        };
        plot.Axes.SetLimits(xCoordMin, xCoordMax, yCoordMin, yCoordMax);
        plot.Layout.Frameless();

        // Render the plot
        plot.SaveTestImage((int)widthPx, (int)heightPx);

        Coordinates coords = new(7, 5);
        double radius = 10; // pixels
        double radiusXCoord = radius * widthCoord / widthPx;
        double radiusYCoord = radius * heightCoord / heightPx;

        CoordinateRect rect = plot.GetCoordinateRect(coords, (float)radius);

        Assert.Multiple(() =>
        {
            Assert.That(rect.Left, Is.EqualTo(coords.X - radiusXCoord), "left");
            Assert.That(rect.Right, Is.EqualTo(coords.X + radiusXCoord), "right");
            Assert.That(rect.Top, Is.EqualTo(coords.Y + radiusYCoord), "top");
            Assert.That(rect.Bottom, Is.EqualTo(coords.Y - radiusYCoord), "bottom");
        });
    }
}
