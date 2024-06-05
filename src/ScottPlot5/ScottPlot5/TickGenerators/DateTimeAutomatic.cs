namespace ScottPlot.TickGenerators;

public class DateTimeAutomatic : IDateTimeTickGenerator
{
    /// <summary>
    /// If assigned, this function will be used to create tick labels
    /// </summary>
    public Func<DateTime, string>? LabelFormatter { get; set; } = null;

    public ITimeUnit? TimeUnit { get; private set; } = null;

    private readonly static List<ITimeUnit> TheseTimeUnits =
    [
        new TimeUnits.Millisecond(),
        new TimeUnits.Centisecond(),
        new TimeUnits.Decisecond(),
        new TimeUnits.Second(),
        new TimeUnits.Minute(),
        new TimeUnits.Hour(),
        new TimeUnits.Day(),
        new TimeUnits.Month(),
        new TimeUnits.Year(),
    ];

    public Tick[] Ticks { get; set; } = [];

    public int MaxTickCount { get; set; } = 10_000;

    private ITimeUnit GetAppropriateTimeUnit(TimeSpan timeSpan, int targetTickCount = 10)
    {
        foreach (var timeUnit in TheseTimeUnits)
        {
            long estimatedUnitTicks = timeSpan.Ticks / timeUnit.MinSize.Ticks;
            foreach (var increment in timeUnit.Divisors)
            {
                long estimatedTicks = estimatedUnitTicks / increment;
                if (estimatedTicks > targetTickCount / 3 && estimatedTicks < targetTickCount * 3)
                    return timeUnit;
            }
        }

        return TheseTimeUnits.Last();
    }

    private ITimeUnit GetLargerTimeUnit(ITimeUnit timeUnit)
    {
        for (int i = 0; i < TheseTimeUnits.Count - 1; i++)
        {
            if (timeUnit.GetType() == TheseTimeUnits[i].GetType())
            {
                return TheseTimeUnits[i + 1];
            }
        }

        return TheseTimeUnits.Last();
    }

    private int? LeastMemberGreaterThan(double value, IReadOnlyList<int> list)
    {
        for (int i = 0; i < list.Count; i++)
            if (list[i] > value)
                return list[i];
        return null;
    }

    public void Regenerate(CoordinateRange range, Edge edge, PixelLength size, SKPaint paint, Label labelStyle)
    {
        if (range.Span >= TimeSpan.MaxValue.Days || double.IsNaN(range.Span))
        {
            // cases of extreme zoom (10,000 years)
            Ticks = [];
            return;
        }

        TimeSpan span = TimeSpan.FromDays(range.Span);
        ITimeUnit? timeUnit = GetAppropriateTimeUnit(span);

        // estimate the size of the largest tick label for this unit this unit
        int maxExpectedTickLabelWidth = (int)Math.Max(16, span.TotalDays / MaxTickCount);
        int tickLabelHeight = 12;
        PixelSize tickLabelBounds = new(maxExpectedTickLabelWidth, tickLabelHeight);
        double coordinatesPerPixel = range.Span / size.Length;

        while (true)
        {
            // determine the ideal spacing to use between ticks
            double increment = coordinatesPerPixel * tickLabelBounds.Width / timeUnit.MinSize.TotalDays;
            int? niceIncrement = LeastMemberGreaterThan(increment, timeUnit.Divisors);
            if (niceIncrement is null)
            {
                timeUnit = TheseTimeUnits.FirstOrDefault(t => t.MinSize > timeUnit.MinSize);
                if (timeUnit is not null)
                    continue;
                timeUnit = TheseTimeUnits.Last();
                niceIncrement = (int)Math.Ceiling(increment);
            }

            TimeUnit = timeUnit;

            // attempt to generate the ticks given these conditions
            (List<Tick>? ticks, PixelSize? largestTickLabelSize) = GenerateTicks(range, timeUnit, niceIncrement.Value, tickLabelBounds, paint, labelStyle);

            // if ticks were returned, use them
            if (ticks is not null)
            {
                Ticks = [.. ticks];
                return;
            }

            // if no ticks were returned it means the conditions were too dense and tick labels
            // overlapped, so expand the tick label bounds and try again.
            if (largestTickLabelSize is not null)
            {
                tickLabelBounds = tickLabelBounds.Max(largestTickLabelSize.Value);
                tickLabelBounds = new PixelSize(tickLabelBounds.Width + 10, tickLabelBounds.Height + 10);
                continue;
            }

            throw new InvalidOperationException($"{nameof(ticks)} and {nameof(largestTickLabelSize)} are both null");
        }
    }

    /// <summary>
    /// This method attempts to find an ideal set of ticks.
    /// If all labels fit within the bounds, the list of ticks is returned.
    /// If a label doesn't fit in the bounds, the list is null and the size of the large tick label is returned.
    /// </summary>
    private (List<Tick>? Positions, PixelSize? PixelSize) GenerateTicks(CoordinateRange range, ITimeUnit unit, int increment, PixelSize tickLabelBounds, SKPaint paint, Label labelStyle)
    {
        DateTime rangeMin = NumericConversion.ToDateTime(range.Min);
        DateTime rangeMax = NumericConversion.ToDateTime(range.Max);

        // range.Min could be anything, but when calculating start it must be "snapped" to the best tick
        DateTime start = GetLargerTimeUnit(unit).Snap(rangeMin);

        start = unit.Next(start, -increment);

        List<Tick> ticks = [];

        const int maxTickCount = 1000;
        for (DateTime dt = start; dt <= rangeMax; dt = unit.Next(dt, increment))
        {
            if (dt < rangeMin)
                continue;

            string tickLabel = LabelFormatter is null
                ? dt.ToString(unit.GetDateTimeFormatString())
                : LabelFormatter(dt);

            PixelSize tickLabelSize = labelStyle.Measure(tickLabel, paint).Size;

            bool tickLabelIsTooLarge = !tickLabelBounds.Contains(tickLabelSize);
            if (tickLabelIsTooLarge)
                return (null, tickLabelSize);

            double tickPosition = NumericConversion.ToNumber(dt);
            Tick tick = new(tickPosition, tickLabel, isMajor: true);
            ticks.Add(tick);

            // this prevents infinite loops with weird axis limits or small delta (e.g., DateTime)
            if (ticks.Count >= maxTickCount)
                break;
        }

        return (ticks, null);
    }

    public IEnumerable<double> ConvertToCoordinateSpace(IEnumerable<DateTime> dates)
    {
        return dates.Select(NumericConversion.ToNumber);
    }
}
