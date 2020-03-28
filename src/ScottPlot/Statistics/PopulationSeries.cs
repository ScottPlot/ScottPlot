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
        public PopulationStats[] populations;
        public string seriesLabel;
        public string[] groupLabels;
        public System.Drawing.Color? color;

        public PopulationSeries(PopulationStats[] populations, string seriesLabel, string[] groupLabels, System.Drawing.Color? color = null)
        {
            this.populations = populations;
            this.seriesLabel = seriesLabel;
            this.groupLabels = groupLabels;

            if (color is null)
                color = new Config.Colors().GetColor(0);

            this.color = color;
        }
    }
}
