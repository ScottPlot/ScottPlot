namespace ScottPlot.TickGenerators.Financial;

public class MonthsAndMondays : IFinancialTickGenerator
{
    public List<(int, string)> GetTicks(DateTime[] DateTimes, int minIndexInView, int maxIndexInView)
    {
        List<(int, string)> ticks = [];

        int lastMonth = DateTimes[0].Month;
        for (int i = minIndexInView; i <= maxIndexInView; i++)
        {
            DateTime dt = DateTimes[i];
            if (dt.Month != lastMonth)
            {
                ticks.Add((i, dt.ToString("MMM")));
                lastMonth = dt.Month;
            }
            else if (dt.DayOfWeek == DayOfWeek.Monday && dt.Day > 14 && dt.Day < 27)
            {
                ticks.Add((i, dt.Day.ToString()));
            }
        }

        return ticks;
    }
}
