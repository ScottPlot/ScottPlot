namespace ScottPlot.TickGenerators;

public class NumericAutomatic : ITickGenerator
{
    public Tick[] Ticks { get; set; } = [];

    public int MaxTickCount
    {
        get => TickSpacingCalculator.MaximumTickCount;
        set => TickSpacingCalculator.MaximumTickCount = value;
    }

    public bool IntegerTicksOnly { get; set; } = false;

    public Func<double, string> LabelFormatter { get; set; } = LabelFormatters.Numeric;

    public IMinorTickGenerator MinorTickGenerator { get; set; } = new EvenlySpacedMinorTickGenerator(5);

    public DecimalTickSpacingCalculator TickSpacingCalculator = new();
    public float MinimumTickSpacing { get; set; } = 0;
    public double TickDensity { get; set; } = 1.0; // TODO: consider adding logic to make this a fraction of the width in pixels
    public int? TargetTickCount = null;

    public void Regenerate(CoordinateRange range, Edge edge, PixelLength size, SKPaint paint, LabelStyle labelStyle)
    {
        Ticks = GenerateTicks(range, edge, size, new PixelLength(12), paint, labelStyle)
            .Where(x => range.Contains(x.Position))
            .ToArray();
    }

    private Tick[] GenerateTicks(CoordinateRange range, Edge edge, PixelLength axisLength, PixelLength maxLabelLength, SKPaint paint, LabelStyle labelStyle, int depth = 0)
    {
        if (depth > 3)
            Debug.WriteLine($"Warning: Tick recursion depth = {depth}");

        // generate ticks and labels based on predicted maximum label size
        float labelWidth = Math.Max(MinimumTickSpacing, maxLabelLength.Length * (1 / (float)TickDensity));

        if (TargetTickCount.HasValue)
        {
            labelWidth = axisLength.Length / (TargetTickCount.Value + 1);
        }

        double[] majorTickPositions = TickSpacingCalculator.GenerateTickPositions(range, axisLength, labelWidth);
        string[] majorTickLabels = majorTickPositions.Select(x => LabelFormatter(x)).ToArray();

        // determine if the actual tick labels are larger than predicted,
        // suggesting density is too high and overlapping may occur.
        (string largestText, PixelLength actualMaxLength) = edge.IsVertical()
            ? labelStyle.MeasureHighestString(majorTickLabels, paint)
            : labelStyle.MeasureWidestString(majorTickLabels, paint);

        // recursively recalculate tick density if necessary
        return actualMaxLength.Length > maxLabelLength.Length
            ? GenerateTicks(range, edge, axisLength, actualMaxLength, paint, labelStyle, depth + 1)
            : GenerateFinalTicks(majorTickPositions, majorTickLabels, range);
    }

    private Tick[] GenerateFinalTicks(double[] positions, string[] labels, CoordinateRange visibleRange)
    {
        if (IntegerTicksOnly)
        {
            List<int> indexesToKeep = [];
            for (int i = 0; i < positions.Length; i++)
            {
                double position = positions[i];
                double distanceFromInteger = Math.Abs(position - (int)position);
                if (distanceFromInteger < .01)
                    indexesToKeep.Add(i);
            }

            positions = indexesToKeep.Select(x => positions[x]).ToArray();
            labels = indexesToKeep.Select(x => labels[x]).ToArray();
        }

        var majorTicks = positions.Select((position, i) => Tick.Major(position, labels[i]));

        var minorTicks = MinorTickGenerator.GetMinorTicks(positions, visibleRange).Select(Tick.Minor);

        return [.. majorTicks, .. minorTicks];
    }
}
