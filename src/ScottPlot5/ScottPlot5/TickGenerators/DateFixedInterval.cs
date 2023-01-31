using ScottPlot.Axis.TimeUnits;

namespace ScottPlot.TickGenerators
{
    public class DateFixedInterval : IDateTickGenerator
    {
        public ITimeUnit Interval { get; set; }
        public int IntervalsPerTick { get; set; } = 1;

        public DateFixedInterval(ITimeUnit interval, int intervalsPerTick = 1)
        {
            Interval = interval;
            IntervalsPerTick = intervalsPerTick;
        }

        public Tick[] Ticks { get; set; } = Array.Empty<Tick>();
        public int MaxTickCount { get; set; } = 10_000;

        public IEnumerable<double> ConvertToCoordinateSpace(IEnumerable<DateTime> dates) => dates.Select(dt => dt.ToOADate());

        public IEnumerable<Tick> GetVisibleTicks(CoordinateRange range)
        {
            return Ticks.Where(x => range.Contains(x.Position));
        }

        public void Regenerate(CoordinateRange range, PixelLength size)
        {
            List<Tick> ticks = new();

            DateTime start = Interval.Next(DateTime.FromOADate(range.Min), -1);
            DateTime end = Interval.Next(DateTime.FromOADate(range.Max), 1);
            for (DateTime dt = start; dt <= end; dt = Interval.Next(dt, IntervalsPerTick))
            {
                ticks.Add(new Tick(dt.ToOADate(), dt.ToString(Interval.GetDateTimeFormatString()), true));
            }

            Ticks = ticks.ToArray();
        }
    }
}
