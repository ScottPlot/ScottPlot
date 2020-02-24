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
            boxAndWiskers[0] = Statistics.BoxAndWhiskerFromData.OutlierQuartileMedian(Data.LineLengths.plot, 1);
            boxAndWiskers[1] = Statistics.BoxAndWhiskerFromData.OutlierQuartileMedian(Data.LineLengths.formsPlot, 2);
            boxAndWiskers[2] = Statistics.BoxAndWhiskerFromData.OutlierQuartileMedian(Data.LineLengths.wpfPlot, 3);

            plt.Title("Source Code Line Length");
            plt.PlotBoxAndWhisker(boxAndWiskers);
            plt.YLabel("Number of Characters");

            double[] xPositions = { 1, 2, 3 };
            string[] labels = { "Plot.cs", "FormsPlot.cs", "WpfPlot.cs" };
            plt.XTicks(xPositions, labels);

            plt.AxisAuto(.3, .2);
            return plt;
        }
    }
}
