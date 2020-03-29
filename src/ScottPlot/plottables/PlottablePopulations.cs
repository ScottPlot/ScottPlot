using ScottPlot.Config;
using ScottPlot.Statistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot
{
    public class PlottablePopulations : Plottable
    {
        PopulationMultiSeries groupedSeries;
        public int groupCount { get { return groupedSeries.groupCount; } }
        public int seriesCount { get { return groupedSeries.seriesCount; } }

        public PlottablePopulations(PopulationMultiSeries groupedSeries)
        {
            this.groupedSeries = groupedSeries;
        }

        public PlottablePopulations(Population[] populations, string label = null, System.Drawing.Color? color = null)
        {
            if (color is null)
                color = System.Drawing.Color.LightGray;

            var ps = new PopulationSeries(populations, label, color.Value);
            groupedSeries = new PopulationMultiSeries(
                multiSeries: new PopulationSeries[] { ps },
                groupLabels: new string[populations.Length],
                colors: new System.Drawing.Color[] { color.Value }
                );
        }

        public PlottablePopulations(Population population, string label = null, System.Drawing.Color? color = null)
        {
            if (color is null)
                color = System.Drawing.Color.LightGray;

            var populations = new Population[] { population };
            var ps = new PopulationSeries(populations, label, color.Value);
            groupedSeries = new PopulationMultiSeries(
                multiSeries: new PopulationSeries[] { ps },
                groupLabels: new string[populations.Length],
                colors: new System.Drawing.Color[] { color.Value }
                );
        }

        public override string ToString()
        {
            return $"PlottableSeries with {groupedSeries.groupCount} groups, {groupedSeries.seriesCount} series, and {GetPointCount()} total points";
        }

        public override int GetPointCount()
        {
            int pointCount = 0;
            foreach (var group in groupedSeries.multiSeries)
                foreach (var population in group.populations)
                    pointCount += population.count;
            return pointCount;
        }

        public override LegendItem[] GetLegendItems()
        {
            var items = new List<LegendItem>();
            foreach (var series in groupedSeries.multiSeries)
                items.Add(new LegendItem(series.seriesLabel, series.color, lineWidth: 10));
            return items.ToArray();
        }

        public override AxisLimits2D GetLimits()
        {
            double minValue = double.PositiveInfinity;
            double maxValue = double.NegativeInfinity;

            foreach (var series in groupedSeries.multiSeries)
            {
                foreach (var population in series.populations)
                {
                    minValue = Math.Min(minValue, population.min);
                    minValue = Math.Min(minValue, population.minus3stDev);
                    maxValue = Math.Max(maxValue, population.max);
                    maxValue = Math.Max(maxValue, population.plus3stDev);
                }
            }

            double positionMin = 0;
            double positionMax = groupedSeries.groupCount - 1;

            // padd slightly
            positionMin -= .5;
            positionMax += .5;

            return new AxisLimits2D(positionMin, positionMax, minValue, maxValue);
        }

        public override void Render(Settings settings)
        {
            Random rand = new Random(0);
            double groupWidth = .8;
            var popWidth = groupWidth / seriesCount;

            for (int seriesIndex = 0; seriesIndex < seriesCount; seriesIndex++)
            {
                for (int groupIndex = 0; groupIndex < groupCount; groupIndex++)
                {
                    var series = groupedSeries.multiSeries[seriesIndex];
                    var population = series.populations[groupIndex];
                    var groupLeft = groupIndex - groupWidth / 2;
                    var popLeft = groupLeft + popWidth * seriesIndex;

                    RenderPopulation.Scatter(settings, population, rand, popLeft, popWidth, series.color, System.Drawing.Color.Black, 128, RenderPopulation.Position.Right);
                    RenderPopulation.Distribution(settings, population, rand, popLeft, popWidth, System.Drawing.Color.Black, RenderPopulation.Position.Right);
                    //RenderPopulation.MeanAndError(settings, population, rand, popLeft, popWidth, series.color, RenderPopulation.Position.Left);
                    //RenderPopulation.Bar(settings, population, rand, popLeft, popWidth, series.color, RenderPopulation.Position.Left);
                    RenderPopulation.Box(settings, population, rand, popLeft, popWidth, series.color, RenderPopulation.Position.Left, RenderPopulation.BoxFormat.OutlierQuartileMedian);
                }
            }
        }
    }
}
