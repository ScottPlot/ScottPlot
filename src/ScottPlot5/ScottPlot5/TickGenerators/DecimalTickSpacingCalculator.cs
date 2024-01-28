namespace ScottPlot.TickGenerators;

public class DecimalTickSpacingCalculator
{
    public double TickDensity { get; set; } = 1.0;
    public int MaximumTickCount { get; set; } = 1000;

    public double[] GenerateTickPositions(CoordinateRange range, PixelLength axisLength, PixelLength maxLabelLength)
    {
        double tickSpacing = GetIdealTickSpacing(range, axisLength, maxLabelLength);

        double firstTickOffset = range.Min % tickSpacing;
        int tickCount = (int)(range.Span / tickSpacing) + 2;
        tickCount = Math.Min(1000, tickCount);
        tickCount = Math.Max(1, tickCount);

        double[] majorTickPositions = Enumerable.Range(0, tickCount)
            .Select(x => range.Min - firstTickOffset + tickSpacing * x)
            .Where(range.Contains)
            .ToArray();

        if (majorTickPositions.Length < 2)
        {
            double tickBelow = range.Min - firstTickOffset;
            double firstTick = majorTickPositions.Length > 0 ? majorTickPositions[0] : tickBelow;
            double nextTick = tickBelow + tickSpacing;
            majorTickPositions = [firstTick, nextTick];
        }

        return majorTickPositions;
    }

    private double GetIdealTickSpacing(CoordinateRange range, PixelLength axisLength, PixelLength maxLabelLength)
    {
        double unitsPerPx = range.Span / axisLength.Length;

        int targetTickCount = (int)(axisLength.Length / maxLabelLength.Length * TickDensity);

        int radix = 10;
        int exponent = (int)Math.Log(range.Span, radix);
        double initialSpace = Math.Pow(radix, exponent);
        List<double> tickSpacings = [initialSpace, initialSpace, initialSpace];

        double[] divBy;
        if (radix == 10)
            divBy = [2, 2, 2.5]; // 10, 5, 2.5, 1
        else if (radix == 16)
            divBy = [2, 2, 2, 2]; // 16, 8, 4, 2, 1
        else
            throw new ArgumentException($"radix {radix} is not supported");

        // TODO: this needs to be better at determining ideal density
        // https://github.com/ScottPlot/ScottPlot/issues/3203
        int divisions = 0;
        int tickCount = 0;
        while (tickCount < targetTickCount && tickSpacings.Count < MaximumTickCount)
        {
            tickSpacings.Add(tickSpacings.Last() / divBy[divisions++ % divBy.Length]);
            tickCount = (int)(range.Span / tickSpacings.Last());
        }

        return tickSpacings[tickSpacings.Count - 3];
    }
}
