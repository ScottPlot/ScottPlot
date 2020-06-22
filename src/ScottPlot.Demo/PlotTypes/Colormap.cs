using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace ScottPlot.Demo.PlotTypes
{
    class Colormap
    {
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Colormap Quickstart";
            public string description { get; } = "Colormaps make it easy to translate a fractional value (from 0 to 1) into a color.";

            public void Render(Plot plt)
            {
                int lineCount = 10;
                for (int i = 0; i < lineCount; i++)
                {
                    double[] ys = DataGen.Sin(51, phase: i * .03);
                    double fraction = (double)i / lineCount;
                    Color c = Drawing.Colormap.Jet.GetColor(fraction);
                    plt.PlotSignal(ys, color: c, lineWidth: 2, markerSize: 0);
                }

                plt.AxisAuto(0);
            }
        }
    }
}
