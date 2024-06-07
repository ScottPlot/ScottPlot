namespace ScottPlot;

#nullable enable

/// <summary>
/// This class contains methods which generate sample data for testing and demonstration purposes
/// </summary>
public static class Generate
{
    public static RandomDataGenerator RandomData { get; } = new(0);

    #region numerical 1D

    /// <summary>
    /// Return an array of evenly-spaced numbers
    /// </summary>
    public static double[] Consecutive(int count = 51, double delta = 1, double first = 0)
    {
        double[] ys = new double[count];
        for (int i = 0; i < ys.Length; i++)
            ys[i] = i * delta + first;
        return ys;
    }

    /// <summary>
    /// Return an array of sine waves between -1 and 1.
    /// Values are multiplied by <paramref name="mult"/> then shifted by <paramref name="offset"/>.
    /// Phase shifts the sine wave horizontally between 0 and 2 Pi.
    /// </summary>
    public static double[] Sin(int count = 51, double mult = 1, double offset = 0, double oscillations = 1, double phase = 0)
    {
        double sinScale = 2 * Math.PI * oscillations / (count - 1);
        double[] ys = new double[count];
        for (int i = 0; i < ys.Length; i++)
            ys[i] = Math.Sin(i * sinScale + phase * Math.PI * 2) * mult + offset;
        return ys;
    }

    /// <summary>
    /// Return an array of cosine waves between -1 and 1.
    /// Values are multiplied by <paramref name="mult"/> then shifted by <paramref name="offset"/>.
    /// Phase shifts the sine wave horizontally between 0 and 2 Pi.
    /// </summary>
    public static double[] Cos(int count = 51, double mult = 1, double offset = 0, double oscillations = 1, double phase = 0)
    {
        double sinScale = 2 * Math.PI * oscillations / (count - 1);
        double[] ys = new double[count];
        for (int i = 0; i < ys.Length; i++)
            ys[i] = Math.Cos(i * sinScale + phase * Math.PI * 2) * mult + offset;
        return ys;
    }

    [Obsolete("use Generate.Sin() then Generate.AddNoise()")]
    public static double[] NoisySin(int count = 51, double magnitude = 1)
    {
        double[] sin = Sin(count);
        AddNoiseInPlace(sin, magnitude);
        return sin;
    }

    /// <summary>
    /// An exponential curve that rises from 0 to <paramref name="mult"/> 
    /// over <paramref name="count"/> points with random positive noise that scales
    /// with the underlying signal. The rate constant of the exponential
    /// curve is <paramref name="tau"/>.
    /// </summary>
    public static double[] NoisyExponential(int count, double mult = 1000, double noise = .5, double tau = 5)
    {
        double[] values = new double[count];

        // add this to ensure we never actually return 0, so Log10() never returns infinity
        double offset = mult * .001;

        for (int i = 0; i < values.Length; i++)
        {
            double x = (double)i / (count - 1);
            double y = (Math.Exp(tau * x) - 1) / Math.Exp(tau); // 0 to 1
            y *= mult;
            y += Math.Abs(RandomData.RandomNormalNumber()) * noise * y;
            y += offset;
            values[i] = y;
        }

        return values;
    }

    public static double[] SquareWave(uint cycles = 20, uint pointsPerCycle = 1_000, double duty = .5, double low = 0, double high = 1)
    {
        if (duty < 0 || duty > 1)
            throw new ArgumentException($"{nameof(duty)} must be in the range [0, 1]");

        uint points = cycles * pointsPerCycle;
        uint cyclePointsHigh = (uint)(pointsPerCycle * duty);
        uint cyclePointsLow = pointsPerCycle - cyclePointsHigh;

        double[] values = new double[points];

        uint i = 0;

        for (int c = 0; c < cycles; c++)
        {
            for (int p = 0; p < cyclePointsLow; p++)
                values[i++] = low;

            for (int p = 0; p < cyclePointsHigh; p++)
                values[i++] = high;
        }

        return values;
    }

    public static double[] Zeros(int count)
    {
        return Repeating(count, 0);
    }

    public static double[] Ones(int count)
    {
        return Repeating(count, 1);
    }

    public static double[] Repeating(int count, double value)
    {
        double[] values = new double[count];

        for (int i = 0; i < values.Length; i++)
        {
            values[i] = value;
        }

        return values;
    }

