using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Customize
{
    class Axis
    {
        public class AxisLabels : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Title and Axis Labels";
            public string description { get; } = "Title and axis labels can be defined and custoized using arguments.";

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.Title("Plot Title");
                plt.XLabel("Horizontal Axis");
                plt.YLabel("Vertical Axis");
            }
        }

        public class RulerMode : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Ruler Mode";
            public string description { get; } = "Ruler mode is an alternative way to display axis tick labels";

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.Ticks(rulerModeX: true, rulerModeY: true);
            }
        }

        public class RulerModeXOnly : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Ruler Mode (X only)";
            public string description { get; } = "Ruler mode only on one axis";

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.Ticks(rulerModeX: true, displayTicksY: false);
                plt.Frame(left: false, right: false, top: false);
                plt.TightenLayout(padding: 0, render: true);
            }
        }

        public class LogAxis : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Log Axis";
            public string description { get; } = "";

            public void Render(Plot plt)
            {
                // generate some interesting log-distributed data
                int pointCount = 200;
                double[] dataXs = new double[pointCount];
                double[] dataYs = new double[pointCount];
                Random rand = new Random(0);
                for (int i = 0; i < pointCount; i++)
                {
                    double x = 10.0 * i / pointCount;
                    dataXs[i] = x;
                    dataYs[i] = Math.Pow(2, x) + rand.NextDouble() * i;
                }

                plt.PlotScatter(dataXs, ScottPlot.Tools.Log10(dataYs), lineWidth: 0);
                plt.Title("Data (Log Scale)");
                plt.YLabel("Vertical Units (10^x)");
                plt.XLabel("Horizontal Units");
            }
        }

        public class PolarAxis : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Polar Axis";
            public string description { get; } = "";

            public void Render(Plot plt)
            {
                int count = 400;
                double step = 0.01;

                double[] dataR = new double[count];
                double[] dataTheta = new double[count];
                double[] dataX = new double[count];
                double[] dataY = new double[count];

                for (int i = 0; i < dataR.Length; i++)
                {
                    dataR[i] = 1 + (double)i * step;
                    dataTheta[i] = i * 2 * Math.PI * step;
                }

                (dataX, dataY) = ScottPlot.Tools.ConvertPolarCoordinates(dataR, dataTheta);

                plt.PlotScatter(dataX, dataY);
                plt.Title("Data (Polar Scale)");
            }
        }
    }
}
