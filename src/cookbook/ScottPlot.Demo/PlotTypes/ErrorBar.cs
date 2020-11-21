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
                double[] dataY1 = DataGen.RandomNormal(rand, pointCount, mean: 20, stdDev: 2);
                double[] dataY2 = DataGen.RandomNormal(rand, pointCount, mean: 10, stdDev: 2);
                double[] dataY3 = DataGen.RandomNormal(rand, pointCount, mean: 0, stdDev: 2);

                // random errorbar sizes
                double[] errorYPositive = DataGen.RandomNormal(rand, pointCount);
                double[] errorXPositive = DataGen.RandomNormal(rand, pointCount);
                double[] errorYNegative = DataGen.RandomNormal(rand, pointCount);
                double[] errorXNegative = DataGen.RandomNormal(rand, pointCount);

                // plot different combinations of errorbars
                var err1 = plt.PlotErrorBars(dataX, dataY1, errorXPositive, errorXNegative, errorYPositive, errorYNegative);
                var err2 = plt.PlotErrorBars(dataX, dataY2, errorXPositive, null, errorYPositive, null);
                var err3 = plt.PlotErrorBars(dataX, dataY3, null, errorXNegative, null, errorYNegative);

                // draw scatter plots on top of the errorbars
                plt.PlotScatter(dataX, dataY1, color: err1.Color, label: "Both");
                plt.PlotScatter(dataX, dataY2, color: err2.Color, label: "Positive");
                plt.PlotScatter(dataX, dataY3, color: err3.Color, label: $"Negative");

                plt.Title("Error Bars with Asymmetric X and Y Values");
                plt.Grid(false);
                plt.Legend();
            }
        }
    }
}