    public static double[] NaN(int count)
    {
        return Repeating(count, double.NaN);
    }

    #endregion

    #region numerical 2D

    /// <summary>
    /// Generates a 2D array of numbers with constant spacing.
    /// </summary>
    /// <param name="rows"></param>
    /// <param name="columns"></param>
    /// <param name="spacing">The space between points.</param>
    /// <param name="offset">The first point.</param>
    /// <returns></returns>
    public static double[,] Consecutive2D(int rows, int columns, double spacing = 1, double offset = 0)
    {
        double[,] data = new double[rows, columns];

        var count = offset;
        for (var y = 0; y < data.GetLength(0); y++)
            for (int x = 0; x < data.GetLength(1); x++)
            {
                data[y, x] = count;
                count += spacing;
            }

        return data;
    }

    /// <summary>
    /// Generates a 2D sine pattern.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="xPeriod">Frequency factor in x direction.</param>
    /// <param name="yPeriod">Frequency factor in y direction.</param>
    /// <param name="multiple">Intensity factor.</param>
    public static double[,] Sin2D(int width, int height, double xPeriod = .2, double yPeriod = .2, double multiple = 100)
    {
        double[,] intensities = new double[height, width];

        for (int y = 0; y < height; y++)
        {
            double siny = Math.Cos(y * yPeriod) * multiple;
            for (int x = 0; x < width; x++)
            {
                double sinx = Math.Sin(x * xPeriod) * multiple;
                intensities[y, x] = sinx + siny;
            }
        }

        return intensities;
    }

    /// <summary>
    /// Generate a 2D array in a diagonal gradient pattern
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static double[,] Ramp2D(int width, int height, double min = 0, double max = 1)
    {
        double[,] intensities = new double[height, width];

        double span = max - min;

        for (int y = 0; y < height; y++)
        {
            double fracY = (double)y / height;
            double valY = fracY * span + min;

            for (int x = 0; x < width; x++)
            {
                double fracX = (double)x / width;
                double valX = fracX * span + min;

                intensities[y, x] = (valX + valY) / 2;
            }
        }

        return intensities;
    }

    public static RootedCoordinateVector[] SampleVectors(int columns = 10, int rows = 10, double oscillations = 1)
    {
        double[] xs = Consecutive(columns);
        double[] ys = Consecutive(rows);

        List<RootedCoordinateVector> vectors = new();
        for (int i = 0; i < xs.Length; i++)
        {
            for (int j = 0; j < ys.Length; j++)
            {
                double x = (double)i / xs.Length * Math.PI * oscillations;
                double y = (double)j / xs.Length * Math.PI * oscillations;
                double dX = Math.Sin(x) + Math.Sin(y);
                double dY = Math.Sin(x) - Math.Sin(y);
                System.Numerics.Vector2 v = new((float)dX, (float)dY);
                Coordinates pt = new(xs[i], ys[j]);
                RootedCoordinateVector vector = new(pt, v);
                vectors.Add(vector);
            }
        }

        return vectors.ToArray();
    }

    #endregion

    #region numerical random

    /// <summary>
    /// Return a series of values starting with <paramref name="offset"/> and
    /// each randomly deviating from the previous by at most <paramref name="mult"/>.
    /// </summary>
    public static double[] RandomWalk(int count, double mult = 1, double offset = 0)
    {
        return RandomData.RandomWalk(count, mult, offset);
    }

    [Obsolete("use RandomSample()")]
    public static double[] Random(int count, double min = 0, double max = 1)
    {
        return RandomSample(count, min, max);
    }

    /// <summary>
    /// Return an array of <paramref name="count"/> random values 
    /// from <paramref name="min"/> to <paramref name="max"/>
    /// </summary>
    public static double[] RandomSample(int count, double min = 0, double max = 1)
    {
        return Enumerable.Range(0, count)
            .Select(_ => RandomData.RandomNumber(min, max))
            .ToArray();
    }

    /// <summary>
    /// Sequence of ascending numbers with random spacing between values.
    /// </summary>
    public static double[] RandomAscending(int count, double minDelta = 0, double maxDelta = 1)
    {
        double[] values = RandomSample(count, minDelta, maxDelta);

        for (int i = 1; i < count; i++)
        {
            values[i] += values[i - 1];
        }

        return values;
    }

