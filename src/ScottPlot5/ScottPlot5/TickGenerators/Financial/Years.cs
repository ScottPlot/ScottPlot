namespace ScottPlot.TickGenerators.Financial;

public class Years : IFinancialTickGenerator
{
    public List<(int, string)> GetTicks(DateTime[] DateTimes, int minIndexInView, int maxIndexInView)
    {
        List<(int, string)> ticks = [];

        int lastYear = DateTimes[0].Year;
        for (int i = minIndexInView; i <= maxIndexInView; i++)
        {
            DateTime dt = DateTimes[i];
            if (dt.Year != lastYear)
            {
                ticks.Add((i, dt.Year.ToString()));
                lastYear = dt.Year;
            }
        }

        return ticks;
    }
}
