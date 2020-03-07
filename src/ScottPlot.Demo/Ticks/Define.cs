using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Ticks
{
    class Define
    {
        public class Spacing : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Define Tick Spacing";
            public string description { get; } = "Tick label visibility can be controlled with arguments to the Ticks() method";

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                //???
            }
        }

        public class Positions : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Define Tick Positions";
            public string description { get; } = "An array of tick positions and labels can be manually defined.";

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);

                double[] xPositions = { 7, 21, 37, 46 };
                string[] xLabels = { "VII", "XXI", "XXXVII", "XLVI" };
                plt.XTicks(xPositions, xLabels);

                double[] yPositions = { -1, 0, .5, 1 };
                string[] yPabels = { "bottom", "center", "half", "top" };
                plt.YTicks(yPositions, yPabels);
            }
        }

        public class Inverted : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Descending Ticks";
            public string description { get; } = "ScottPlot will ALWAYS display data where X values ascend from left to right. To simulate an inverted axis (where numbers decrease from left to right) plot data in the NEGATIVE space, then use a Tick() argument to invert the sign of tick labels.";

            public void Render(Plot plt)
            {
                // plot in the negative space
                plt.PlotSignal(DataGen.Sin(50), xOffset: -50);

                // then invert the sign of the axis tick labels
                plt.Ticks(invertSignX: true);
                plt.Ticks(invertSignY: true);
            }
        }

    }
}
