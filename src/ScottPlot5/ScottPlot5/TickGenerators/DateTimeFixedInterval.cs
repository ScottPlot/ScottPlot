using ScottPlot.TickGenerators.TimeUnits;

namespace ScottPlot.TickGenerators
{
    public class DateTimeFixedInterval : IDateTimeTickGenerator
    {
        /// <summary>
        /// The time unit to use for major ticks
        /// </summary>
        public ITimeUnit Interval { get; set; }

        /// <summary>
        /// The number of <see cref="Interval"/> units between major ticks (e.g. major ticks every 7 <see cref="Day"/>s)
        /// </summary>
        public int IntervalsPerTick { get; set; }

        /// <summary>
        /// The time unit to use for minor ticks. If null, no minor ticks are generated.
        /// </summary>
        public ITimeUnit? MinorInterval { get; set; }

        /// <summary>
        /// The number of <see cref="MinorInterval"/> units between minor ticks.
        /// </summary>
        public int MinorIntervalsPerTick { get; set; }

        /// <summary>
        /// An optional function to override where the intervals for ticks start. The DateTime argument provided is
        /// the start range of the axis (i.e. <see cref="IAxis.Min"/>).
        /// </summary>
        /// <remarks>
        /// If omitted, the ticks will start from <see cref="IAxis.Min"/>. This may have undesirable effects when zooming
        /// and panning. If provided, the ticks will start from the returned DateTime.
        /// </remarks>
        /// <example>
        /// If the plot contains weekly data, and it is desired to have ticks on the 1st of each month:
        /// <code>
        /// dt => new DateTime(dt.Year, dt.Month, 1);
        /// </code>
        /// If the plot contains hourly data, and it is desired to have ticks every 6 hours at 00:00, 6:00, 12:00, etc,
        /// then set <see cref="Interval"/> to <see cref="Hour"/>, <see cref="IntervalsPerTick"/> to 6, and provide the function:
        /// <code>
        /// dt => new DateTime(dt.Year, dt.Month, dt.Day);
        /// </code>
        /// </example>
        public Func<DateTime, DateTime>? GetIntervalStartFunc { get; set; }

        /// <summary>
        /// If assigned, this function will be used to create tick labels
        /// </summary>
        public Func<DateTime, string>? LabelFormatter { get; set; } = null;

        /// <summary>
        /// Creates a new <see cref="DateTimeFixedInterval"/> generator.
        /// </summary>
        /// <param name="interval">The time unit to use for major ticks</param>
        /// <param name="intervalsPerTick">The number of <see cref="Interval"/> units between major ticks</param>
        /// <param name="minorInterval">The time unit to use for minor ticks. If null, no minor ticks are generated.</param>
        /// <param name="minorIntervalsPerTick">The number of <see cref="MinorInterval"/> units between minor ticks.</param>
        /// <param name="getIntervalStartFunc">
        /// An optional function to override where the intervals for ticks start. The DateTime argument provided is
        /// the start range of the axis (i.e. <see cref="IAxis.Min"/>).
        /// </param>
        public DateTimeFixedInterval(ITimeUnit interval, int intervalsPerTick = 1, ITimeUnit? minorInterval = null, int minorIntervalsPerTick = 1, Func<DateTime, DateTime>? getIntervalStartFunc = null)
        {
            Interval = interval;
            IntervalsPerTick = intervalsPerTick;
            MinorInterval = minorInterval;
            MinorIntervalsPerTick = minorIntervalsPerTick;
            GetIntervalStartFunc = getIntervalStartFunc;
        }

        public Tick[] Ticks { get; set; } = Array.Empty<Tick>();

        public int MaxTickCount { get; set; } = 10_000;

        public IEnumerable<double> ConvertToCoordinateSpace(IEnumerable<DateTime> dates)
        {
            return dates.Select(NumericConversion.ToNumber);
        }

        public void Regenerate(CoordinateRange range, Edge edge, PixelLength size, SKPaint paint, Label labelStyle)
        {
            List<Tick> ticks = new();
            HashSet<DateTime> timesWithTicks = new(); // Avoid having minor and major ticks at same time

            // Modify the start time of the ticks if a delegate was provided
            DateTime rangeMin = NumericConversion.ToDateTime(range.Min);
            DateTime start = GetIntervalStartFunc?.Invoke(rangeMin) ?? Interval.Next(rangeMin, -1 * IntervalsPerTick);

            DateTime end = Interval.Next(NumericConversion.ToDateTime(range.Max));
            for (DateTime dt = start; dt <= end; dt = Interval.Next(dt, IntervalsPerTick))
            {
                string tickLabel = LabelFormatter is null
                    ? dt.ToString(Interval.GetDateTimeFormatString())
                    : LabelFormatter(dt);

                ticks.Add(new Tick(NumericConversion.ToNumber(dt), tickLabel, true));
                timesWithTicks.Add(dt);
            }

            if (MinorInterval is not null)
            {
                for (DateTime dt = start; dt <= end; dt = MinorInterval.Next(dt, MinorIntervalsPerTick))
                {
                    if (timesWithTicks.Contains(dt))
                    {
                        continue;
                    }

                    ticks.Add(new Tick(NumericConversion.ToNumber(dt), dt.ToString(Interval.GetDateTimeFormatString()), false));
                }
            }

            Ticks = ticks.Where(x => range.Contains(x.Position)).ToArray();
        }
    }
}
