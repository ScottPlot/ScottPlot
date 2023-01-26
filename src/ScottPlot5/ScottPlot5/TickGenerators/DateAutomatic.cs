namespace ScottPlot.TickGenerators
{
    public class DateAutomatic : ITickGenerator
    {
        public Tick[] Ticks { get; set; } = Array.Empty<Tick>();
        public int MaxTickCount { get; set; } = 10_000; // TODO: Honor this

        public IEnumerable<Tick> GetVisibleTicks(CoordinateRange range)
        {
            return Ticks.Where(x => range.Contains(x.Position));
        }

        public void Regenerate(CoordinateRange range, PixelLength size)
        {
            Ticks = GetTicks(range).ToArray();
        }

        private static IEnumerable<Tick> GetTicks(CoordinateRange range)
        {
            // TODO: This is hopelessly naive
            for (double i = (int)range.Min; i <= range.Max + 1; i += 14)
            {
                yield return new Tick(i, DateTime.FromOADate(i).ToLongDateString(), true);
            }
        }
    }
}