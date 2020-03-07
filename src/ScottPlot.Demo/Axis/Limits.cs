using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Axis
{
    class Limits
    {
        public class Auto : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Automatically fit to data";
            public string description { get; } = "Automatically adjust axis limits to fit data. By default the data is slightly padded with extra space.";

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.AxisAuto();
            }
        }

        public class AutoMargin : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Automatic fit with specified margin";
            public string description { get; } = "AxisAuto() arguments allow the user to define the amount of padding (margin) for each axis. Setting the margin to 0 will adjust the plot axis limits to tightly fit the data.";

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.AxisAuto(horizontalMargin: 0, verticalMargin: 0.5);
            }
        }

        public class Manual : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Manually define axis limits";
            public string description { get; } = "The user can manually define axis limits. If a null is passed in, that axis limit is not adjusted.";

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.Axis(-10, 60, -3, 3);
            }
        }
    }
}
