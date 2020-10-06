using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Demo.Experimental
{
    class CustomPlottables
    {
        public class AddPlottable : PlotDemo, IPlotDemo
        {

            public string name { get; } = "Add a Plottable Manually";
            public string description { get; } = "Demonstrates how to add a Plottable to the plot without relying on a method in the Plot module.";

            public void Render(Plot plt)
            {
                // rather than call Plot.PlotText(), create the Plottable manually
                var customPlottable = new PlottableText()
                {
                    text = "test",
                    x = 2,
                    y = 3,
                    FontColor = System.Drawing.Color.Magenta,
                    FontName = "arial",
                    FontSize = 26,
                    FontBold = true,
                    alignment = TextAlignment.middleCenter,
                    rotation = 0,
                    frame = false,
                    frameColor = System.Drawing.Color.Green
                };

                // you can access properties which may not be exposed by a Plot method
                customPlottable.rotation = 45;

                // add the custom plottable to the list of plottables like this
                List<Plottable> plottables = plt.GetPlottables();
                plottables.Add(customPlottable);
            }
        }
    }
}
