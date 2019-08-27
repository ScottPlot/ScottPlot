using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Config
{
    public class Colors
    {
        // plot colors (https://github.com/vega/vega/wiki/Scales#scale-range-literals)

        string[] plottableColors10 = new string[] { "#1f77b4", "#ff7f0e", "#2ca02c", "#d62728",
                    "#9467bd", "#8c564b", "#e377c2", "#7f7f7f", "#bcbd22", "#17becf" };

        string[] plottableColors20 = new string[] { "#1f77b4", "#aec7e8", "#ff7f0e", "#ffbb78",
                "#2ca02c", "#98df8a", "#d62728", "#ff9896", "#9467bd", "#c5b0d5",
                "#8c564b", "#c49c94", "#e377c2", "#f7b6d2", "#7f7f7f", "#c7c7c7",
                "#bcbd22", "#dbdb8d", "#17becf", "#9edae5", };

        public bool useTwentyColors = false;

        public Color GetColor(int index)
        {
            string[] colors = (useTwentyColors) ? plottableColors20 : plottableColors10;
            return ColorTranslator.FromHtml(colors[index % colors.Length]);
        }
    }
}
