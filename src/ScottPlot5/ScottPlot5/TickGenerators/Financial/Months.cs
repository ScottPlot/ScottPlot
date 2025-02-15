namespace ScottPlot.TickGenerators.Financial;

public class Months : IFinancialTickGenerator
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
                string label = dt.Month == 1 ? dt.Year.ToString() : dt.ToString("MMM");
                ticks.Add((i, label));
                lastMonth = dt.Month;
            }
        }

        return ticks;
    }
}
