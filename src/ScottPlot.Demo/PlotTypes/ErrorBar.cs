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

                for (int plotNumber = 0; plotNumber < 3; plotNumber++)
                {
                    // create random data to plot
                    double[] dataX = new double[pointCount];
                    double[] dataY = new double[pointCount];
                    double[] errorYPositive = new double[pointCount];
                    double[] errorXPositive = new double[pointCount];
                    double[] errorYNegative = new double[pointCount];
                    double[] errorXNegative = new double[pointCount];
                    for (int i = 0; i < pointCount; i++)
                    {
                        dataX[i] = i + rand.NextDouble();
                        dataY[i] = rand.NextDouble() * 100 + 100 * plotNumber;
                        errorYPositive[i] = rand.NextDouble() * 10;
                        errorXPositive[i] = rand.NextDouble();
                        errorYNegative[i] = rand.NextDouble() * 10;
                        errorXNegative[i] = rand.NextDouble();
                    }

                    // demonstrate different ways to plot errorbars
                    if (plotNumber == 0)
                    {
                        PlottableScatter ps = plt.PlotScatter(dataX, dataY, lineWidth: 0, label: $"Asymmetric X and Y errors");
                        plt.PlotErrorBars(dataX, dataY, errorXPositive, errorXNegative, errorYPositive, errorYNegative, ps.color);
                    }
                    else if (plotNumber == 1)
                    {
                        PlottableScatter ps = plt.PlotScatter(dataX, dataY, lineWidth: 0, label: $"Positive errors only");
                        plt.PlotErrorBars(dataX, dataY, errorXPositive, null, errorYPositive, null, ps.color);
                    }
                    else
                    {
                        PlottableScatter ps = plt.PlotScatter(dataX, dataY, lineWidth: 0, label: $"Negative errors only");
                        plt.PlotErrorBars(dataX, dataY, null, errorXNegative, null, errorYNegative, ps.color);
                    }
                }

                plt.Legend();
                plt.AxisAuto();
            }
        }
    }
}
