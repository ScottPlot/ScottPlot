using System.Globalization;

namespace ScottPlot.TickGenerators.ScottPlot4;

internal class NumericTickGenerator : ITickGenerator
{
    private readonly bool IsVertical;

    public Tick[] Ticks { get; set; } = Array.Empty<Tick>();

    public int MaxTickCount { get; set; } = 10_000;

    public IEnumerable<Tick> GetVisibleTicks(CoordinateRange range)
    {
        return Ticks.Where(x => range.Contains(x.Position));
    }

    public NumericTickGenerator(bool isVertical)
    {
        IsVertical = isVertical;
    }

    public void Regenerate(CoordinateRange range, PixelLength size)
    {
        PixelSize largestLabel = new(12, 12);
        Ticks = GenerateTicks(range, size, largestLabel);
    }

    private Tick[] GenerateTicks(CoordinateRange range, PixelLength size, PixelSize predictedTickSize, int depth = 0)
    {
        if (depth > 3)
            System.Diagnostics.Debug.WriteLine($"Warning: Tick recusion depth = {depth}");

        // generate ticks and labels based on predicted maximum label size
        float maxPredictedSize = IsVertical ? predictedTickSize.Height : predictedTickSize.Width;
        double[] majorTickPositions = GenerateTickPositions(range, size, maxPredictedSize);
        string[] majorTickLabels = majorTickPositions.Select(position => GetPrettyTickLabel(position)).ToArray();

        // determine if the actual tick labels are larger than predicted (suggesting density is too high and overlapping may occur)
        using SkiaSharp.SKPaint paint = new();
        PixelSize measuredLabel = Drawing.MeasureLargestString(majorTickLabels, paint);
        PixelSize largestLabel = new(
            width: Math.Max(predictedTickSize.Width, measuredLabel.Width),
            height: Math.Max(predictedTickSize.Height, measuredLabel.Height));
        bool tickExceedsPredictedSize = largestLabel.Area > predictedTickSize.Area;

        // recursively recalculate tick density if necessary
        return tickExceedsPredictedSize
            ? GenerateTicks(range, size, largestLabel, depth + 1)
            : GenerateFinalTicks(majorTickPositions, majorTickLabels, range);
    }

    private static double[] GenerateTickPositions(CoordinateRange range, PixelLength size, float predictedTickSize)
    {
        double unitsPerPx = range.Span / size.Length;

        float tickDensity = 1.0f;
        int targetTickCount = (int)(size.Length / predictedTickSize * tickDensity);
        double tickSpacing = GetIdealTickSpacing(range, targetTickCount);

        double firstTickOffset = range.Min % tickSpacing;
        int tickCount = (int)(range.Span / tickSpacing) + 2;
        tickCount = Math.Min(1000, tickCount);
        tickCount = Math.Max(1, tickCount);

        double[] majorTickPositions = Enumerable.Range(0, tickCount)
            .Select(x => range.Min - firstTickOffset + tickSpacing * x)
            .Where(x => range.Contains(x))
            .ToArray();

        if (majorTickPositions.Length < 2)
        {
            double tickBelow = range.Min - firstTickOffset;
            double firstTick = majorTickPositions.Length > 0 ? majorTickPositions[0] : tickBelow;
            double nextTick = tickBelow + tickSpacing;
            majorTickPositions = new double[] { firstTick, nextTick };
        }

        return majorTickPositions;
    }

    private static Tick[] GenerateFinalTicks(double[] positions, string[] labels, CoordinateRange range, int minorTicksPerMajorTick = 5)
    {
        Tick[] majorTicks = positions
            .Select((position, i) => Tick.Major(position, labels[i]))
            .ToArray();

        Tick[] minorTicks = MinorFromMajor(positions, minorTicksPerMajorTick, range)
            .Select(position => Tick.Minor(position))
            .ToArray();

        return majorTicks.Concat(minorTicks).ToArray();
    }

    private static double GetIdealTickSpacing(CoordinateRange range, int maxTickCount)
    {
        int radix = 10;
        int exponent = (int)Math.Log(range.Span, radix);
        double initialSpace = Math.Pow(radix, exponent);
        List<double> tickSpacings = new() { initialSpace, initialSpace, initialSpace };

        double[] divBy;
        if (radix == 10)
            divBy = new double[] { 2, 2, 2.5 }; // 10, 5, 2.5, 1
        else if (radix == 16)
            divBy = new double[] { 2, 2, 2, 2 }; // 16, 8, 4, 2, 1
        else
            throw new ArgumentException($"radix {radix} is not supported");

        int divisions = 0;
        int tickCount = 0;
        while (tickCount < maxTickCount && tickSpacings.Count < 1000)
        {
            tickSpacings.Add(tickSpacings.Last() / divBy[divisions++ % divBy.Length]);
            tickCount = (int)(range.Span / tickSpacings.Last());
        }

        return tickSpacings[tickSpacings.Count - 3];
    }

    private static double[] MinorFromMajor(double[] majorTicks, double minorTicksPerMajorTick, CoordinateRange range)
    {
        if (majorTicks == null || majorTicks.Length < 2)
            return new double[] { };

        double majorTickSpacing = majorTicks[1] - majorTicks[0];
        double minorTickSpacing = majorTickSpacing / minorTicksPerMajorTick;

        List<double> majorTicksWithPadding = new()
        {
            majorTicks[0] - majorTickSpacing
        };
        majorTicksWithPadding.AddRange(majorTicks);

        List<double> minorTicks = new();
        foreach (var majorTickPosition in majorTicksWithPadding)
        {
            for (int i = 1; i < minorTicksPerMajorTick; i++)
            {
                double minorTickPosition = majorTickPosition + minorTickSpacing * i;
                if (range.Contains(minorTickPosition))
                    minorTicks.Add(minorTickPosition);
            }
        }

        return minorTicks.ToArray();
    }

    private static string GetPrettyTickLabel(double position)
    {
        string label = FormatLocal(position, CultureInfo.CurrentCulture);
        return (label == "-0") ? "0" : label;
    }

    private static string FormatLocal(double value, CultureInfo culture)
    {
        // if the number is round or large, use the numeric format
        // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#the-numeric-n-format-specifier
        bool isRoundNumber = (int)value == value;
        bool isLargeNumber = Math.Abs(value) > 1000;
        if (isRoundNumber || isLargeNumber)
            return value.ToString("N0", culture);

        // otherwise the number is probably small or very precise to use the general format (with slight rounding)
        // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#the-general-g-format-specifier
        return Math.Round(value, 10).ToString("G", culture);
    }
}
