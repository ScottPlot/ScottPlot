using ScottPlot;
using ScottPlot.Axis.TimeUnits;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ScottPlot.TickGenerators;

public class DateAutomatic : ITickGenerator
{
    private readonly static IReadOnlyList<ITimeUnit> defaultTimeUnits;
    static DateAutomatic() { 
        defaultTimeUnits = typeof(ITimeUnit).Assembly
        .GetTypes()
        .Where(t => t.IsClass && t.Namespace == "ScottPlot.Axis.TimeUnits" && t.GetInterfaces().Contains(typeof(ITimeUnit)))
        .Select(t => (ITimeUnit)Activator.CreateInstance(t))
        .OrderBy(t => t.MinSize)
        .ToArray();
    }
    
    public Tick[] Ticks { get; set; } = Array.Empty<Tick>();
    public int MaxTickCount { get; set; } = 10_000;

    public IReadOnlyList<ITimeUnit> TimeUnits { get; set; } = defaultTimeUnits;

    public ITimeUnit GetAppropriateTimeUnit(TimeSpan timeSpan, int targetTickCount = 10)
    {
        foreach (var timeUnit in TimeUnits)
        {
            long estimatedUnitTicks = timeSpan.Ticks / timeUnit.MinSize.Ticks;
            foreach (var increment in timeUnit.NiceIncrements) {
                long estimatedTicks = estimatedUnitTicks / increment;
                if (estimatedTicks > targetTickCount / 2 && estimatedTicks < targetTickCount * 2)
                    return timeUnit;
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
        using SKPaint paint = new();
        TimeSpan span = TimeSpan.FromDays(range.Span);

        ITimeUnit timeUnit = GetAppropriateTimeUnit(span);

        int minTickWidth = (int)Math.Max(16, span.TotalDays / MaxTickCount);
        PixelSize bounds = new(minTickWidth, 12);

        while (true)
        {
            double coordinatesPerPixel = range.Span / size.Length;
            double increment = coordinatesPerPixel * bounds.Width / timeUnit.MinSize.TotalDays;
            int? niceIncrement = LeastMemberGreaterThan(increment, timeUnit.NiceIncrements);
            
            if (niceIncrement is null)
            {
                timeUnit = TimeUnits.FirstOrDefault(t => t.MinSize > timeUnit.MinSize);
                if (timeUnit is null)
                {
                    timeUnit = TimeUnits.Last();
                    niceIncrement = (int)Math.Ceiling(increment);
                } else
                {
                    continue;
                }
            }

            var result = GenerateTicks(range, timeUnit, niceIncrement.Value, bounds, paint);

            if (result.ActiveField == 0)
            {
                Ticks = result.First.ToArray();
                return;
            }
            else
            {
                bounds = bounds.Max(result.Second);
            }
        }
    }

    private static Option<List<Tick>, PixelSize> GenerateTicks(CoordinateRange range, ITimeUnit unit, int increment, PixelSize bounds, SKPaint paint)
    {
        // TODO: This generates tick labels with newlines, which are not measured or rendered correctly by skia
        List<Tick> ticks = new();

        DateTime start = unit.Next(DateTime.FromOADate(range.Min), -increment);
        DateTime end = unit.Next(DateTime.FromOADate(range.Max), increment);
        for (DateTime dt = start; dt <= end; dt = unit.Next(dt, increment))
        {
            var text = dt.ToString(unit.GetFormat());

            var actualSize = Drawing.MeasureString(text, paint);
            if (!bounds.Contains(actualSize))
            {
                return new(actualSize);
            }

            ticks.Add(new Tick(dt.ToOADate(), text, true));
        }

        return new(ticks);
    }
}