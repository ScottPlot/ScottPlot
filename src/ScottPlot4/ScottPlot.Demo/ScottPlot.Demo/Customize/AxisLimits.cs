using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Customize
{
    class AxisLimits
    {
        public class Auto : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Automatically fit to data";
            public string description { get; } = "Automatically adjust axis limits to fit data. By default the data is slightly padded with extra space.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.AxisAuto();
            }
        }

        public class AutoMargin : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Automatic fit with specified margin";
            public string description { get; } = "AxisAuto() arguments allow the user to define the amount of padding (margin) for each axis. Setting the margin to 0 will adjust the plot axis limits to tightly fit the data.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.AxisAuto(horizontalMargin: 0, verticalMargin: 0.5);
            }
        }

        public class Manual : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Manually define axis limits";
            public string description { get; } = "The user can manually define axis limits. If a null is passed in, that axis limit is not adjusted.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.Axis(-10, 60, -3, 3);
            }
        }

        public class Zoom : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Zoom";
            public string description { get; } = "The user can easily zoom and zoom by providing a fractional zoom amount. Numbers >1 zoom in while numbers <1 zoom out.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.AxisZoom(1.5, 1.5);
            }
        }

        public class Pan : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Pan";
            public string description { get; } = "The user can easily pan by a defined amount on each axis.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.AxisPan(-10, .5);
            }
        }
    }
}
