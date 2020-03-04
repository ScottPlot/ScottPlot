using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Experimental
{
    class BoxAndWhisker
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {

            public string name { get; } = "Box and Whisker (Stdev, Stderr, and Mean)";
            public string description { get; }

            public void Render(Plot plt)
            {
                var boxAndWiskers = new Statistics.BoxAndWhisker[3];
                boxAndWiskers[0] = Statistics.BoxAndWhiskerFromData.StdevStderrMean(Data.LineLengths.plot, 1);
                boxAndWiskers[1] = Statistics.BoxAndWhiskerFromData.StdevStderrMean(Data.LineLengths.formsPlot, 2);
                boxAndWiskers[2] = Statistics.BoxAndWhiskerFromData.StdevStderrMean(Data.LineLengths.wpfPlot, 3);

                plt.Title("Source Code Line Length");
                plt.PlotBoxAndWhisker(boxAndWiskers);
                plt.YLabel("Number of Characters");

                double[] xPositions = { 1, 2, 3 };
                string[] labels = { "Plot.cs", "FormsPlot.cs", "WpfPlot.cs" };
                plt.XTicks(xPositions, labels);

                plt.AxisAuto(.3, .2);
            }
        }

        public class Advanced : PlotDemo, IPlotDemo
        {

            public string name { get; } = "Box and Whisker (Outlier, Quartile, and Median)";
            public string description { get; }

            public void Render(Plot plt)
            {
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
            }
        }
    }
}
