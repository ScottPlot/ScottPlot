using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class ErrorBar
    {
        public class ErrorBarsAsymmetric : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Scatter Plot with Asymmetric Errorbars";
            public string description { get; } = "Asymmetric X and Y error ranges can be supplied as optional double arrays for positive and/or negative error bars";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 20;

                // random data points
                double[] dataX = DataGen.Consecutive(pointCount);
                double[] dataY1 = DataGen.Sin(pointCount, offset: 0);
                double[] dataY2 = DataGen.Sin(pointCount, offset: 3);
                double[] dataY3 = DataGen.Sin(pointCount, offset: 6);

                // random errorbar sizes
                double[] errorYPositive = DataGen.RandomNormal(rand, pointCount);
                double[] errorXPositive = DataGen.RandomNormal(rand, pointCount);
                double[] errorYNegative = DataGen.RandomNormal(rand, pointCount);
                double[] errorXNegative = DataGen.RandomNormal(rand, pointCount);

                // plot errors in all 4 directions
                var err1 = plt.PlotErrorBars(dataX, dataY1, errorXPositive, errorXNegative, errorYPositive, errorYNegative);
                plt.PlotScatter(dataX, dataY1, lineWidth: 0, label: $"Asymmetric X and Y errors", color: err1.color);

                // plot upper and right errors only
                var err2 = plt.PlotErrorBars(dataX, dataY2, errorXPositive, null, errorYPositive, null);
                plt.PlotScatter(dataX, dataY2, lineWidth: 0, label: $"Positive errors only", color: err2.color);

                // plot lower and left errors only
                var err3 = plt.PlotErrorBars(dataX, dataY3, null, errorXNegative, null, errorYNegative);
                plt.PlotScatter(dataX, dataY3, lineWidth: 0, label: $"Negative errors only", color: err3.color);

                plt.Grid(false);
                plt.Legend();
            }
        }
    }
}
