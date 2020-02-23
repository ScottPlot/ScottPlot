using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    public static class BoxAndWhisker
    {
        public static Plot Quickstart()
        {
            var plt = new Plot();

            var boxAndWiskers = new Statistics.BoxAndWhisker[3];
            boxAndWiskers[0] = GetBoxAndWhisker(Data.LineLengths.plot, 1);
            boxAndWiskers[1] = GetBoxAndWhisker(Data.LineLengths.formsPlot, 2);
            boxAndWiskers[2] = GetBoxAndWhisker(Data.LineLengths.wpfPlot, 3);

            plt.Title("Source Code Line Length");
            plt.PlotBoxAndWhisker(boxAndWiskers);
            plt.YLabel("Number of Characters");

            double[] xPositions = { 1, 2, 3 };
            string[] labels = { "Plot.cs", "FormsPlot.cs", "WpfPlot.cs" };
            plt.XTicks(xPositions, labels);

            plt.AxisAuto(.3, .2);
            return plt;
        }

        public static Plot Quickstart2()
        {
            var plt = new Plot();

            double[] xPositions = { 1, 2, 3 };
            double[][] dataArrays = new double[][] { Data.LineLengths.plot, Data.LineLengths.formsPlot, Data.LineLengths.wpfPlot };
            plt.PlotBoxAndWhiskerV2(xPositions, dataArrays);

            plt.Title("Source Code Line Length (Benny's Version)");
            plt.YLabel("Number of Characters");

            string[] labels = { "Plot.cs", "FormsPlot.cs", "WpfPlot.cs" };
            plt.XTicks(xPositions, labels);

            plt.AxisAuto(.3, .2);
            return plt;
        }

        static Statistics.BoxAndWhisker GetBoxAndWhisker(double[] data, double xPosition)
        {
            var baw = new Statistics.BoxAndWhisker(xPosition);
            var stats = new Statistics.PopulationStats(data);
            baw.midline.position = stats.median;
            baw.whisker.max = stats.max;
            baw.whisker.min = stats.min;
            baw.box.max = stats.mean + stats.stDev;
            baw.box.min = stats.mean - stats.stDev;
            return baw;
        }
    }
}
