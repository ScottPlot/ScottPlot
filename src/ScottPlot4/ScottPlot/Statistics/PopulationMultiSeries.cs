using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Statistics
{
    public class PopulationMultiSeries
    {
        public PopulationSeries[] multiSeries;

        public string[] seriesLabels { get { return multiSeries.Select(x => x.seriesLabel).ToArray(); } }
        public int seriesCount { get { return multiSeries.Length; } }
        public int groupCount { get { return multiSeries[0].populations.Length; } }

        public PopulationMultiSeries(PopulationSeries[] multiSeries)
        {
            if (multiSeries is null)
                throw new ArgumentException("groupedSeries cannot be null");

            foreach (var series in multiSeries)
                if (series.populations.Length != multiSeries[0].populations.Length)
                    throw new ArgumentException("All series must have the same number of populations");

            this.multiSeries = multiSeries;
        }
    }
}
