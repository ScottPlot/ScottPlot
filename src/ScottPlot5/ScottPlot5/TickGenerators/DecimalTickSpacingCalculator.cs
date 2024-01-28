namespace ScottPlot.TickGenerators;

public class DecimalTickSpacingCalculator
{
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

        int targetTickCount = (int)(axisLength.Length / maxLabelLength.Length) + 1;

        int radix = 10;
        int exponent = (int)Math.Log(range.Span, radix) + 1;
        double initialSpace = Math.Pow(radix, exponent);
        List<double> tickSpacings = [initialSpace];

        double[] divBy;
        if (radix == 10)
            divBy = [2, 2, 2.5]; // 10, 5, 2.5, 1
        else if (radix == 16)
            divBy = [2, 2, 2, 2]; // 16, 8, 4, 2, 1
        else
            throw new ArgumentException($"radix {radix} is not supported");

        // generate possible tick spacings
        while (tickSpacings.Count < MaximumTickCount)
        {
            double divisor = divBy[tickSpacings.Count % divBy.Length];
            double smallerSpacing = tickSpacings.Last() / divisor;
            tickSpacings.Add(smallerSpacing);
            int tickCount = (int)(range.Span / tickSpacings.Last());
            if (tickCount > targetTickCount)
                break;
        }

        // choose the densest tick spacing that is still good
        for (int i = 0; i < tickSpacings.Count; i++)
        {
            double thisTickSpacing = tickSpacings[tickSpacings.Count - 1 - i];
            double thisTickCount = range.Span / thisTickSpacing;
            double spacePerTick = axisLength.Length / thisTickCount;
            double neededSpcePerTick = maxLabelLength.Length;

            // add more space between small labels
            if (neededSpcePerTick < 10)
                neededSpcePerTick *= 2;
            else if (neededSpcePerTick < 25)
                neededSpcePerTick *= 1.5;
            else
                neededSpcePerTick *= 1.2;

            if (spacePerTick > neededSpcePerTick)
            {
                return thisTickSpacing;
            }
        }

        return tickSpacings[0];
    }
}
