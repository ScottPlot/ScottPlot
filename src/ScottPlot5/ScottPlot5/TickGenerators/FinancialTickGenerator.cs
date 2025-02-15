namespace ScottPlot.TickGenerators;

/// <summary>
/// Experimental tick generator designed for displaying financial time series data
/// where values are evenly spaced visually despite having DateTimes which may contain gaps.
/// </summary>
public class FinancialTickGenerator(DateTime[] dates) : ITickGenerator
{
    // NOTE: Using a tick generator like this for financial charting is probably not the best solution.
    // Tick generators are for things like tick marks and grid lines.
    // Tick marks strictly have a single text label, but financial charts may benefit
    // from stacked labels like month on top and day on bottom.
    // A custom plottable may be ideal for displaying DateTime labels outside the data area.
    // This class works somewhat, but is limited in how much it can do.

    private DateTime[] Dates { get; } = dates;

    public Tick[] Ticks { get; private set; } = [];

    public int MaxTickCount { get; set; } = 1000;

    public void Regenerate(CoordinateRange range, Edge edge, PixelLength size, SKPaint paint, LabelStyle labelStyle)
    {
        if (Dates.Length == 0)
        {
            if (Ticks.Length > 0)
                Ticks = [];
            return;
        }

        // TODO: add different functions for year, month and day, day, etc. based on range
        int minIndexInView = (int)(Math.Max(0, range.Min));
        int maxIndexInView = (int)(Math.Min(Dates.Length - 1, range.Max));
        if (minIndexInView >= maxIndexInView)
        {
            Ticks = [];
            return;
        }

        TimeSpan timeSpanInView = Dates[maxIndexInView] - Dates[minIndexInView];

        if (timeSpanInView.TotalDays < 45)
        {
            Ticks = GetDayTicks();
        }
        else if (timeSpanInView.TotalDays < 365 * 2)
        {
            Ticks = GetMonthTicks();
        }
        else
        {
            Ticks = []; // TODO: year ticks
        }
    }

    private Tick[] GetMonthTicks()
    {
        List<Tick> ticks = [];

        int lastMonth = Dates.First().Month;

        for (int i = 0; i < Dates.Length; i++)
        {
            DateTime date = Dates[i];
            if (date.Month != lastMonth)
            {
                string label = date.ToString("MMM");
                ticks.Add(Tick.Major(i, label));
                lastMonth = date.Month;
            }
        }

        return [.. ticks];
    }

    private Tick[] GetDayTicks()
    {
        List<Tick> ticks = [];

        int lastDay = Dates.First().Day;
        int lastMonth = Dates.First().Month;

        for (int i = 0; i < Dates.Length; i++)
        {
            DateTime date = Dates[i];
            if (date.Day != lastDay)
            {
                string label = date.Day.ToString();
                if (date.Month != lastMonth)
                {
                    lastMonth = date.Month;
                    label = date.ToString("MMM");
                }
                ticks.Add(Tick.Major(i, label));
                lastDay = date.Day;
            }
        }

        return [.. ticks];
    }
}
