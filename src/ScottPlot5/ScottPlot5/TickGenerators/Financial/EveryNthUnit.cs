namespace ScottPlot.TickGenerators.Financial
{
    public class EveryNthUnit : IFinancialTickGenerator
    {
        private int PlaceEveryNthUnit { get; }

        public EveryNthUnit(int placeEveryNthUnit)
        {
            PlaceEveryNthUnit = placeEveryNthUnit;
        }

        public List<(int, string)> GetTicks(DateTime[] DateTimes, int minIndexInView, int maxIndexInView)
        {
            List<(int, string)> ticks = [];
            var initialIndex = minIndexInView % PlaceEveryNthUnit == 0
                ? minIndexInView
                : minIndexInView + (minIndexInView % PlaceEveryNthUnit);
            for (int i = initialIndex; i <= maxIndexInView; i += PlaceEveryNthUnit)
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
}
