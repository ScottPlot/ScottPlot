using System;
using System.Drawing;

namespace ScottPlot.Demo.PlotTypes
{
    class Quiver
    {
	    /// <summary>
	    /// Inspired by https://plotly.com/python/quiver-plots/
	    /// </summary>
        public class Quickstart : PlotDemo, IPlotDemo
        {
            public string name { get; } = "Quiver plot";
            public string description { get; } = "quivers can be added at specific points on the plot";

            public void Render(Plot plt)
            {
	            var scale = 0.25;
					for (var x = -2.0; x <= 2; x += 0.25)
					for (var y = -2.0; y <= 2; y += 0.25)
					{
						var e = scale * Math.Exp(-x * x - y * y);
						// Get scaled gradient
						var dx = (1 - 2 * x * x) * e;
						var dy = -2 * x * y * e;
						
						plt.PlotQuiver(x, y, x + dx, y + dy, color:Color.Red);
					}
					plt.Legend(fixedLineWidth: false);
            }
        }
    }
}
