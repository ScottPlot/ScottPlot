using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Statistics
{
    public static class BoxAndWhiskerFromData
    {
        public static BoxAndWhisker GetBoxAndWhisker_LimitsAndDeviation(double[] data, double xPosition)
        {
            var baw = new BoxAndWhisker(xPosition);

            var stats = new PopulationStats(data);
            baw.midline.position = stats.median;
            baw.whisker.max = stats.max;
            baw.whisker.min = stats.min;
            baw.box.max = stats.mean + stats.stDev;
            baw.box.min = stats.mean - stats.stDev;

            baw.points = new double[]{ };

            return baw;
        }

        public static BoxAndWhisker GetBoxAndWhisker_OutliersAndQuartiles(double[] data, double xPosition)
        {
            var baw = new BoxAndWhisker(xPosition);

            var stats = new PopulationStats(data);
            baw.midline.position = stats.median;
            baw.whisker.max = stats.highOutliers.Length == 0 ? stats.maxNonOutlier : stats.Q3 + 1.5 * stats.IQR;
            baw.whisker.min = stats.lowOutliers.Length == 0 ? stats.minNonOutlier : stats.Q1 - 1.5 * stats.IQR;
            baw.box.max = stats.Q3;
            baw.box.min = stats.Q1;

            List<double> pointsList = new List<double>();
            pointsList.AddRange(stats.highOutliers);
            pointsList.AddRange(stats.lowOutliers);
            baw.points = pointsList.ToArray();

            return baw;
        }
    }
}
