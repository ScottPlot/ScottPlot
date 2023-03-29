namespace ScottPlot;

public static class RandomDataGeneratorExtensions
{
    public static List<IOHLC> RandomOHLCs(this RandomDataGenerator gen, int count)
    {
        DateTime[] dates = Generate.DateTime.Weekdays(count);
        TimeSpan span = TimeSpan.FromDays(1);

        double mult = 1;

        List<IOHLC> ohlcs = new();
        double open = gen.RandomNumber(150, 250);
        for (int i = 0; i < count; i++)
        {
            double close = open + gen.RandomNumber(-mult, mult);
            double high = Math.Max(open, close) + gen.RandomNumber(0, mult);
            double low = Math.Min(open, close) - gen.RandomNumber(0, mult);
            OHLC ohlc = new(open, high, low, close, dates[i], span);
            ohlcs.Add(ohlc);
            open = close + gen.RandomNumber(-mult / 2, mult / 2);
        }

        return ohlcs;
    }
}
