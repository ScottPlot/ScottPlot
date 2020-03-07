using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class Scatter
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Quickstart";
            public string description { get; } = "Scatter plots are best for small numbers of paired X/Y data points. For evenly-spaced data points Signal is much faster.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin);
                plt.PlotScatter(x, cos);
            }
        }

        public class CustomizeMarkers : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Custom markers";
            public string description { get; } = "Arguments allow markers to be customized";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);

                plt.PlotScatter(x, sin, markerSize: 15, markerShape: MarkerShape.openCircle);
                plt.PlotScatter(x, cos, markerSize: 7, markerShape: MarkerShape.filledSquare);
            }
        }

        public class AllMarkers : PlotDemo, IPlotDemo
        {
            public string name { get; } = "All marker shapes";
            public string description { get; } = "This plot demonstrates all available markers";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);

                string[] markerShapeNames = Enum.GetNames(typeof(MarkerShape));
                for (int i = 0; i < markerShapeNames.Length; i++)
                {
                    string markerShapeName = markerShapeNames[i];
                    MarkerShape markerShape = (MarkerShape)Enum.Parse(typeof(MarkerShape), markerShapeName);
                    double[] sin = DataGen.Sin(pointCount, 2, -i);
                    plt.PlotScatter(x, sin, label: markerShapeName, markerShape: markerShape, markerSize: 7);
                }

                plt.Grid(false);
                plt.Legend(fontSize: 10);
            }
        }

        public class CustomizeLines : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Custom lines";
            public string description { get; } = "Arguments allow line color, size, and pattern to be customized. Setting markerSize to 0 prevents markers from being rendered.";

            public void Render(Plot plt)
            {
                int pointCount = 51;
                double[] x = DataGen.Consecutive(pointCount);
                double[] sin = DataGen.Sin(pointCount);
                double[] cos = DataGen.Cos(pointCount);
                double[] cos2 = DataGen.Cos(pointCount, mult: -1);

                plt.PlotScatter(x, sin, color: Color.Magenta, label: "sin", lineWidth: 0, markerSize: 10);
                plt.PlotScatter(x, cos, color: Color.Green, label: "cos", lineWidth: 5, markerSize: 0);
                plt.PlotScatter(x, cos2, color: Color.Blue, label: "-cos", lineWidth: 3, markerSize: 0, lineStyle: LineStyle.DashDot);

                plt.Legend(fixedLineWidth: false);
            }
        }

        public class RandomXY : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Random X/Y Points";
            public string description { get; } = "X data for scatter plots does not have to be evenly spaced, making scatter plots are ideal for displaying random data like this.";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 51;
                double[] xs1 = DataGen.RandomNormal(rand, pointCount, 1);
                double[] xs2 = DataGen.RandomNormal(rand, pointCount, 3);
                double[] ys1 = DataGen.RandomNormal(rand, pointCount, 5);
                double[] ys2 = DataGen.RandomNormal(rand, pointCount, 7);

                plt.PlotScatter(xs1, ys1, markerSize: 0, label: "lines only");
                plt.PlotScatter(xs2, ys2, lineWidth: 0, label: "markers only");
                plt.Legend();
            }
        }
    }
}
