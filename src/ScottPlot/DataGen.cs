using System;
using System.Collections.Generic;
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

        public static double[] Sin(int pointCount, double oscillations = 1)
        {
            double sinScale = 2 * Math.PI * oscillations / pointCount;
            double[] ys = new double[pointCount];
            for (int i = 0; i < ys.Length; i++)
                ys[i] = Math.Sin(i * sinScale);
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
    }
}
