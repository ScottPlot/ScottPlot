using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using ScottPlot.Config;
using ScottPlot.Drawing;
using ScottPlot.Statistics;

namespace ScottPlot
{
    public class PlottablePopulations : Plottable, IPlottable
    {
        public readonly PopulationMultiSeries popMultiSeries;
        public int groupCount { get { return popMultiSeries.groupCount; } }
        public int seriesCount { get { return popMultiSeries.seriesCount; } }
        public string[] labels { get { return popMultiSeries.seriesLabels; } }

        public enum DisplayItems { BoxOnly, BoxAndScatter, ScatterAndBox, ScatterOnly };
        public enum BoxStyle { BarMeanStDev, BarMeanStdErr, BoxMeanStdevStderr, BoxMedianQuartileOutlier, MeanAndStdev, MeanAndStderr };

        public bool displayDistributionCurve = true;
        public LineStyle distributionCurveLineStyle = LineStyle.Solid;
        public Color distributionCurveColor = Color.Black;
        public Color scatterOutlineColor = Color.Black;
        public DisplayItems displayItems = DisplayItems.BoxAndScatter;
        public BoxStyle boxStyle = BoxStyle.BoxMedianQuartileOutlier;

        public PlottablePopulations(PopulationMultiSeries groupedSeries)
        {
            popMultiSeries = groupedSeries;
        }

        public PlottablePopulations(Population[] populations, string label = null, Color? color = null)
        {
            var ps = new PopulationSeries(populations, label, color ?? Color.LightGray);
            popMultiSeries = new PopulationMultiSeries(new PopulationSeries[] { ps });
        }

        public PlottablePopulations(PopulationSeries populationSeries)
        {
            popMultiSeries = new PopulationMultiSeries(new PopulationSeries[] { populationSeries });
        }

        public PlottablePopulations(Population population, string label = null, Color? color = null)
        {
            var populations = new Population[] { population };
            var ps = new PopulationSeries(populations, label, color ?? Color.LightGray);
            popMultiSeries = new PopulationMultiSeries(new PopulationSeries[] { ps });
        }

        public override string ToString()
        {
            return $"PlottableSeries with {popMultiSeries.groupCount} groups, {popMultiSeries.seriesCount} series, and {GetPointCount()} total points";
        }

        public override int GetPointCount()
        {
            int pointCount = 0;
            foreach (var group in popMultiSeries.multiSeries)
                foreach (var population in group.populations)
                    pointCount += population.count;
            return pointCount;
        }

        public override LegendItem[] GetLegendItems()
        {
            var items = new List<LegendItem>();
            foreach (var series in popMultiSeries.multiSeries)
                items.Add(new LegendItem(series.seriesLabel, series.color, lineWidth: 10));
            return items.ToArray();
        }

        public override AxisLimits2D GetLimits()
        {
            double minValue = double.PositiveInfinity;
            double maxValue = double.NegativeInfinity;

            foreach (var series in popMultiSeries.multiSeries)
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
            double positionMax = popMultiSeries.groupCount - 1;

            // padd slightly
            positionMin -= .5;
            positionMax += .5;

            return new AxisLimits2D(positionMin, positionMax, minValue, maxValue);
        }

        // TODO: add validation to the Population module and check for it here
        public string ValidationErrorMessage { get; private set; }
        public bool IsValidData(bool deepValidation = false) => true;

        public override void Render(Settings settings) => throw new InvalidOperationException("use other Render method");

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            Random rand = new Random(0);
            double groupWidth = .8;
            var popWidth = groupWidth / seriesCount;

            for (int seriesIndex = 0; seriesIndex < seriesCount; seriesIndex++)
            {
                for (int groupIndex = 0; groupIndex < groupCount; groupIndex++)
                {
                    var series = popMultiSeries.multiSeries[seriesIndex];
                    var population = series.populations[groupIndex];
                    var groupLeft = groupIndex - groupWidth / 2;
                    var popLeft = groupLeft + popWidth * seriesIndex;

                    RenderPopulation.Position scatterPos, boxPos;
                    switch (displayItems)
                    {
                        case DisplayItems.BoxAndScatter:
                            boxPos = RenderPopulation.Position.Left;
                            scatterPos = RenderPopulation.Position.Right;
                            break;
                        case DisplayItems.BoxOnly:
                            boxPos = RenderPopulation.Position.Center;
                            scatterPos = RenderPopulation.Position.Hide;
                            break;
                        case DisplayItems.ScatterAndBox:
                            boxPos = RenderPopulation.Position.Right;
                            scatterPos = RenderPopulation.Position.Left;
                            break;
                        case DisplayItems.ScatterOnly:
                            boxPos = RenderPopulation.Position.Hide;
                            scatterPos = RenderPopulation.Position.Center;
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    RenderPopulation.Scatter(dims, bmp, lowQuality, population, rand, popLeft, popWidth, series.color, scatterOutlineColor, 128, scatterPos);

                    if (displayDistributionCurve)
                        RenderPopulation.Distribution(dims, bmp, lowQuality, population, rand, popLeft, popWidth, distributionCurveColor, scatterPos, distributionCurveLineStyle);

                    switch (boxStyle)
                    {
                        case BoxStyle.BarMeanStdErr:
                            RenderPopulation.Bar(dims, bmp, lowQuality, population, rand, popLeft, popWidth, series.color, boxPos, useStdErr: true);
                            break;
                        case BoxStyle.BarMeanStDev:
                            RenderPopulation.Bar(dims, bmp, lowQuality, population, rand, popLeft, popWidth, series.color, boxPos, useStdErr: false);
                            break;
                        case BoxStyle.BoxMeanStdevStderr:
                            RenderPopulation.Box(dims, bmp, lowQuality, population, rand, popLeft, popWidth, series.color, boxPos, RenderPopulation.BoxFormat.StdevStderrMean);
                            break;
                        case BoxStyle.BoxMedianQuartileOutlier:
                            RenderPopulation.Box(dims, bmp, lowQuality, population, rand, popLeft, popWidth, series.color, boxPos, RenderPopulation.BoxFormat.OutlierQuartileMedian);
                            break;
                        case BoxStyle.MeanAndStderr:
                            RenderPopulation.MeanAndError(dims, bmp, lowQuality, population, rand, popLeft, popWidth, series.color, boxPos, useStdErr: true);
                            break;
                        case BoxStyle.MeanAndStdev:
                            RenderPopulation.MeanAndError(dims, bmp, lowQuality, population, rand, popLeft, popWidth, series.color, boxPos, useStdErr: false);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }
    }
}