    public static double[] RandomNormal(int count, double mean = 0, double stdDev = 1)
    {
        return Enumerable.Range(0, count)
            .Select(_ => RandomData.RandomNormalNumber(mean, stdDev))
            .ToArray();
    }

    /// <summary>
    /// RandomSample integer between zero (inclusive) and <paramref name="max"/> (exclusive)
    /// </summary>
    public static int RandomInteger(int max)
    {
        return RandomData.RandomInteger(max);
    }

    /// <summary>
    /// RandomSample integer between <paramref name="min"/> (inclusive) and <paramref name="max"/> (exclusive)
    /// </summary>
    public static int RandomInteger(int min, int max)
    {
        return RandomData.RandomInteger(min, max);
    }

    /// <summary>
    /// RandomSample integers between zero (inclusive) and <paramref name="max"/> (exclusive)
    /// </summary>
    public static int[] RandomIntegers(int count, int max)
    {
        return Enumerable
            .Range(0, count)
            .Select(x => RandomData.RandomInteger(max))
            .ToArray();
    }

    /// <summary>
    /// RandomSample integers between <paramref name="min"/> (inclusive) and <paramref name="max"/> (exclusive)
    /// </summary>
    public static int[] RandomIntegers(int count, int min, int max)
    {
        return Enumerable
            .Range(0, count)
            .Select(x => RandomData.RandomInteger(min, max))
            .ToArray();
    }

    /// <summary>
    /// RandomSample integer between 0 (inclusive) and 1 (exclusive)
    /// </summary>
    public static double RandomNumber()
    {
        return RandomData.RandomNumber(0, 1);
    }

    /// <summary>
    /// RandomSample integer between 0 (inclusive) and <paramref name="max"/> (exclusive)
    /// </summary>
    public static double RandomNumber(double max)
    {
        return RandomData.RandomNumber(0, max);
    }

    /// <summary>
    /// RandomSample integer between <paramref name="min"/> (inclusive) and <paramref name="max"/> (exclusive)
    /// </summary>
    public static double RandomNumber(double min, double max)
    {
        return RandomData.RandomNumber(min, max);
    }

    /// <summary>
    /// Random numbers between zero (inclusive) and <paramref name="max"/> (exclusive)
    /// </summary>
    public static double[] RandomNumbers(int count, double max)
    {
        return RandomData.RandomSample(count, 0, max);
    }

    /// <summary>
    /// Random numbers between <paramref name="min"/> (inclusive) and <paramref name="max"/> (exclusive)
    /// </summary>
    public static double[] RandomNumbers(int count, double min, double max)
    {
        return RandomData.RandomSample(count, min, max);
    }

    /// <summary>
    /// Return a copy of the given array with random values added to each point
    /// </summary>
    public static double[] AddNoise(double[] input, double magnitude = 1)
    {
        return RandomData.AddNoise(input, magnitude);
    }

    /// <summary>
    /// Mutate the given array by adding a random value to each point
    /// </summary>
    public static void AddNoiseInPlace(double[] values, double magnitude = 1)
    {
        RandomData.AddNoiseInPlace(values, magnitude);
    }

    #endregion

    #region text

    public static char RandomChar()
    {
        return (char)('A' + RandomInteger(26));
    }

    public static string RandomString(int length)
    {
        char[] chars = new char[length];

        for (int i = 0; i < chars.Length; i++)
        {
            chars[i] = RandomChar();
        }

        return new string(chars);
    }

    #endregion

    #region Axes

    public static Coordinates RandomCoordinates(double xMult = 1, double yMult = 1, double xOffset = 0, double yOffset = 0)
    {
        double x = RandomData.RandomNumber() * xMult + xOffset;
        double y = RandomData.RandomNumber() * yMult + yOffset;
        return new Coordinates(x, y);
    }

    public static Coordinates[] RandomCoordinates(int count, double xMult = 1, double yMult = 1, double xOffset = 0, double yOffset = 0)
    {
        Coordinates[] cs = new Coordinates[count];

        for (int i = 0; i < count; i++)
        {
            cs[i] = RandomCoordinates(xMult, yMult, xOffset, yOffset);
        }

        return cs;
    }

