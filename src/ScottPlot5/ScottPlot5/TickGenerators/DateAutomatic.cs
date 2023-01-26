namespace ScottPlot.TickGenerators
{
    public class DateAutomatic : ITickGenerator
    {
        public Tick[] Ticks { get; set; } = Array.Empty<Tick>();
        public int MaxTickCount { get; set; } = 10_000;

        public IEnumerable<Tick> GetVisibleTicks(CoordinateRange range)
        {
            return Ticks.Where(x => range.Contains(x.Position));
        }

        public void Regenerate(CoordinateRange range, PixelLength size)
        {
            using SKPaint paint = new();
            TimeSpan span = TimeSpan.FromDays(range.Span);
            TimeSpan increment = TimeSpan.FromDays(1); // TODO: Don't hardcode this

            int minTickWidth = (int)Math.Max(16, span.TotalDays / MaxTickCount);
            PixelSize bounds = new(minTickWidth, 12);
            
            while(true)
            {
                double coordinatesPerPixel = range.Span / size.Length;
                int numberOfIncrements = (int)Math.Ceiling(coordinatesPerPixel * bounds.Width); // TODO: Favour nice numbers (6 hours, 7 days, 3 months, etc)

                var newIncrement = TimeSpan.FromTicks(increment.Ticks * numberOfIncrements); // Multiplication of TimeSpans isn't supported on all targets
                var result = GenerateTicks(range, newIncrement, bounds, paint);
                
                if (result.ActiveField == 0)
                {
                    Ticks = result.First.ToArray();
                    return;
                } else
                {
                    bounds = bounds.Max(result.Second);
                }
            }
        }

        private static Option<List<Tick>, PixelSize> GenerateTicks(CoordinateRange range, TimeSpan increment, PixelSize bounds, SKPaint paint)
        {
            List<Tick> ticks = new();

            DateTime start = DateTime.FromOADate(range.Min) - increment;
            DateTime end = DateTime.FromOADate(range.Max) + increment;
            for (DateTime dt = start; dt <= end; dt += increment)
            {
                var text = dt.ToLongDateString(); // TODO: Proper format

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
}