namespace ScottPlot.TickGenerators;

public class DecimalTickSpacingCalculator
{
    public int MaximumTickCount { get; set; } = 1000;

    public double[] GenerateTickPositions(CoordinateRange range, PixelLength axisLength, PixelLength maxLabelLength)
    {
        var AbsSpan = Math.Abs(range.Span);
        var RangeMin = Math.Min(range.Min, range.Max);
        double tickSpacing = GetIdealTickSpacing(range, axisLength, maxLabelLength);

        double firstTickOffset = RangeMin % tickSpacing;
        int tickCount = (int)(AbsSpan / tickSpacing) + 2;
        tickCount = Math.Min(1000, tickCount);
        tickCount = Math.Max(1, tickCount);

        double[] majorTickPositions = Enumerable.Range(0, tickCount)
            .Select(x => RangeMin - firstTickOffset + tickSpacing * x)
            .Where(range.Contains)
            .ToArray();

        if (majorTickPositions.Length < 2)
        {
            double tickBelow = RangeMin - firstTickOffset;
            double firstTick = majorTickPositions.Length > 0 ? majorTickPositions[0] : tickBelow;
            double nextTick = tickBelow + tickSpacing;
            majorTickPositions = [firstTick, nextTick];
        }

        return majorTickPositions;
    }

    private double GetIdealTickSpacing(CoordinateRange range, PixelLength axisLength, PixelLength maxLabelLength)
    {
        var AbsSpan = Math.Abs(range.Span);

        int targetTickCount = (int)(axisLength.Length / maxLabelLength.Length) + 1;

        int radix = 10;
        int exponent = (int)Math.Log(AbsSpan, radix) + 1;
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
            int tickCount = (int)(AbsSpan / tickSpacings.Last());
            if (tickCount > targetTickCount)
                break;
        }

        // choose the densest tick spacing that is still good
        for (int i = 0; i < tickSpacings.Count; i++)
        {
            double thisTickSpacing = tickSpacings[tickSpacings.Count - 1 - i];
            double thisTickCount = AbsSpan / thisTickSpacing;
            double spacePerTick = axisLength.Length / thisTickCount;
            double neededSpacePerTick = maxLabelLength.Length;

            // add more space between small labels
            if (neededSpacePerTick < 10)
                neededSpacePerTick *= 2;
            else if (neededSpacePerTick < 25)
                neededSpacePerTick *= 1.5;
            else
                neededSpacePerTick *= 1.2;

            if (spacePerTick > neededSpacePerTick)
            {
                return thisTickSpacing;
            }
        }

        return tickSpacings[0];
    }
}
