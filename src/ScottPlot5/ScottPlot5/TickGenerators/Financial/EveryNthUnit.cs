namespace ScottPlot.TickGenerators.Financial;

public class EveryNthUnit : IFinancialTickGenerator
{
    private int PlaceEveryNthUnit { get; }
    private int StartIndex { get; set; }

    public EveryNthUnit(int placeEveryNthUnit, int startIndex)
    {
        PlaceEveryNthUnit = placeEveryNthUnit;
        StartIndex = startIndex;
    }

    public List<(int, string)> GetTicks(DateTime[] DateTimes, int minIndexInView, int maxIndexInView)
    {
        List<(int, string)> ticks = [];

        if (StartIndex == -1)
        {
            StartIndex = minIndexInView;
        }

        while (StartIndex < minIndexInView)
        {
            StartIndex += PlaceEveryNthUnit;
        }

        if ((StartIndex - minIndexInView) > PlaceEveryNthUnit)
        {
            var multiplier = (StartIndex - minIndexInView) / PlaceEveryNthUnit;
            StartIndex -= multiplier * PlaceEveryNthUnit;
        }

        for (int i = StartIndex; i <= maxIndexInView; i += PlaceEveryNthUnit)
        {
            DateTime dt = DateTimes[i];
            try
            {
                var idx = i - PlaceEveryNthUnit;
                if (idx < 0)
                    ticks.Add((i, dt.ToString("MMM - MM")));
                else if (dt.Day > DateTimes[idx].Day)
                    if (dt.Month > DateTimes[idx].Month)
                        ticks.Add((i, dt.ToString("MMM - MM")));
                    else
                        ticks.Add((i, dt.ToString("ddd - dd")));
                else
                    ticks.Add((i, dt.ToString("HH:mm:ss")));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        return ticks;
    }
}
