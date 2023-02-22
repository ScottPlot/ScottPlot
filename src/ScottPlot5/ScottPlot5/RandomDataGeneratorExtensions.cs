namespace ScottPlot;

public static class RandomDataGeneratorExtensions
{
    public static OHLC[] RandomOHLCs(this RandomDataGenerator gen, int count)
    {
        DateTime[] dates = Generate.DateTime.Weekdays(count);
        TimeSpan span = TimeSpan.FromDays(1);

        double mult = 1;

        OHLC[] ohlcs = new OHLC[count];
        double open = gen.RandomNumber(150, 250);
        for (int i = 0; i < count; i++)
        {
            double close = open + gen.RandomNumber(-mult, mult);
            double high = Math.Max(open, close) + gen.RandomNumber(0, mult);
            double low = Math.Min(open, close) - gen.RandomNumber(0, mult);
            ohlcs[i] = new OHLC(open, high, low, close, dates[i], span);
            open = close + gen.RandomNumber(-mult / 2, mult / 2);
        }

        return ohlcs;
    }
}
