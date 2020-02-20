using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    public static class DataGen
    {
        public static double[] Consecutive(int pointCount, double spacing = 1, double offset = 0)
        {
            double[] ys = new double[pointCount];
            for (int i = 0; i < ys.Length; i++)
                ys[i] = i * spacing + offset;
            return ys;
        }

        public static double[] Sin(int pointCount, double oscillations = 1, double offset = 0, double mult = 1, double phase = 0)
        {
            double sinScale = 2 * Math.PI * oscillations / (pointCount - 1);
            double[] ys = new double[pointCount];
            for (int i = 0; i < ys.Length; i++)
                ys[i] = Math.Sin(i * sinScale + phase * Math.PI * 2) * mult + offset;
            return ys;
        }

        public static double[] SinSweep(int pointCount, double density = 50.0)
        {
            double[] data = new double[pointCount];
            for (int i = 0; i < data.Length; i++)
            {
                double t = (double)i / pointCount * density;
                double tSquared = Math.Pow(t, 2);
                data[i] = Math.Sin(tSquared);
            }
            return data;
        }

        public static byte[] SinSweepByte(int pointCount, double density = 50.0)
        {
            byte[] data = new byte[pointCount];
            for (int i = 0; i < data.Length; i++)
            {
                double t = (double)i / pointCount * density;
                double tSquared = Math.Pow(t, 2);
                data[i] = (byte)(Math.Sin(tSquared) * 120 + 128);
            }
            return data;
        }

        public static double[] Cos(int pointCount, double oscillations = 1, double offset = 0, double mult = 1, double phase = 0)
        {
            double sinScale = 2 * Math.PI * oscillations / (pointCount - 1);
            double[] ys = new double[pointCount];
            for (int i = 0; i < ys.Length; i++)
                ys[i] = Math.Cos(i * sinScale + phase * Math.PI * 2) * mult + offset;
            return ys;
        }

        public static double[] Random(Random rand, int pointCount, double multiplier = 1, double offset = 0)
        {
            if (rand is null)
                rand = new Random();
            double[] ys = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
                ys[i] = rand.NextDouble() * multiplier + offset;
            return ys;
        }

        public static int[] RandomInts(Random rand, int pointCount, double multiplier = 1, double offset = 0)
        {
            if (rand is null)
                rand = new Random();
            int[] ys = new int[pointCount];
            for (int i = 0; i < pointCount; i++)
                ys[i] = (int)(rand.NextDouble() * multiplier + offset);
            return ys;
        }

        private static double RandomNormalValue(Random rand, double mean = 0, double stdDev = 1)
        {
            double u1 = 1.0 - rand.NextDouble();
            double u2 = 1.0 - rand.NextDouble();
            double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
            return mean + stdDev * randStdNormal;
        }

        public static double[] RandomNormal(Random rand, int pointCount, double mean = .5, double stdDev = .5)
        {
            if (rand == null)
                rand = new Random();
            double[] values = new double[pointCount];
            for (int i = 0; i < values.Length; i++)
                values[i] = RandomNormalValue(rand, mean, stdDev);

            return values;
        }

        public static double[] NoisyLinear(Random rand, int pointCount = 100, double slope = 1, double offset = 0, double noise = 0.1)
        {
            if (rand is null)
                rand = new Random();

            double[] data = new double[pointCount];
            for (int i = 0; i < data.Length; i++)
                data[i] = slope * i + offset + RandomNormalValue(rand, 0, noise);

            return data;
        }

        public static double[] NoisySin(Random rand, int pointCount, double oscillations = 1, double noiseLevel = .5)
        {
            if (rand is null)
                rand = new Random();
            double[] values = Sin(pointCount, oscillations);
            for (int i = 0; i < values.Length; i++)
                values[i] += rand.NextDouble() * noiseLevel;
            return values;
        }

        public static Color RandomColor(Random rand, int min = 0, int max = 255)
        {
            if (rand is null)
                rand = new Random();
            int r = rand.Next(min, max);
            int g = rand.Next(min, max);
            int b = rand.Next(min, max);
            return Color.FromArgb(r, g, b);
        }
        public static double[] RandomWalk(Random rand, int pointCount, double mult = 1, double offset = 0)
        {
            if (rand is null)
                rand = new Random();
            var data = new double[pointCount];
            data[0] = offset;
            for (int i = 1; i < data.Length; i++)
                data[i] = data[i - 1] + (rand.NextDouble() * 2 - 1) * mult;
            double maxVal = data.Max();
            double minVal = data.Min();
            double span = maxVal - minVal;
            return data;
        }

        public static OHLC[] RandomStockPrices(Random rand, int pointCount, double mult = 10, double startingPrice = 123.45, int deltaMinutes = 0, int deltaDays = 1)
        {
            if (rand is null)
                rand = new Random(0);

            double[] basePrices = ScottPlot.DataGen.RandomWalk(rand, pointCount, mult, startingPrice);

            OHLC[] ohlcs = new OHLC[pointCount];

            DateTime dt = new DateTime(1985, 9, 24, 9, 30, 0);

            for (int i = 0; i < ohlcs.Length; i++)
            {
                double open = rand.NextDouble() * 10 + 50;
                double close = rand.NextDouble() * 10 + 50;
                double high = Math.Max(open, close) + rand.NextDouble() * 10;
                double low = Math.Min(open, close) - rand.NextDouble() * 10;

                // offset prices by randomwalk
                open += basePrices[i];
                close += basePrices[i];
                high += basePrices[i];
                low += basePrices[i];

                if (deltaMinutes > 0)
                {
                    dt = dt.AddMinutes(deltaMinutes);
                }
                else if (deltaDays > 0)
                {
                    dt = dt.AddDays(deltaDays);
                    while ((dt.DayOfWeek == DayOfWeek.Saturday) || (dt.DayOfWeek == DayOfWeek.Sunday))
                        dt = dt.AddDays(1);
                }

                ohlcs[i] = new ScottPlot.OHLC(open, high, low, close, dt);
            }

            return ohlcs;
        }

        public static (double, double) RandomSpan(Random rand = null, double low = 0, double high = 100, double minimumSpacing = 10)
        {
            if (rand is null)
                rand = new Random();

            double span = Math.Abs(high - low);

            for (int attempts = 0; attempts < 10_000; attempts++)
            {
                double valA = rand.NextDouble() * span + low;
                double valB = rand.NextDouble() * span + low;
                if (Math.Abs(valA - valB) >= minimumSpacing)
                {
                    if (valA < valB)
                        return (valA, valB);
                    else
                        return (valB, valA);
                }
            }

            throw new ArgumentException();
        }

        public static double[] Range(int stop)
        {
            return Range(0, stop, 1);
        }

        public static double[] Range(int start, int stop)
        {
            return Range(start, stop, 1);
        }

        public static double[] Range(double start, double stop, double step)
        {
            if (step <= 0)
                throw new ArgumentException("step must be >0. To make a descending series make stop < start.");

            double valueSpan = Math.Abs(start - stop);
            int valueCount = (int)(valueSpan / step);
            double stepSize = (stop > start) ? step : -step;
            double[] values = new double[valueCount];

            for (int i = 0; i < valueCount; i++)
                values[i] = start + i * stepSize;

            return values;
        }
    }
}
