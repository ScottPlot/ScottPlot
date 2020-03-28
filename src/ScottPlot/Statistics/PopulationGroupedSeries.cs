using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScottPlot.Statistics
{
    /// <summary>
    /// This module holds groups of population series.
    /// </summary>
    public class PopulationGroupedSeries
    {
        /*
         * The goal is to display SERIES data in GROUPS
         *   - All bars of a series will be the same color
         *   - A series as the same number of elements as the number of groups
         *   - Each series will appear once in the legend
         *   
         */

        public PopulationSeries[] groupedSeries;

        public string[] groupLabels;
        public int groupCount { get { return groupLabels.Length; } }

        public string[] seriesLabels { get { return groupedSeries.Select(x => x.seriesLabel).ToArray(); } }
        public int seriesCount { get { return groupedSeries.Length; } }

        public PopulationGroupedSeries(PopulationSeries[] groupedSeries, string[] groupLabels)
        {
            this.groupedSeries = groupedSeries;
            this.groupLabels = groupLabels;

            var defaultColors = new ScottPlot.Config.Colors();
            for (int i = 0; i < groupedSeries.Length; i++)
            {
                if (groupedSeries[i].color is null)
                    groupedSeries[i].color = defaultColors.GetColor(i);
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
                    var pop = groupedSeries[seriesIndex].populations[groupIndex];
                    sb.AppendLine($"{group} {series}: {pop.mean:0.00} +/- {pop.stdErr:0.00}");
                }
            }
            return sb.ToString();
        }
    }
}
