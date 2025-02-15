namespace ScottPlot.DataGenerators;

public class FinancialDataGenerator(int? seed = null)
{
    readonly private RandomDataGenerator RandomData = new(seed);

    public OHLC[] RandomWalk(int count)
    {
        double scale = 1.0;
        OHLC[] ohlcs = new OHLC[count];

        double open = RandomData.RandomNumber(150 * scale, 250 * scale);

        for (int i = 0; i < count; i++)
        {
            double close = open + RandomData.RandomNumber(-scale, scale);
            double high = Math.Max(open, close) + RandomData.RandomNumber(0, scale);
            double low = Math.Min(open, close) - RandomData.RandomNumber(0, scale);
            ohlcs[i] = new(open, high, low, close);
            open = close + RandomData.RandomNumber(-scale / 2, scale / 2);
        }

        return ohlcs;
    }

    public OHLC[] OHLCsByMinute(int count)
    {
        DateTime start = new(2024, 09, 24, 9, 30, 0);
        TimeSpan delta = TimeSpan.FromMinutes(1);
        DateTime[] dates = Generate.Consecutive(count, start, delta);
        OHLC[] ohlcs = RandomWalk(count);

        for (int i = 0; i < count; i++)
        {
            ohlcs[i] = ohlcs[i].WithDate(dates[i]).WithTimeSpan(delta);
        }

        return ohlcs;
    }
}
