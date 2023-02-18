namespace ScottPlot;

public class RandomDataGenerator
{
    readonly Random Rand;

    /// <summary>
    /// Random seed
    /// </summary>
    public RandomDataGenerator()
    {
        Rand = new();
    }

    /// <summary>
    /// Defined seed
    /// </summary>
    public RandomDataGenerator(int seed = 0)
    {
        Rand = new Random(seed);
    }

    public double[] RandomSin(int count)
    {
        double mult = Math.Pow(2, 1 + Rand.NextDouble() * 10);
        double offset = mult * (Rand.NextDouble() - .5);
        double oscillations = 1 + Rand.NextDouble() * 5;
        double phase = Rand.NextDouble() * Math.PI * 2;
        return Generate.Sin(count, mult, offset, oscillations, phase);
    }

    public double RandomNumber(double min, double max)
    {
        double span = max - min;
        return min + Rand.NextDouble() * span;
    }

    public double[] RandomWalk(int pointCount, double mult = 1, double offset = 0)
    {
        double[] data = new double[pointCount];
        data[0] = offset;
        for (int i = 1; i < data.Length; i++)
            data[i] = data[i - 1] + (Rand.NextDouble() * 2 - 1) * mult;
        return data;
    }

    public IEnumerable<OHLC> RandomOHLCs(int count, double mult = 1, double offset = 150)
    {
        double[] openingPrices = RandomWalk(count + 1, mult, offset);

        OHLC GetOHLC(double open, double close)
        {
            double min = Math.Min(open, close);
            double max = Math.Max(open, close);

            double span = max - min;

            double high = RandomNumber(max, max + span * 1.02);
            double low = RandomNumber(min - span * 0.98, min);

            return new OHLC(open, high, low, close);
        }

        return openingPrices
            .Take(count)
            .Zip(openingPrices.Skip(1), GetOHLC);
    }
}
