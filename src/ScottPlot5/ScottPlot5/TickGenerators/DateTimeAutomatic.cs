using ScottPlot.Axis.TimeUnits;

namespace ScottPlot.TickGenerators;

public class DateTimeAutomatic : IDateTickGenerator
{
    private readonly static IReadOnlyList<ITimeUnit> defaultTimeUnits;

    static DateTimeAutomatic()
    {
        defaultTimeUnits = typeof(ITimeUnit).Assembly
            .GetTypes()
            .Where(t => t.IsClass && t.Namespace == "ScottPlot.Axis.TimeUnits" && t.GetInterfaces().Contains(typeof(ITimeUnit)))
            .Select(t => (ITimeUnit)Activator.CreateInstance(t)!)
            .OrderBy(t => t.MinSize)
            .ToArray();
    }

    public Tick[] Ticks { get; set; } = Array.Empty<Tick>();
    public int MaxTickCount { get; set; } = 10_000;

    public IReadOnlyList<ITimeUnit> TimeUnits { get; set; } = defaultTimeUnits;

    private ITimeUnit GetAppropriateTimeUnit(TimeSpan timeSpan, int targetTickCount = 10)
    {
        foreach (var timeUnit in TimeUnits)
        {
            long estimatedUnitTicks = timeSpan.Ticks / timeUnit.MinSize.Ticks;
            foreach (var increment in timeUnit.Divisors)
            {
                long estimatedTicks = estimatedUnitTicks / increment;
                if (estimatedTicks > targetTickCount / 3 && estimatedTicks < targetTickCount * 3)
                    return timeUnit;
            }
        }

        return TimeUnits.Last();
    }

    private ITimeUnit GetLargerTimeUnit(ITimeUnit timeUnit)
    {
        for (int i = 0; i < TimeUnits.Count - 1; i++)
        {
            if (timeUnit.GetType() == TimeUnits[i].GetType())
            {
                return TimeUnits[i + 1];
            }
        }

        return TimeUnits.Last();
    }

    private int? LeastMemberGreaterThan(double value, IReadOnlyList<int> list)
    {
        for (int i = 0; i < list.Count; i++)
            if (list[i] > value)
                return list[i];
        return null;
    }

    public IEnumerable<Tick> GetVisibleTicks(CoordinateRange range)
    {
        return Ticks.Where(x => range.Contains(x.Position));
    }

    public void Regenerate(CoordinateRange range, PixelLength size)
    {
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
                timeUnit = TimeUnits.FirstOrDefault(t => t.MinSize > timeUnit.MinSize);
                if (timeUnit is not null)
                    continue;
                timeUnit = TimeUnits[TimeUnits.Count - 1];
                niceIncrement = (int)Math.Ceiling(increment);
            }

            // attempt to generate the ticks given these conditions
            (List<Tick>? ticks, PixelSize? largestTickLabelSize) = GenerateTicks(range, timeUnit, niceIncrement.Value, tickLabelBounds);

            // if ticks were returned, use them
            if (ticks is not null)
            {
                Ticks = ticks.ToArray();
                return;
            }

            // if no ticks were returned it means the conditions were too dense and tick labels
            // overlapped, so expand the tick label bounds and try again.
            if (largestTickLabelSize is not null)
            {
                tickLabelBounds = tickLabelBounds.Max(largestTickLabelSize.Value);
                tickLabelBounds.Expand(new PixelPadding(10, 10, 0, 0));
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
    private (List<Tick>? Positions, PixelSize? PixelSize) GenerateTicks(CoordinateRange range, ITimeUnit unit, int increment, PixelSize tickLabelBounds)
    {
        DateTime rangeMin = DateTime.FromOADate(range.Min);
        DateTime rangeMax = DateTime.FromOADate(range.Max);

        // range.Min could be anything, but when calculating start and stop it must be "snapped" to the best tick
        rangeMin = GetLargerTimeUnit(unit).Snap(rangeMin);
        rangeMax = unit.Snap(rangeMax);

        DateTime start = unit.Next(rangeMin, -increment);
        DateTime end = unit.Next(rangeMax, increment);
        string dtFormat = unit.GetDateTimeFormatString();

        using SKPaint paint = new();
        List<Tick> ticks = new();
        for (DateTime dt = start; dt <= end; dt = unit.Next(dt, increment))
        {
            string tickLabel = dt.ToString(dtFormat);
            PixelSize tickLabelSize = Drawing.MeasureString(tickLabel, paint);

            bool tickLabelIsTooLarge = !tickLabelBounds.Contains(tickLabelSize);
            if (tickLabelIsTooLarge)
                return (null, tickLabelSize);

            double tickPosition = dt.ToOADate();
            Tick tick = new(tickPosition, tickLabel, isMajor: true);
            ticks.Add(tick);
        }

        return (ticks, null);
    }

    public IEnumerable<double> ConvertToCoordinateSpace(IEnumerable<DateTime> dates) => dates.Select(dt => dt.ToOADate());
}
