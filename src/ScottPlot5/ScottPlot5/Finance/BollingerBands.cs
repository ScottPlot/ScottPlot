using ScottPlot.Statistics;

namespace ScottPlot.Finance;

public class BollingerBands
{
    public readonly double[] Means;
    public readonly double[] UpperValues;
    public readonly double[] LowerValues;
    public readonly DateTime[] DateTimes;
    public readonly double[] Dates;

    public BollingerBands(List<OHLC> ohlcs, int N, double sdCoeff = 2)
    {
        double[] prices = ohlcs.Select(x => x.Close).ToArray();
        double[] sma = Series.MovingAverage(prices, N, preserveLength: true);
        double[] smstd = Series.SimpleMovingStandardDeviation(prices, N, preserveLength: true);

        UpperValues = new double[prices.Length];
        LowerValues = new double[prices.Length];
        for (int i = 0; i < prices.Length; i++)
        {
            LowerValues[i] = sma[i] - sdCoeff * smstd[i];
            UpperValues[i] = sma[i] + sdCoeff * smstd[i];
        }

        // skip the first points which all contain NaN
        Means = sma.Skip(N).ToArray();
        LowerValues = LowerValues.Skip(N).ToArray();
        UpperValues = UpperValues.Skip(N).ToArray();
        DateTimes = ohlcs.Skip(N).Select(x => x.DateTime).ToArray();
        Dates = DateTimes.Select(x => x.ToOADate()).ToArray();
    }
}
