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
}
