using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot
{
    /// <summary>
    /// Tools to create data (usually arrays of doubles) from nothing
    /// </summary>
    public class Generate
    {

        private static System.Random rand = new System.Random();

        /// <summary>
        /// Create a sequence of evenly spaced numbers
        /// </summary>
        public static double[] Sequence(int count, double scale=1, double offset=0)
        {
            double[] data = new double[count];
            for(int i = 0; i < count; i++)
            {
                data[i] = i*scale+offset;
            }
            return data;
        }

        /// <summary>
        /// Add some salt (randomness) to an array.
        /// </summary>
        public static double[] Salt(double[] Xs, double mult=1)
        {
            for(int i=0; i < Xs.Length; i++)
            {
                Xs[i] = Xs[i] + (rand.NextDouble()-.5) * mult;
            }
            return Xs;
        }

        /// <summary>
        /// Create a sequence of random numbers (from 0 to "scale")
        /// </summary>
        public static double[] Random(int count, double scale=1, double offset = 0)
        {
            double[] data = new double[count];
            for (int i = 0; i < count; i++)
            {
                data[i] = rand.NextDouble() * scale + offset;
            }
            return data;
        }

        /// <summary>
        /// Create a sine wave
        /// </summary>
        public static double[] Sine(int count, double cycles = 1, double scale = 1, double offset = 0)
        {
            double[] data = new double[count];
            double step = cycles * 2 * Math.PI / data.Length;
            for (int i = 0; i < count; i++)
            {
                data[i] = Math.Sin(i * step) * scale + offset;
            }
            return data;
        }
    }
}
