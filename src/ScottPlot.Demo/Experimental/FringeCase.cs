using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Experimental
{
    public class FringeCase
    {
        public class EmptyPlot : PlotDemo, IPlotDemo
        {

            public string name { get; } = "Empty Plot";
            public string description { get; } = "This is what a plot looks like if you never added a plottable.";

            public void Render(Plot plt)
            {
                plt.Title("Empty Plot");
            }
        }
    }
}
