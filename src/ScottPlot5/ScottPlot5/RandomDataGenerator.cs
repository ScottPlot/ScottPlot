using System.Security.Cryptography;

namespace ScottPlot;

#nullable enable

public class RandomDataGenerator
{
    private Random Rand { get; }
    private RandomNumberGenerator RNG { get; }

    /// <summary>
    /// Use a random seed so each generator returns different data.
    /// </summary>
    public RandomDataGenerator()
    {
        RNG = RandomNumberGenerator.Create();
        byte[] data = new byte[sizeof(int)];
        RNG.GetBytes(data);
        int randomValue = BitConverter.ToInt32(data, 0) & (int.MaxValue - 1);
        Rand = new Random(randomValue);
    }

    /// <summary>
    /// Use a fixed seed so each generator returns the same data.
    /// </summary>
    public RandomDataGenerator(int seed = 0)
    {
        Rand = new(seed);
        RNG = RandomNumberGenerator.Create();
    }

    #region Methods that return single numbers

    /// <summary>
    /// Return a uniformly random number between 0 (inclusive) and the given maximum (exclusive)
    /// </summary>
    public double RandomNumber(double maxValue = 1)
    {
        return Rand.NextDouble() * maxValue;
    }

    /// <summary>
    /// Return a random number guaranteed not to be zero
    /// </summary>
    public double RandomNonZeroNumber(double maxValue = 1)
    {
        double randomValue = RandomNumber(maxValue);
        return randomValue != 0
            ? randomValue
            : RandomNonZeroNumber();
    }

    /// <summary>
    /// Return a uniformly random number between the given values
    /// </summary>
    public double RandomNumberInRange(double minValue, double maxValue)
    {
        double span = maxValue - minValue;
        return minValue + Rand.NextDouble() * span;
    }

    /// <summary>
    /// Return a random integer up to the maximum integer size
    /// </summary>
    public double RandomInteger()
    {
        return Rand.Next();
    }

    /// <summary>
    /// Return a random integer between zero (inclusive) and the given value (exclusive)
    /// </summary>
    public double RandomInteger(int maxValue)
    {
        return Rand.Next(maxValue);
    }

    /// <summary>
    /// Return a number normally distributed around the given <paramref name="mean"/> 
    /// according to the <paramref name="stdDev"/> standard deviation.
    /// </summary>
    public double RandomNormalNumber(double mean = 0, double stdDev = 1)
    {
        double u1 = RandomNonZeroNumber();
        double u2 = RandomNumber();
        double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
        return mean + stdDev * randStdNormal;
    }

    #endregion

    /// <summary>
    /// Uniformly distributed random numbers
    /// </summary>
    public double[] RandomSample(int count, double mult = 1, double offset = 0)
    {
        double[] values = new double[count];
        for (int i = 0; i < count; i++)
            values[i] = Rand.NextDouble() * mult + offset;
        return values;
    }

    /// <summary>
    /// Return a collection of numbers normally distributed around the given <paramref name="mean"/> 
    /// according to the <paramref name="stdDev"/> standard deviation.
    /// </summary>
    public double[] RandomNormalSample(int count, double mean = 0, double stdDev = 1)
    {
        double[] values = new double[count];
        for (int i = 0; i <= count; i++)
        {
            values[i] = RandomNormalNumber(mean, stdDev);
        }
        return values;
    }

    /// <summary>
    /// Sine wave with random frequency, amplitude, and phase
    /// </summary>
    public double[] RandomSin(int count)
    {
        double mult = Math.Pow(2, 1 + Rand.NextDouble() * 10);
        double offset = mult * (Rand.NextDouble() - .5);
        double oscillations = 1 + Rand.NextDouble() * 5;
        double phase = Rand.NextDouble() * Math.PI * 2;
        return Generate.Sin(count, mult, offset, oscillations, phase);
    }

    /// <summary>
    /// A sequence of numbers that starts at <paramref name="offset"/> 
    /// and "walks" randomly from one point to the next, scaled by <paramref name="mult"/>.
    /// </summary>
    public double[] RandomWalk(int count, double mult = 1, double offset = 0)
    {
        double[] data = new double[count];
        data[0] = offset;
        for (int i = 1; i < data.Length; i++)
            data[i] = data[i - 1] + (Rand.NextDouble() * 2 - 1) * mult;
        return data;
    }

    /// <summary>
    /// Return a collection OHLCs representing random price action
    /// </summary>
    public List<IOHLC> RandomOHLCs(int count)
    {
        DateTime[] dates = Generate.DateTime.Weekdays(count);
        TimeSpan span = TimeSpan.FromDays(1);

        double mult = 1;

        List<IOHLC> ohlcs = new();
        double open = RandomNumberInRange(150, 250);
        for (int i = 0; i < count; i++)
        {
            double close = open + RandomNumberInRange(-mult, mult);
            double high = Math.Max(open, close) + RandomNumberInRange(0, mult);
            double low = Math.Min(open, close) - RandomNumberInRange(0, mult);
            OHLC ohlc = new(open, high, low, close, dates[i], span);
            ohlcs.Add(ohlc);
            open = close + RandomNumberInRange(-mult / 2, mult / 2);
        }

        return ohlcs;
    }
}
