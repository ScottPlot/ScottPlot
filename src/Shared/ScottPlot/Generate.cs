using System;
using System.Linq;

namespace ScottPlot;

#nullable enable

/// <summary>
/// This class contains methods which generate sample data for testing and demonstration purposes
/// </summary>
public static class Generate
{
    #region numerical 1D

    /// <summary>
    /// Return an array of evenly-spaced numbers
    /// </summary>
    public static double[] Consecutive(int count, double delta = 1, double first = 0)
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
    public static double[] Sin(int count, double mult = 1, double offset = 0, double oscillations = 1, double phase = 0)
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
    public static double[] Cos(int count, double mult = 1, double offset = 0, double oscillations = 1, double phase = 0)
    {
        double sinScale = 2 * Math.PI * oscillations / (count - 1);
        double[] ys = new double[count];
        for (int i = 0; i < ys.Length; i++)
            ys[i] = Math.Cos(i * sinScale + phase * Math.PI * 2) * mult + offset;
        return ys;
    }

    public static double[] NoisySin(Random rand, int count, double noiseLevel = 1)
    {
        double[] data = Sin(count);
        for (int i = 0; i < data.Length; i++)
        {
            data[i] += rand.NextDouble() * noiseLevel;
        }
        return data;
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

    #endregion

    #region numerical random

    /// <summary>
    /// Return a series of values starting with <paramref name="offset"/> and
    /// each randomly deviating from the previous by at most <paramref name="mult"/>.
    /// Random values are deterministic based on the value of <paramref name="seed"/>.
    /// </summary>
    public static double[] RandomWalk(int count, double mult = 1, double offset = 0, int seed = 0)
    {
        RandomDataGenerator gen = new(seed);
        return gen.RandomWalk(count, mult, offset);
    }

    /// <summary>
    /// Return an array of <paramref name="count"/> random values 
    /// from <paramref name="min"/> to <paramref name="max"/>
    /// according to the random seed defined by <paramref name="seed"/>
    /// </summary>
    public static double[] Random(int count, double min = 0, double max = 1, int seed = 0)
    {
        RandomDataGenerator gen = new(seed);
        return Enumerable.Range(0, count)
            .Select(_ => gen.RandomNumber(min, max))
            .ToArray();
    }

    #endregion

    #region DateTime

    /// <summary>
    /// Contains methods for generating DateTime sequences
    /// </summary>
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
}
