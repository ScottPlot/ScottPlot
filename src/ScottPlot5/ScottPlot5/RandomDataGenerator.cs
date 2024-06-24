using System.Threading;

namespace ScottPlot;

#nullable enable

public class RandomDataGenerator
{
    /// <summary>
    /// Global random number generator, to ensure each generator will returns different data.
    /// Using ThreadLocal, because Random is not thread safe.
    /// </summary>
    private static readonly ThreadLocal<Random> GlobalRandomThread = new(() => new Random(GetCryptoRandomInt()));

    /// <summary>
    /// To select right random number generator
    /// </summary>
    private Random Rand;

    /// <summary>
    /// Create a random number generator.
    /// The seed is random by default, but could be fixed to the defined value
    /// </summary>
    public RandomDataGenerator(int? seed = null)
    {
        Rand = seed.HasValue
            ? new Random(seed.Value)
            : GlobalRandomThread.Value!;
    }

    public void Seed(int seed)
    {
        Rand = new(seed);
    }

    public static RandomDataGenerator Generate { get; private set; } = new(0);

    private static int GetCryptoRandomInt()
    {
        var RNG = System.Security.Cryptography.RandomNumberGenerator.Create();
        byte[] data = new byte[sizeof(int)];
        RNG.GetBytes(data);
        return BitConverter.ToInt32(data, 0) & (int.MaxValue - 1);
    }

    #region Methods that return single numbers

    /// <summary>
    /// Return a uniformly random number between 0 (inclusive) and 1 (exclusive)
    /// </summary>
    public double RandomNumber()
    {
        return Rand.NextDouble();
    }

    /// <summary>
    /// Return a uniformly random number between 0 (inclusive) and <paramref name="max"/> (exclusive)
    /// </summary>
    public double RandomNumber(double max)
    {
        return Rand.NextDouble() * max;
    }

    /// <summary>
    /// Return a uniformly random number between <paramref name="min"/> (inclusive) and <paramref name="max"/> (exclusive)
    /// </summary>
    public double RandomNumber(double min, double max)
    {
        return Rand.NextDouble() * (max - min) + min;
    }

    /// <summary>
    /// Return a random number guaranteed not to be zero
    /// </summary>
    public double RandomNonZeroNumber(double max = 1)
    {
        double randomValue = RandomNumber(max);
        return randomValue != 0
            ? randomValue
            : RandomNonZeroNumber();
    }

    /// <summary>
    /// Return a random integer up to the maximum integer size
    /// </summary>
    public int RandomInteger()
    {
        return Rand.Next();
    }

    /// <summary>
    /// Return a random byte (0-255)
    /// </summary>
    public byte RandomByte()
    {
        return (byte)Rand.Next(256);
    }

    /// <summary>
    /// Return a random byte between the given values (inclusive)
    /// </summary>
    public byte RandomByte(byte min, byte max)
    {
        return (byte)Rand.Next(min, max + 1);
    }

    /// <summary>
    /// Return a random integer between zero (inclusive) and <paramref name="max"/> (exclusive)
    /// </summary>
    public int RandomInteger(int max)
    {
        return Rand.Next(max);
    }

    /// <summary>
    /// Return a random integer between <paramref name="min"/> (inclusive) and <paramref name="max"/> (exclusive)
    /// </summary>
    public int RandomInteger(int min, int max)
    {
        return Rand.Next(min, max);
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
    /// Mutate the given array by adding noise (± magnitude) and return it
    /// </summary>
    public void AddNoiseInPlace(double[] values, double magnitude)
    {
        for (int i = 0; i < values.Length; i++)
        {
            double noise = (2 * RandomNumber() - .5) * magnitude;
            values[i] = values[i] + noise;
        }
    }

    /// <summary>
    /// Return a copy of the given data with random noise added (± magnitude)
    /// </summary>
    public double[] AddNoise(double[] input, double magnitude)
    {
        double[] output = new double[input.Length];
        Array.Copy(input, output, input.Length);
        AddNoiseInPlace(output, magnitude);
        return output;
    }

    /// <summary>
    /// Uniformly distributed random numbers between 0 and 1
    /// (multiplied by <paramref name="mult"/> then added to <paramref name="offset"/>).
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
        for (int i = 0; i < count; i++)
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
        return ScottPlot.Generate.Sin(count, mult, offset, oscillations, phase);
    }

    /// <summary>
    /// A sequence of numbers that starts at <paramref name="offset"/> 
    /// and "walks" randomly from one point to the next, scaled by <paramref name="mult"/>
    /// with an approximate slope of <paramref name="slope"/>.
    /// </summary>
    public double[] RandomWalk(int count, double mult = 1, double offset = 0, double slope = 0)
    {
        double[] data = new double[count];
        data[0] = offset;
        for (int i = 1; i < data.Length; i++)
        {
            // RandomSample number between -1 and 1;
            double randomStep = Rand.NextDouble() * 2 - 1;
            // Using linear equation y_2 = m * x + y_1 where x = 1,
            // then adding a scaled random step simplifies to:
            data[i] = slope + data[i - 1] + randomStep * mult;
        }
        return data;
    }

    /// <summary>
    /// Return a collection OHLCs representing random price action
    /// </summary>
    public List<OHLC> RandomOHLCs(int count)
    {
        return RandomOHLCs(count, new DateTime(2024, 1, 1));
    }

    /// <summary>
    /// Return a collection OHLCs representing random price action
    /// </summary>
    public List<OHLC> RandomOHLCs(int count, DateTime start)
    {
        DateTime[] dates = ScottPlot.Generate.ConsecutiveWeekdays(count, start);
        TimeSpan span = TimeSpan.FromDays(1);

        double mult = 1;

        List<OHLC> ohlcs = new();
        double open = RandomNumber(150, 250);
        for (int i = 0; i < count; i++)
        {
            double close = open + RandomNumber(-mult, mult);
            double high = Math.Max(open, close) + RandomNumber(0, mult);
            double low = Math.Min(open, close) - RandomNumber(0, mult);
            OHLC ohlc = new(open, high, low, close, dates[i], span);
            ohlcs.Add(ohlc);
            open = close + RandomNumber(-mult / 2, mult / 2);
        }

        return ohlcs;
    }
}
