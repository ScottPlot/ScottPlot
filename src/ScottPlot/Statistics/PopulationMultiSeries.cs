using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Statistics
{
    public class PopulationMultiSeries
    {
        public PopulationSeries[] multiSeries;

        public string[] groupLabels;
        public int groupCount { get { return groupLabels.Length; } }

        public string[] seriesLabels { get { return multiSeries.Select(x => x.seriesLabel).ToArray(); } }
        public int seriesCount { get { return multiSeries.Length; } }

        public PopulationMultiSeries(PopulationSeries[] multiSeries, string[] groupLabels, System.Drawing.Color[] colors = null)
        {
            if (multiSeries is null)
                throw new ArgumentException("groupedSeries cannot be null");
            else
                this.multiSeries = multiSeries;

            if (groupLabels is null)
                throw new ArgumentException("group labels must have same number of elements as groupedSeries");
            else
                foreach (var series in multiSeries)
                    if (series.populations.Length != groupLabels.Length)
                        throw new ArgumentException("GroupLabels must be identical in length to all populations in the multiSeries");

            this.groupLabels = groupLabels;

            if (colors is null)
            {
                var colorset = new ScottPlot.Config.Colors();
                for (int i = 0; i < multiSeries.Length; i++)
                    multiSeries[i].color = colorset.GetColor(i);
            }
            else
            {
                if (colors.Length != multiSeries.Length)
                    throw new ArgumentException("colors must have same number of elements as groupedSeries");
                else
                    for (int i = 0; i < multiSeries.Length; i++)
                        multiSeries[i].color = colors[i];
            }
        }

        public string GetDescription()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Grouped data with {groupCount} groups ({String.Join(", ", groupLabels)}), each with {seriesCount} series ({String.Join(", ", seriesLabels)})");
            for (int groupIndex = 0; groupIndex < groupCount; groupIndex++)
            {
                sb.AppendLine();
                var group = groupLabels[groupIndex];
                for (int seriesIndex = 0; seriesIndex < seriesCount; seriesIndex++)
                {
                    var series = seriesLabels[seriesIndex];
                    var pop = multiSeries[seriesIndex].populations[groupIndex];
                    sb.AppendLine($"{group} {series}: {pop.mean:0.00} +/- {pop.stdErr:0.00}");
                }
            }
            return sb.ToString();
        }
    }
}
