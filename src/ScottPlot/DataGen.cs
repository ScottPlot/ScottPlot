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

        public static double[] Sin(int pointCount, double oscillations = 1, double offset = 0,  double mult = 1, double phase = 0)
        {
            double sinScale = 2 * Math.PI * oscillations / pointCount;
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

        public static double[] Cos(int pointCount, double oscillations = 1)
        {
            double sinScale = 2 * Math.PI * oscillations / pointCount;
            double[] ys = new double[pointCount];
            for (int i = 0; i < ys.Length; i++)
                ys[i] = Math.Cos(i * sinScale);
            return ys;
        }

        public static double[] Random(Random rand, int pointCount, double multiplier = 1, double offset = 0)
        {
            double[] ys = new double[pointCount];
            for (int i = 0; i < pointCount; i++)
                ys[i] = rand.NextDouble() * multiplier + offset;
            return ys;
        }

        public static double[] RandomNormal(Random rand, int pointCount, double mean = .5, double stdDev = .5)
        {
            if (rand == null)
                rand = new Random();
            double[] values = new double[pointCount];
            for (int i = 0; i < values.Length; i++)
            {
                double u1 = 1.0 - rand.NextDouble();
                double u2 = 1.0 - rand.NextDouble();
                double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
                double randNormal = mean + stdDev * randStdNormal;
                values[i] = randNormal;
            }
            return values;
        }

        public static Color RandomColor(Random rand, int min = 0, int max = 255)
        {
            int r = rand.Next(min, max);
            int g = rand.Next(min, max);
            int b = rand.Next(min, max);
            return Color.FromArgb(r, g, b);
        }
        public static double[] RandomWalk(Random rand, int pointCount, double mult = 1, double offset = 0)
        {
            var data = new double[pointCount];
            data[0] = offset;
            for (int i = 1; i < data.Length; i++)
                data[i] = data[i - 1] + (rand.NextDouble() * 2 - 1) * mult;
            double maxVal = data.Max();
            double minVal = data.Min();
            double span = maxVal - minVal;
            return data;
        }

        public static OHLC[] RandomStockPrices(Random rand, int pointCount, double mult = 10, double startingPrice = 123.45)
        {

            double[] basePrices = ScottPlot.DataGen.RandomWalk(rand, pointCount, mult, startingPrice);

            OHLC[] ohlcs = new OHLC[pointCount];

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

                ohlcs[i] = new ScottPlot.OHLC(open, high, low, close, i);
            }

            return ohlcs;
        }
    }
}
