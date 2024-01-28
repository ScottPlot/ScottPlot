namespace ScottPlot.TickGenerators;

public class NumericAutomatic : ITickGenerator
{
    public Tick[] Ticks { get; set; } = [];

    public int MaxTickCount { get; set; } = 10_000;

    public bool IntegerTicksOnly { get; set; } = false;

    public Func<double, string> LabelFormatter { get; set; } = DefaultLabelFormatter;

    public IMinorTickGenerator MinorTickGenerator { get; set; } = new EvenlySpacedMinorTickGenerator(5);

    public DecimalTickSpacingCalculator TickSpacingCalculator = new();

    public static string DefaultLabelFormatter(double value)
    {
        CultureInfo culture = CultureInfo.InvariantCulture;

        // if the number is round or large, use the numeric format
        // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#the-numeric-n-format-specifier
        bool isRoundNumber = (int)value == value;
        bool isLargeNumber = Math.Abs(value) > 1000;
        if (isRoundNumber || isLargeNumber)
            return value.ToString("N0", culture);

        // otherwise the number is probably small or very precise to use the general format (with slight rounding)
        // https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings#the-general-g-format-specifier
        string label = Math.Round(value, 10).ToString("G", culture);
        return label == "-0" ? "0" : label;
    }

    public void Regenerate(CoordinateRange range, Edge edge, PixelLength size, SKPaint paint)
    {
        Ticks = GenerateTicks(range, edge, size, new PixelLength(12), paint)
            .Where(x => range.Contains(x.Position))
            .ToArray();
    }

    private Tick[] GenerateTicks(CoordinateRange range, Edge edge, PixelLength axisLength, PixelLength maxLabelLength, SKPaint paint, int depth = 0)
    {
        if (depth > 3)
            Debug.WriteLine($"Warning: Tick recusion depth = {depth}");

        // generate ticks and labels based on predicted maximum label size
        double tickSpacing = TickSpacingCalculator.GetIdealTickSpacing(range, axisLength, maxLabelLength.Length, MaxTickCount);
        double[] majorTickPositions = GenerateTickPositions(range, tickSpacing);
        string[] majorTickLabels = majorTickPositions.Select(x => LabelFormatter(x)).ToArray();

        // determine if the actual tick labels are larger than predicted,
        // suggesting density is too high and overlapping may occur.
        (string largestText, PixelLength actualMaxLength) = edge.IsVertical()
            ? Drawing.MeasureWidestString(majorTickLabels, paint)
            : Drawing.MeasureWidestString(majorTickLabels, paint);

        Debug.WriteLineIf(edge == Edge.Bottom, $"[{depth}] Largest: {actualMaxLength} '{largestText}'");

        // recursively recalculate tick density if necessary
        return actualMaxLength.Length > maxLabelLength.Length
            ? GenerateTicks(range, edge, axisLength, actualMaxLength, paint, depth + 1)
            : GenerateFinalTicks(majorTickPositions, majorTickLabels, range);
    }

    private double[] GenerateTickPositions(CoordinateRange range, double tickSpacing)
    {
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

    private Tick[] GenerateFinalTicks(double[] positions, string[] labels, CoordinateRange visibleRange)
    {
        // TODO: make this process cleaner
        if (IntegerTicksOnly)
        {
            List<int> indexes = [];
            for (int i = 0; i < positions.Length; i++)
            {
                if (positions[i] == (int)positions[i])
                    indexes.Add(i);
            }

            positions = indexes.Select(x => positions[x]).ToArray();
            labels = indexes.Select(x => labels[x]).ToArray();
        }

        var majorTicks = positions.Select((position, i) => Tick.Major(position, labels[i]));

        var minorTicks = MinorTickGenerator.GetMinorTicks(positions, visibleRange).Select(Tick.Minor);

        return [.. majorTicks, .. minorTicks];
    }
}
