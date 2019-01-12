using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlotTests
{
    class DataGenerator
    {
        public static double[] EvenlySpaced(int pointCount, double spacing = 1)
        {
            var data = new double[pointCount];
            for (int i = 0; i < data.Length; i++)
                data[i] = i * spacing;
            return data;
        }

        public static double[] Random(int pointCount, double mult = 1, double offset = 0, int seed = 0)
        {
            Random rand = new Random(seed);
            var data = new double[pointCount];
            for (int i = 0; i < data.Length; i++)
                data[i] = rand.NextDouble() * mult + offset;
            return data;
        }

        public static double[] RandomWalk(int pointCount, double mult = 1, double offset = 0, int seed = 3)
        {
            Random rand = new Random(seed);
            var data = new double[pointCount];
            data[0] = offset;
            for (int i = 1; i < data.Length; i++)
                data[i] = data[i - 1] + (rand.NextDouble() * 2 - 1) * mult;
            return data;
        }
    }
}