    public static Coordinates RandomLocation()
    {
        AxisLimits limits = new(0, 1, 0, 1);
        return RandomLocation(limits);
    }

    public static Coordinates RandomLocation(AxisLimits limits)
    {
        double x = RandomData.RandomNumber() * limits.HorizontalSpan + limits.Left;
        double y = RandomData.RandomNumber() * limits.VerticalSpan + limits.Bottom;
        return new Coordinates(x, y);
    }

    public static Coordinates[] RandomLocations(int count)
    {
        return Enumerable
            .Range(0, count)
            .Select(x => RandomCoordinates())
            .ToArray();
    }

    public static Coordinates[] RandomLocations(int count, AxisLimits limits)
    {
        return Enumerable
            .Range(0, count)
            .Select(x => RandomLocation(limits))
            .ToArray();
    }

    #endregion

    #region DateTime

    public static System.DateTime[] Consecutive(int count, System.DateTime start, TimeSpan timeSpan)
    {
        System.DateTime[] dates = new System.DateTime[count];

        for (int i = 0; i < count; i++)
        {
            dates[i] = start;
            start += timeSpan;
        }

        return dates;
    }

    public static System.DateTime[] ConsecutiveDateTimes(int count, System.DateTime start, TimeSpan timeSpan)
    {
        return Consecutive(count, start, timeSpan);
    }

    public static System.DateTime[] ConsecutiveDays(int count, int year = 2023, int month = 1, int day = 1)
    {
        return Consecutive(count, new(year, month, day), TimeSpan.FromDays(1));
    }

    public static System.DateTime[] ConsecutiveDays(int count, System.DateTime start)
    {
        return Consecutive(count, start, TimeSpan.FromDays(1));
    }

    public static System.DateTime[] ConsecutiveWeekdays(int count, int year = 2023, int month = 1, int day = 1)
    {
        return ConsecutiveWeekdays(count, new(year, month, day));
    }

    public static System.DateTime[] ConsecutiveWeekdays(int count, System.DateTime start)
    {
        System.DateTime[] dates = new System.DateTime[count];
        TimeSpan step = TimeSpan.FromDays(1);
        int i = 0;
        while (i < count)
        {
            while (start.DayOfWeek == DayOfWeek.Saturday || start.DayOfWeek == DayOfWeek.Sunday)
                start += step;
            dates[i++] = start;
            start += step;
        }
        return dates;
    }

    public static System.DateTime[] ConsecutiveHours(int count)
    {
        var start = new System.DateTime(2023, 01, 01);
        return ConsecutiveHours(count, start);
    }

    public static System.DateTime[] ConsecutiveHours(int count, System.DateTime start)
    {
        return Consecutive(count, start, TimeSpan.FromHours(1));
    }

    public static System.DateTime[] ConsecutiveQuarterHours(int count, System.DateTime start)
    {
        return Consecutive(count, start, TimeSpan.FromMinutes(15));
    }

    public static System.DateTime[] ConsecutiveMinutes(int count, System.DateTime start)
    {
        return Consecutive(count, start, TimeSpan.FromMinutes(1));
    }

    public static System.DateTime[] ConsecutiveSeconds(int count, System.DateTime start)
    {
        return Consecutive(count, start, TimeSpan.FromSeconds(1));
    }

    [Obsolete("use ConsecutiveDays(), ConsecutiveHours(), ConsecutiveMinutes(), etc.", true)]
    public static class DateTime
    {
        /// <summary>
        /// Date of the first ScottPlot commit
        /// </summary>
        public static readonly System.DateTime ExampleDate = new(2018, 01, 03);

        /// <summary>
        /// Evenly-spaced DateTimes
        /// </summary>
        public static System.DateTime[] Consecutive(int count, System.DateTime start, TimeSpan step)
        {
            System.DateTime dt = start;
            System.DateTime[] values = new System.DateTime[count];
            for (int i = 0; i < count; i++)
            {
                values[i] = dt;
                dt += step;
            }
            return values;
        }

