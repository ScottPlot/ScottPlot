using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Statistics
{
    public static class BoxAndWhiskerFromData
    {
        public static BoxAndWhisker StdevStderrMean(double[] data, double xPosition)
        {
            var baw = new BoxAndWhisker(xPosition, data);
            var stats = new PopulationStats(data);
            baw.midline.position = stats.median;
            baw.whisker.max = stats.max;
            baw.whisker.min = stats.min;
            baw.box.max = stats.mean + stats.stDev;
            baw.box.min = stats.mean - stats.stDev;
            return baw;
        }

        public static BoxAndWhisker OutlierQuartileMedian(double[] data, double xPosition)
        {
            var baw = new BoxAndWhisker(xPosition, data);
            var stats = new PopulationStats(data);
            baw.midline.position = stats.median;
            baw.whisker.max = stats.highOutliers.Length == 0 ? stats.maxNonOutlier : stats.Q3 + 1.5 * stats.IQR;
            baw.whisker.min = stats.lowOutliers.Length == 0 ? stats.minNonOutlier : stats.Q1 - 1.5 * stats.IQR;
            baw.box.max = stats.Q3;
            baw.box.min = stats.Q1;
            return baw;
        }
    }
}
