using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlotTests.Statistics
{
    class NthOrderStatistics
    {
        [Test]
        public void MatchesSortedOrder()
        {
            Random rand = new Random(0);

            const int n = 5000;
            double[] values = Enumerable.Range(0, n).Select(_ => rand.NextDouble()).ToArray();
            double[] sorted_values = new double[n];
            values.CopyTo(sorted_values, 0);
            Array.Sort(sorted_values);

            for (int i = 0; i < n; i++)
            {
                Assert.AreEqual(sorted_values[i], ScottPlot.Statistics.Common.NthOrderStatistic(values, i));
            }
        }
    }
}
