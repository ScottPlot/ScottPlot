using ScottPlot.Config;
using ScottPlot.Statistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot
{
    [Obsolete("WARNING: this will be deleted and replaced with PlottableGroupedSeries")]
    public class PlottableSeries : Plottable
    {
        PopulationSeries series;
        int groupCount { get { return series.populations.Length; } }

        public PlottableSeries(ScottPlot.Statistics.PopulationSeries series)
        {
            this.series = series;
        }

        public override string ToString()
        {
            return $"PlottableSeries with {groupCount} groups ({GetPointCount()} total points)";
        }

        public override LegendItem[] GetLegendItems()
        {
            var singleLegendItem = new LegendItem(series.seriesLabel, series.color.Value, lineWidth: 10);
            return new LegendItem[] { singleLegendItem };
        }

        public override AxisLimits2D GetLimits()
        {
            double minValue = double.PositiveInfinity;
            double maxValue = double.NegativeInfinity;

            foreach (var population in series.populations)
            {
                minValue = Math.Min(minValue, population.min);
                maxValue = Math.Max(maxValue, population.max);
            }

            double positionMin = 0;
            double positionMax = series.populations.Length - 1;

            // padd slightly
            positionMin -= .5;
            positionMax += .5;

            return new AxisLimits2D(positionMin, positionMax, minValue, maxValue);
        }

        public override int GetPointCount()
        {
            int pointCount = 0;
            foreach (var population in series.populations)
                pointCount += population.count;
            return pointCount;
        }

        public override void Render(Settings settings)
        {
            Random rand = new Random(0);

            for (int seriesIndex = 0; seriesIndex < series.populations.Length; seriesIndex++)
            {
                double position = seriesIndex;
                double positionLeft = position - .4;
                double positionRight = position + .4;
                RenderPopulation(settings, series.populations[seriesIndex], positionLeft, positionRight, rand);
            }
        }

        private void RenderPopulation(Settings settings, PopulationStats population, double positionLeft, double positionRight, Random rand)
        {
            System.Drawing.Color color = System.Drawing.Color.Navy;
            System.Drawing.Pen markerPen = new System.Drawing.Pen(color, 1);
            System.Drawing.Pen errorPen = new System.Drawing.Pen(color, 2);
            System.Drawing.Brush meanMarkerBrush = new System.Drawing.SolidBrush(color);

            double positionCenter = (positionLeft + positionRight) / 2;

            RenderMeanMarker(settings, population, positionLeft, positionCenter, meanMarkerBrush);
            RenderErrors(settings, population, positionLeft, positionCenter, errorPen);
            RenderScatter(settings, population, positionCenter, positionRight, rand, markerPen);
        }

        private void RenderErrors(Settings settings, PopulationStats population, double positionLeft, double positionRight,
            System.Drawing.Pen pen, double errorCapSizeFrac = .5)
        {
            double errorCapHalfWidth = errorCapSizeFrac * (positionRight - positionLeft) / 2;
            double positionCenter = (positionLeft + positionRight) / 2;
            double error1px = settings.GetPixelY(population.mean - population.stDev);
            double error2px = settings.GetPixelY(population.mean + population.stDev);
            double positionPx = settings.GetPixelX(positionCenter);
            double cap1Px = settings.GetPixelX(positionCenter - errorCapHalfWidth);
            double cap2Px = settings.GetPixelX(positionCenter + errorCapHalfWidth);

            settings.gfxData.DrawLine(pen, (float)positionPx, (float)error1px, (float)positionPx, (float)error2px);
            settings.gfxData.DrawLine(pen, (float)cap1Px, (float)error1px, (float)cap2Px, (float)error1px);
            settings.gfxData.DrawLine(pen, (float)cap1Px, (float)error2px, (float)cap2Px, (float)error2px);
        }

        private void RenderMeanMarker(Settings settings, PopulationStats population, double positionLeft, double positionRight,
            System.Drawing.Brush brush, double markerSize = 5)
        {
            double positionCenter = (positionLeft + positionRight) / 2;
            double xPx = settings.GetPixelX(positionCenter);
            double yPx = settings.GetPixelY(population.mean);
            settings.gfxData.FillEllipse(brush, (float)(xPx - markerSize), (float)(yPx - markerSize), (float)(markerSize * 2), (float)(markerSize * 2));
        }

        private void RenderScatter(Settings settings, PopulationStats population, double positionLeft, double positionRight, Random rand,
            System.Drawing.Pen pen, double spread = .5, double radius = 5)
        {
            double positionCenter = (positionLeft + positionRight) / 2;
            double spreadValue = spread * (positionRight - positionLeft);
            foreach (double value in population.values)
            {
                double valuePixel = settings.GetPixelY(value);
                double positionPixel = settings.GetPixelX(positionCenter + (rand.NextDouble() - .5) * spreadValue);

                System.Drawing.RectangleF rect = new System.Drawing.RectangleF(
                        x: (float)(positionPixel - radius),
                        y: (float)(valuePixel - radius),
                        width: (float)(radius * 2),
                        height: (float)(radius * 2)
                    );
                settings.gfxData.DrawEllipse(pen, rect);
            }
        }
    }
}
