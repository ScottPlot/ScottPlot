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
        public string[] groupLabels;
        public System.Drawing.Color color;

        public PopulationSeries(Population[] populations, string seriesLabel, string[] groupLabels, System.Drawing.Color color)
        {
            this.populations = populations;
            this.seriesLabel = seriesLabel;
            this.groupLabels = groupLabels;
            this.color = color;
        }
    }
}
