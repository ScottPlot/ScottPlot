﻿using System;
using System.Drawing;
using System.Linq;
using ScottPlot.Drawing;
using ScottPlot.Statistics;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Population plots are designed to show collections of data.
    /// A Population is a 1D array of values, and statistics are calculated automatically.
    /// Populations can be displayed as bar plots, box plots, or scatter plots.
    /// Public methods, fields, and properties allow extensive customization.
    /// This plottable supports higher-order grouping (groups of groups).
    /// </summary>
    public class PopulationPlot : IPlottable
    {
        public readonly PopulationMultiSeries MultiSeries;
        public int GroupCount { get { return MultiSeries.groupCount; } }
        public int SeriesCount { get { return MultiSeries.seriesCount; } }
        public string[] SeriesLabels { get { return MultiSeries.seriesLabels; } }
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public enum DisplayItems { BoxOnly, BoxAndScatter, ScatterAndBox, ScatterOnly };
        public enum BoxStyle { BarMeanStDev, BarMeanStdErr, BoxMeanStdevStderr, BoxMedianQuartileOutlier, MeanAndStdev, MeanAndStderr };
        public bool DistributionCurve = true;
        public LineStyle DistributionCurveLineStyle = LineStyle.Solid;
        public Color DistributionCurveColor = Color.Black;
        public Color ScatterOutlineColor = Color.Black;
        public DisplayItems DataFormat = DisplayItems.BoxAndScatter;
        public BoxStyle DataBoxStyle = BoxStyle.BoxMedianQuartileOutlier;

        public PopulationPlot(PopulationMultiSeries groupedSeries)
        {
            MultiSeries = groupedSeries;
        }

        public PopulationPlot(Population[] populations, string label = null, Color? color = null)
        {
            var ps = new PopulationSeries(populations, label, color ?? Color.LightGray);
            MultiSeries = new PopulationMultiSeries(new PopulationSeries[] { ps });
        }

        public PopulationPlot(PopulationSeries populationSeries)
        {
            MultiSeries = new PopulationMultiSeries(new PopulationSeries[] { populationSeries });
        }

        public PopulationPlot(Population population, string label = null, Color? color = null)
        {
            var populations = new Population[] { population };
            var ps = new PopulationSeries(populations, label, color ?? Color.LightGray);
            MultiSeries = new PopulationMultiSeries(new PopulationSeries[] { ps });
        }

        public override string ToString()
        {
            return $"PlottableSeries with {MultiSeries.groupCount} groups, {MultiSeries.seriesCount} series, and {PointCount} total points";
        }

        public int PointCount
        {
            get
            {
                int pointCount = 0;
                foreach (var group in MultiSeries.multiSeries)
                    foreach (var population in group.populations)
                        pointCount += population.count;
                return pointCount;
            }
        }

        public void ValidateData(bool deep = false)
        {
            if (MultiSeries is null)
                throw new InvalidOperationException("population multi-series cannot be null");
        }

        public LegendItem[] GetLegendItems() => MultiSeries.multiSeries
                .Select(x => new LegendItem() { label = x.seriesLabel, color = x.color, lineWidth = 10 })
                .ToArray();

        public AxisLimits GetAxisLimits()
        {
            double minValue = double.PositiveInfinity;
            double maxValue = double.NegativeInfinity;

            foreach (var series in MultiSeries.multiSeries)
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
            double positionMax = MultiSeries.groupCount - 1;

            // padd slightly
            positionMin -= .5;
            positionMax += .5;

            return new AxisLimits(positionMin, positionMax, minValue, maxValue);
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            Random rand = new Random(0);
            double groupWidth = .8;
            var popWidth = groupWidth / SeriesCount;

            for (int seriesIndex = 0; seriesIndex < SeriesCount; seriesIndex++)
            {
                for (int groupIndex = 0; groupIndex < GroupCount; groupIndex++)
                {
                    var series = MultiSeries.multiSeries[seriesIndex];
                    var population = series.populations[groupIndex];
                    var groupLeft = groupIndex - groupWidth / 2;
                    var popLeft = groupLeft + popWidth * seriesIndex;

                    Position scatterPos, boxPos;
                    switch (DataFormat)
                    {
                        case DisplayItems.BoxAndScatter:
                            boxPos = Position.Left;
                            scatterPos = Position.Right;
                            break;
                        case DisplayItems.BoxOnly:
                            boxPos = Position.Center;
                            scatterPos = Position.Hide;
                            break;
                        case DisplayItems.ScatterAndBox:
                            boxPos = Position.Right;
                            scatterPos = Position.Left;
                            break;
                        case DisplayItems.ScatterOnly:
                            boxPos = Position.Hide;
                            scatterPos = Position.Center;
                            break;
                        default:
                            throw new NotImplementedException();
                    }

                    Scatter(dims, bmp, lowQuality, population, rand, popLeft, popWidth, series.color, ScatterOutlineColor, 128, scatterPos);

                    if (DistributionCurve)
                        Distribution(dims, bmp, lowQuality, population, rand, popLeft, popWidth, DistributionCurveColor, scatterPos, DistributionCurveLineStyle);

                    switch (DataBoxStyle)
                    {
                        case BoxStyle.BarMeanStdErr:
                            Bar(dims, bmp, lowQuality, population, rand, popLeft, popWidth, series.color, boxPos, useStdErr: true);
                            break;
                        case BoxStyle.BarMeanStDev:
                            Bar(dims, bmp, lowQuality, population, rand, popLeft, popWidth, series.color, boxPos, useStdErr: false);
                            break;
                        case BoxStyle.BoxMeanStdevStderr:
                            Box(dims, bmp, lowQuality, population, rand, popLeft, popWidth, series.color, boxPos, BoxFormat.StdevStderrMean);
                            break;
                        case BoxStyle.BoxMedianQuartileOutlier:
                            Box(dims, bmp, lowQuality, population, rand, popLeft, popWidth, series.color, boxPos, BoxFormat.OutlierQuartileMedian);
                            break;
                        case BoxStyle.MeanAndStderr:
                            MeanAndError(dims, bmp, lowQuality, population, rand, popLeft, popWidth, series.color, boxPos, useStdErr: true);
                            break;
                        case BoxStyle.MeanAndStdev:
                            MeanAndError(dims, bmp, lowQuality, population, rand, popLeft, popWidth, series.color, boxPos, useStdErr: false);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }

        public enum Position { Hide, Center, Left, Right }

        private static void Scatter(PlotDimensions dims, Bitmap bmp, bool lowQuality, Population pop, Random rand,
            double popLeft, double popWidth, Color fillColor, Color edgeColor, byte alpha, Position position)
        {
            // adjust edges to accomodate special positions
            if (position == Position.Hide) return;
            if (position == Position.Left || position == Position.Right) popWidth /= 2;
            if (position == Position.Right) popLeft += popWidth;

            // contract edges slightly to encourage padding between elements
            double edgePaddingFrac = 0.2;
            popLeft += popWidth * edgePaddingFrac;
            popWidth -= (popWidth * edgePaddingFrac) * 2;

            float radius = 5;

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (Pen penEdge = GDI.Pen(Color.FromArgb(alpha, edgeColor)))
            using (Brush brushFill = GDI.Brush(Color.FromArgb(alpha, fillColor)))
            {
                foreach (double value in pop.values)
                {
                    double yPx = dims.GetPixelY(value);
                    double xPx = dims.GetPixelX(popLeft + rand.NextDouble() * popWidth);
                    gfx.FillEllipse(brushFill, (float)(xPx - radius), (float)(yPx - radius), radius * 2, radius * 2);
                    gfx.DrawEllipse(penEdge, (float)(xPx - radius), (float)(yPx - radius), radius * 2, radius * 2);
                }
            }
        }

        private static void Distribution(PlotDimensions dims, Bitmap bmp, bool lowQuality, Population pop, Random rand,
            double popLeft, double popWidth, Color color, Position position, LineStyle lineStyle)
        {
            // adjust edges to accomodate special positions
            if (position == Position.Hide) return;
            if (position == Position.Left || position == Position.Right) popWidth /= 2;
            if (position == Position.Right) popLeft += popWidth;

            // contract edges slightly to encourage padding between elements
            double edgePaddingFrac = 0.2;
            popLeft += popWidth * edgePaddingFrac;
            popWidth -= (popWidth * edgePaddingFrac) * 2;

            double[] ys = DataGen.Range(pop.minus3stDev, pop.plus3stDev, dims.UnitsPerPxY);
            if (ys.Length == 0)
                return;
            double[] ysFrac = pop.GetDistribution(ys, normalize: false);

            PointF[] points = new PointF[ys.Length];
            for (int i = 0; i < ys.Length; i++)
            {
                float x = (float)dims.GetPixelX(popLeft + popWidth * ysFrac[i]);
                float y = (float)dims.GetPixelY(ys[i]);
                points[i] = new PointF(x, y);
            }

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (Pen pen = GDI.Pen(color, 1, lineStyle, true))
            {
                gfx.DrawLines(pen, points);
            }
        }

        private static void MeanAndError(PlotDimensions dims, Bitmap bmp, bool lowQuality, Population pop, Random rand,
            double popLeft, double popWidth, Color color, Position position, bool useStdErr = false)
        {
            // adjust edges to accomodate special positions
            if (position == Position.Hide) return;
            if (position == Position.Left || position == Position.Right) popWidth /= 2;
            if (position == Position.Right) popLeft += popWidth;

            // determine the center point and calculate bounds
            double centerX = popLeft + popWidth / 2;
            double xPx = dims.GetPixelX(centerX);
            double yPx = dims.GetPixelY(pop.mean);

            double errorMaxPx, errorMinPx;
            if (useStdErr)
            {
                errorMaxPx = dims.GetPixelY(pop.mean + pop.stdErr);
                errorMinPx = dims.GetPixelY(pop.mean - pop.stdErr);
            }
            else
            {
                errorMaxPx = dims.GetPixelY(pop.mean + pop.stDev);
                errorMinPx = dims.GetPixelY(pop.mean - pop.stDev);
            }

            // make cap width a fraction of available space
            double capWidthFrac = .38;
            double capWidth = popWidth * capWidthFrac;
            double capPx1 = dims.GetPixelX(centerX - capWidth / 2);
            double capPx2 = dims.GetPixelX(centerX + capWidth / 2);
            float radius = 5;

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (Pen pen = GDI.Pen(color, 2))
            using (Brush brush = GDI.Brush(color))
            {
                gfx.FillEllipse(brush, (float)(xPx - radius), (float)(yPx - radius), radius * 2, radius * 2);
                gfx.DrawLine(pen, (float)xPx, (float)errorMinPx, (float)xPx, (float)errorMaxPx);
                gfx.DrawLine(pen, (float)capPx1, (float)errorMinPx, (float)capPx2, (float)errorMinPx);
                gfx.DrawLine(pen, (float)capPx1, (float)errorMaxPx, (float)capPx2, (float)errorMaxPx);
            }
        }

        private static void Bar(PlotDimensions dims, Bitmap bmp, bool lowQuality, Population pop, Random rand,
            double popLeft, double popWidth, Color color, Position position, bool useStdErr = false)
        {
            // adjust edges to accomodate special positions
            if (position == Position.Hide) return;
            if (position == Position.Left || position == Position.Right) popWidth /= 2;
            if (position == Position.Right) popLeft += popWidth;

            // determine the center point and calculate bounds
            double centerX = popLeft + popWidth / 2;
            double xPx = dims.GetPixelX(centerX);
            double yPxTop = dims.GetPixelY(pop.mean);
            double yPxBase = dims.GetPixelY(0);

            double errorMaxPx, errorMinPx;
            if (useStdErr)
            {
                errorMaxPx = dims.GetPixelY(pop.mean + pop.stdErr);
                errorMinPx = dims.GetPixelY(pop.mean - pop.stdErr);
            }
            else
            {
                errorMaxPx = dims.GetPixelY(pop.mean + pop.stDev);
                errorMinPx = dims.GetPixelY(pop.mean - pop.stDev);
            }

            // make cap width a fraction of available space
            double capWidthFrac = .38;
            double capWidth = popWidth * capWidthFrac;
            double capPx1 = dims.GetPixelX(centerX - capWidth / 2);
            double capPx2 = dims.GetPixelX(centerX + capWidth / 2);

            // contract edges slightly to encourage padding between elements
            double edgePaddingFrac = 0.2;
            popLeft += popWidth * edgePaddingFrac;
            popWidth -= (popWidth * edgePaddingFrac) * 2;
            double leftPx = dims.GetPixelX(popLeft);
            double rightPx = dims.GetPixelX(popLeft + popWidth);

            RectangleF rect = new RectangleF((float)leftPx, (float)yPxTop, (float)(rightPx - leftPx), (float)(yPxBase - yPxTop));

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (Pen pen = GDI.Pen(Color.Black))
            using (Brush brush = GDI.Brush(color))
            {
                gfx.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
                gfx.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);
                gfx.DrawLine(pen, (float)xPx, (float)errorMinPx, (float)xPx, (float)errorMaxPx);
                gfx.DrawLine(pen, (float)capPx1, (float)errorMinPx, (float)capPx2, (float)errorMinPx);
                gfx.DrawLine(pen, (float)capPx1, (float)errorMaxPx, (float)capPx2, (float)errorMaxPx);
            }
        }

        public enum BoxFormat { StdevStderrMean, OutlierQuartileMedian }
        public enum HorizontalAlignment { Left, Center, Right }

        private static void Box(PlotDimensions dims, Bitmap bmp, bool lowQuality, Population pop, Random rand,
            double popLeft, double popWidth, Color color, Position position, BoxFormat boxFormat,
            HorizontalAlignment errorAlignment = HorizontalAlignment.Right)
        {
            // adjust edges to accomodate special positions
            if (position == Position.Hide) return;
            if (position == Position.Left || position == Position.Right) popWidth /= 2;
            if (position == Position.Right) popLeft += popWidth;

            double errorMaxPx, errorMinPx;
            double yPxTop, yPxBase;
            double yPx;
            if (boxFormat == BoxFormat.StdevStderrMean)
            {
                errorMaxPx = dims.GetPixelY(pop.mean + pop.stDev);
                errorMinPx = dims.GetPixelY(pop.mean - pop.stDev);
                yPxTop = dims.GetPixelY(pop.mean + pop.stdErr);
                yPxBase = dims.GetPixelY(pop.mean - pop.stdErr);
                yPx = dims.GetPixelY(pop.mean);
            }
            else if (boxFormat == BoxFormat.OutlierQuartileMedian)
            {
                errorMaxPx = dims.GetPixelY(pop.maxNonOutlier);
                errorMinPx = dims.GetPixelY(pop.minNonOutlier);
                yPxTop = dims.GetPixelY(pop.Q3);
                yPxBase = dims.GetPixelY(pop.Q1);
                yPx = dims.GetPixelY(pop.median);
            }
            else
            {
                throw new NotImplementedException();
            }

            // make cap width a fraction of available space
            double capWidthFrac = .38;
            double capWidth = popWidth * capWidthFrac;

            // contract edges slightly to encourage padding between elements
            double edgePaddingFrac = 0.2;
            popLeft += popWidth * edgePaddingFrac;
            popWidth -= (popWidth * edgePaddingFrac) * 2;
            double leftPx = dims.GetPixelX(popLeft);
            double rightPx = dims.GetPixelX(popLeft + popWidth);
            RectangleF rect = new RectangleF(
                x: (float)leftPx,
                y: (float)yPxTop,
                width: (float)(rightPx - leftPx),
                height: (float)(yPxBase - yPxTop));

            // determine location of errorbars and caps
            double capPx1, capPx2, errorPxX;
            switch (errorAlignment)
            {
                case HorizontalAlignment.Center:
                    double centerX = popLeft + popWidth / 2;
                    errorPxX = dims.GetPixelX(centerX);
                    capPx1 = dims.GetPixelX(centerX - capWidth / 2);
                    capPx2 = dims.GetPixelX(centerX + capWidth / 2);
                    break;
                case HorizontalAlignment.Right:
                    errorPxX = dims.GetPixelX(popLeft + popWidth);
                    capPx1 = dims.GetPixelX(popLeft + popWidth - capWidth / 2);
                    capPx2 = dims.GetPixelX(popLeft + popWidth);
                    break;
                case HorizontalAlignment.Left:
                    errorPxX = dims.GetPixelX(popLeft);
                    capPx1 = dims.GetPixelX(popLeft);
                    capPx2 = dims.GetPixelX(popLeft + capWidth / 2);
                    break;
                default:
                    throw new NotImplementedException();
            }

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (Pen pen = GDI.Pen(Color.Black))
            using (Brush brush = GDI.Brush(color))
            {
                // draw the box
                gfx.FillRectangle(brush, rect.X, rect.Y, rect.Width, rect.Height);
                gfx.DrawRectangle(pen, rect.X, rect.Y, rect.Width, rect.Height);

                // draw the line in the center
                gfx.DrawLine(pen, rect.X, (float)yPx, rect.X + rect.Width, (float)yPx);

                // draw errorbars and caps
                gfx.DrawLine(pen, (float)errorPxX, (float)errorMinPx, (float)errorPxX, rect.Y + rect.Height);
                gfx.DrawLine(pen, (float)errorPxX, (float)errorMaxPx, (float)errorPxX, rect.Y);
                gfx.DrawLine(pen, (float)capPx1, (float)errorMinPx, (float)capPx2, (float)errorMinPx);
                gfx.DrawLine(pen, (float)capPx1, (float)errorMaxPx, (float)capPx2, (float)errorMaxPx);
            }
        }
    }
}
