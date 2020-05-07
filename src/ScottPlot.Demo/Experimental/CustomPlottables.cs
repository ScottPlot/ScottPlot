using System;
using System.Collections.Generic;
using System.Linq;
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
                // rather than call Plot.Text(), create the Plottable object manually
                var customPlottable = new PlottableText(text: "test", x: 2, y: 3, 
                    color: System.Drawing.Color.Magenta, fontName: "arial", fontSize: 26, 
                    bold: true, label: "", alignment: TextAlignment.middleCenter,
                    rotation: 0, frame: false, frameColor: System.Drawing.Color.Green);

                // you can access properties which may not be exposed by a Plot method
                customPlottable.rotation = 45;

                // add the custom plottable to the list of plottables like this
                List<Plottable> plottables = plt.GetPlottables();
                plottables.Add(customPlottable);
            }
        }
        public class GappedSignal : PlotDemo, IPlotDemo
        {
            public string name { get; } = "SignalWithGaps";
            public string description { get; } = "Signal with not evently spaced data";

            public void Render(Plot plt)
            {
                Random rand = new Random(0);
                int pointCount = 1_000_000;

                double[] sine = ScottPlot.DataGen.Sin(pointCount, 3);
                double[] noise = ScottPlot.DataGen.RandomNormal(rand, pointCount, 0, 0.5);

                double[] ys = sine.Zip(noise, (s, n) => s + n).ToArray();

                double[] xs = Enumerable.Range(0, pointCount)
                    .Select(x => (double)x)
                    .Select(x => x > 500_000 ? x + 1_000_000 : x)
                    .Select(x => x > 200_000 ? x + 100_000 : x)
                    .ToArray();

                plt.PlotSignal(xs, ys);
            }
        }
    }
}