using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot.Statistics
{
    /// <summary>
    /// A population series is a collection of similar PopulationStats objects.
    /// </summary>
    public class PopulationSeries
    {
        public Population[] populations;
        public string seriesLabel;
        public System.Drawing.Color color;

        public PopulationSeries(Population[] populations, string seriesLabel = null, System.Drawing.Color? color = null)
        {
            this.populations = populations;
            this.seriesLabel = seriesLabel;
            this.color = (color is null) ? System.Drawing.Color.LightGray : color.Value;
        }
    }
}
