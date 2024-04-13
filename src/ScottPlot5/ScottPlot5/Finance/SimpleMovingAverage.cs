using ScottPlot.Statistics;

namespace ScottPlot.Finance;

public class SimpleMovingAverage
{
    public readonly double[] Means;
    public readonly DateTime[] DateTimes;
    public readonly double[] Dates;

    public SimpleMovingAverage(List<OHLC> ohlcs, int N)
    {
        double[] prices = ohlcs.Select(x => x.Close).ToArray();
        Means = Series.MovingAverage(prices, N);
        DateTimes = ohlcs.Skip(N).Select(x => x.DateTime).ToArray();
        Dates = DateTimes.Select(x => x.ToOADate()).ToArray();
    }
}
