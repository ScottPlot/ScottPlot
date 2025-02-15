namespace ScottPlotTests.UnitTests.TickGenerators;

internal class DecimalTickSpacingCalculatorTests
{
    [Test]
    public void Test_CalculatorLabels_ShouldAlwaysFitInGivenSpace()
    {
        ScottPlot.TickGenerators.DecimalTickSpacingCalculator calc = new();
        CoordinateRange range = new(-500_000_000, 500_000_000);
        PixelLength axisLength = new(500);

        for (int i = 10; i < 100; i += 10)
        {
            PixelLength maxLabelLength = new(i);
            double[] positions = calc.GenerateTickPositions(range, axisLength, maxLabelLength);
            double spacePerLabel = axisLength.Length / positions.Length;
            Console.WriteLine($"when labels are {maxLabelLength}, each {spacePerLabel} px of space");
            spacePerLabel.Should().BeGreaterThan(maxLabelLength.Length);
        }
    }

    [Test]
    public void GenerateTickPositions__SmallAxisSize_2DifferentTickPostitions()
    {
        ScottPlot.TickGenerators.DecimalTickSpacingCalculator calc = new();
        CoordinateRange range = new(1, 5);
        PixelLength axisLength = new(50);
        PixelLength maxLabelLength = 30;

        double[] positions = calc.GenerateTickPositions(range, axisLength, maxLabelLength);

        Assert.That(positions.Length, Is.EqualTo(2));
        Assert.That(positions[0], Is.Not.EqualTo(positions[1]));
    }
}
