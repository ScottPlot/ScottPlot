using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot
{
    /// <summary>
    /// This class generates data for testing
    /// </summary>
    public static class Generate
    {
        public static double[] Sin(int pointCount, double oscillations = 1, double offset = 0, double mult = 1, double phase = 0)
        {
            if (pointCount < 1)
                throw new ArgumentException("must have at least 1 point");

            double[] ys = new double[pointCount];
            double sinScale = 2 * Math.PI * oscillations / (pointCount - 1);
            for (int i = 0; i < ys.Length; i++)
                ys[i] = Math.Sin(i * sinScale + phase * Math.PI * 2) * mult + offset;
            return ys;
        }

        public static double[] Cos(int pointCount, double oscillations = 1, double offset = 0, double mult = 1, double phase = 0)
        {
            if (pointCount < 1)
                throw new ArgumentException("must have at least 1 point");

            double[] ys = new double[pointCount];
            double sinScale = 2 * Math.PI * oscillations / (pointCount - 1);
            for (int i = 0; i < ys.Length; i++)
                ys[i] = Math.Cos(i * sinScale + phase * Math.PI * 2) * mult + offset;
            return ys;
        }

        public static double[] Consecutive(int pointCount, double spacing = 1, double offset = 0)
        {
            if (pointCount < 1)
                throw new ArgumentException("must have at least 1 point");

            double[] ys = new double[pointCount];
            for (int i = 0; i < ys.Length; i++)
                ys[i] = i * spacing + offset;
            return ys;
        }

        public static double[] RandomWalk(Random rand, int pointCount, double mult = 1, double offset = 0)
        {
            if (pointCount < 1)
                throw new ArgumentException("must have at least 1 point");

            if (rand is null)
                rand = new Random();
            var data = new double[pointCount];
            data[0] = offset;
            for (int i = 1; i < data.Length; i++)
                data[i] = data[i - 1] + (rand.NextDouble() * 2 - 1) * mult;
            return data;
        }
    }
}
