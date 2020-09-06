using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Customize
{
    class Grid
    {
        public class Hide : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Hide the grid";
            public string description { get; } = "Grid visibility (and numerous other options) are available as arguments in the Grid() method.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.Grid(enable: false);
            }
        }

        public class LineWidth : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Grid Line Width";
            public string description { get; } = "Grid line width can be customized. Floating point values are acceptable.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.Grid(lineWidth: 2);
            }
        }

        public class LineStyle : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Grid Line Style";
            public string description { get; } = "Grid line style can be customized.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.Grid(lineStyle: ScottPlot.LineStyle.Dot);
            }
        }

        public class DefineSpacing : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Defined Grid Spacing";
            public string description { get; } = "The space between grid lines (the same as tick marks) can be manually defined.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);

                plt.Grid(xSpacing: 2, ySpacing: .1);
            }
        }
    }
}
