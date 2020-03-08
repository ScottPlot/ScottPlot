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
                GenericPlots.SinAndCos(plt);
                plt.Grid(enable: false);
            }
        }

        public class DefineSpacing : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Defined Grid Spacing";
            public string description { get; } = "The space between grid lines (the same as tick marks) can be manually defined.";

            public void Render(Plot plt)
            {
                GenericPlots.SinAndCos(plt);
                plt.Grid(xSpacing: 2, ySpacing: .1);
            }
        }
    }
}