        public static System.DateTime[] Weekdays(int count, System.DateTime start)
        {
            System.DateTime[] dates = new System.DateTime[count];
            int i = 0;
            while (i < count)
            {
                if (start.DayOfWeek != DayOfWeek.Saturday && start.DayOfWeek != DayOfWeek.Sunday)
                {
                    dates[i] = start;
                    i++;
                }

                start = start.AddDays(1);
            }
            return dates;
        }

        public static System.DateTime[] Weekdays(int count) => Weekdays(count, ExampleDate);

        public static System.DateTime[] Days(int count, System.DateTime start) => Consecutive(count, start, TimeSpan.FromDays(1));

        public static System.DateTime[] Days(int count) => Days(count, ExampleDate);

        public static System.DateTime[] Hours(int count, System.DateTime start) => Consecutive(count, start, TimeSpan.FromHours(1));

        public static System.DateTime[] Hours(int count) => Hours(count, ExampleDate);

        public static System.DateTime[] Minutes(int count, System.DateTime start) => Consecutive(count, start, TimeSpan.FromMinutes(1));

        public static System.DateTime[] Minutes(int count) => Hours(count, ExampleDate);

        public static System.DateTime[] Seconds(int count, System.DateTime start) => Consecutive(count, start, TimeSpan.FromSeconds(1));

        public static System.DateTime[] Seconds(int count) => Hours(count, ExampleDate);
    }

    #endregion

    #region Finance

    public static OHLC RandomOHLC()
    {
        return RandomData.RandomOHLCs(1).Single();
    }

    public static OHLC RandomOHLC(System.DateTime date)
    {
        OHLC ohlc = RandomOHLC();
        ohlc.DateTime = date;
        return ohlc;
    }

    public static List<OHLC> RandomOHLCs(int count)
    {
        return RandomData.RandomOHLCs(count);
    }

    public static List<OHLC> RandomOHLCs(int count, System.DateTime startDate)
    {
        return RandomData.RandomOHLCs(count);
    }

    #endregion

    #region Plot Items

    public static Box RandomBox(double position)
    {
        int N = 50;
        double mean = RandomData.RandomNumber(3);
        double stdDev = RandomData.RandomNumber(3);

        double[] values = ScottPlot.Generate.RandomNormal(N, mean, stdDev);
        Array.Sort(values);
        double min = values[0];
        double q1 = values[N / 4];
        double median = values[N / 2];
        double q3 = values[3 * N / 4];
        double max = values[N - 1];

        return new Box
        {
            Position = position,
            WhiskerMin = min,
            BoxMin = q1,
            BoxMiddle = median,
            BoxMax = q3,
            WhiskerMax = max,
        };
    }

    public static Color RandomHue() => Color.RandomHue();

    public static Color RandomColor()
    {
        byte r = RandomData.RandomByte();
        byte g = RandomData.RandomByte();
        byte b = RandomData.RandomByte();
        return new Color(r, g, b);
    }

    /// <summary>
    /// Generate a dark color by defining the maximum value to use for R, G, and B
    /// </summary>
    public static Color RandomColor(byte max)
    {
        byte r = (byte)RandomData.RandomInteger(max);
        byte g = (byte)RandomData.RandomInteger(max);
        byte b = (byte)RandomData.RandomInteger(max);
        return new Color(r, g, b);
    }

    public static Color RandomColor(IColormap colormap)
    {
        return colormap.GetColor(RandomData.RandomNumber());
    }

    public static Color[] RandomColors(int count, IColormap colormap)
    {
        return RandomSample(count).Select(colormap.GetColor).ToArray();
    }

    public static MarkerShape RandomMarkerShape()
    {
        MarkerShape[] markerShapes = Enum
            .GetValues(typeof(MarkerShape))
            .Cast<MarkerShape>()
            .ToArray();

        int i = RandomInteger(markerShapes.Length);
        return markerShapes[i];
    }

    public static CoordinateLine RandomLine()
    {
        return new CoordinateLine(RandomLocation(), RandomLocation());
    }

    public static LinePattern RandomLinePattern()
    {
        LinePattern[] linePatterns = Enum
            .GetValues(typeof(LinePattern))
            .Cast<LinePattern>()
            .ToArray();

        int i = RandomInteger(linePatterns.Length);
        return linePatterns[i];
    }

    #endregion

    #region Data Generators

    public readonly static DataGenerators.RandomWalker RandomWalker = new();

    #endregion
}
