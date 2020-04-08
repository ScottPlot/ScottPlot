using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    public static class Fill
    {
        public class FillBeneathCurve : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Filled Curve";
            public string description { get; } = "You can create a filled scatter plot where the area between the curve and baseline is shaded with a color. The baseline defaults to 0, but can be set with an optional argument.";

            public void Render(Plot plt)
            {
                double[] xs = DataGen.Range(0, 10, .1, true);
                double[] sin = DataGen.Sin(xs);
                double[] cos = DataGen.Cos(xs);

                plt.PlotFill(xs, sin, "sin", lineWidth: 2, fillAlpha: .5);
                plt.PlotFill(xs, cos, "cos", lineWidth: 2, fillAlpha: .5);
                plt.PlotHLine(0, color: Color.Black);
                plt.AxisAuto(0);
                plt.Legend(location: legendLocation.lowerLeft);
            }
        }

        public class FillBetweenCurves : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Fill Between Curves";
            public string description { get; } = "You can fill the area between curves by supplying two pairs of X/Y coordinates";

            public void Render(Plot plt)
            {
                double[] xs = DataGen.Range(0, 10, .1, true);
                double[] sin = DataGen.Sin(xs);
                double[] cos = DataGen.Cos(xs);

                plt.PlotFill(xs, sin, xs, cos, fillAlpha: .5);
                plt.PlotScatter(xs, sin, Color.Black);
                plt.PlotScatter(xs, cos, Color.Black);

                plt.AxisAuto(0);
            }
        }
    }
}
