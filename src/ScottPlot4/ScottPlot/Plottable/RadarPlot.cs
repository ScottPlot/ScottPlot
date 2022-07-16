﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ScottPlot.Drawing;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// A radar chart is a graphical method of displaying multivariate data in the form of 
    /// a two-dimensional chart of three or more quantitative variables represented on axes 
    /// starting from the same point.
    /// 
    /// Data is managed using 2D arrays where groups (colored shapes) are rows and categories (arms of the web) are columns.
    /// </summary>
    public class RadarPlot : IPlottable
    {
        /// <summary>
        /// Values for every group (rows) and category (columns) normalized from 0 to 1.
        /// </summary>
        private double[,] Norm;

        /// <summary>
        /// Single value to normalize all values against for all groups/categories.
        /// </summary>
        private double NormMax;

        /// <summary>
        /// Individual values (one per category) to use for normalization.
        /// Length must be equal to the number of columns (categories) in the original data.
        /// </summary>
        private double[] NormMaxes;

        /// <summary>
        /// Labels for each category.
        /// Length must be equal to the number of columns (categories) in the original data.
        /// </summary>
        /// <remarks>
        /// If showing icons, labels will be ignored.
        /// </remarks>
        public string[] CategoryLabels { get; set; }

        /// <summary>
        /// Icons for each category.
        /// Length must be equal to the number of columns (categories) in the original data. 
        /// </summary>
        /// <remarks>
        /// If showing icons, labels will be ignored.
        /// </remarks>
        public System.Drawing.Image[] CategoryImages { get; set; }

        /// <summary>
        /// Labels for each group.
        /// Length must be equal to the number of rows (groups) in the original data.
        /// </summary>
        public string[] GroupLabels { get; set; }

        /// <summary>
        /// Colors (typically semi-transparent) to shade the inner area of each group.
        /// Length must be equal to the number of rows (groups) in the original data.
        /// </summary>
        public System.Drawing.Color[] FillColors { get; set; }

        /// <summary>
        /// Colors to outline the shape for each group.
        /// Length must be equal to the number of rows (groups) in the original data.
        /// </summary>
        public System.Drawing.Color[] LineColors { get; set; }

        /// <summary>
        /// Color of the axis lines and concentric circles representing ticks
        /// </summary>
        public System.Drawing.Color WebColor { get; set; } = System.Drawing.Color.Gray; // TODO: avoid this name in the future (see #1948)

        /// <summary>
        /// Contains options for hatched (patterned) fills for each slice
        /// </summary>
        public HatchOptions[] HatchOptions { get; set; }

        /// <summary>
        /// Controls if values along each category axis are scaled independently or uniformly across all axes.
        /// </summary>
        public bool IndependentAxes { get; set; }

        /// <summary>
        /// Font used for labeling values on the plot
        /// </summary>
        public readonly Drawing.Font Font = new();

        /// <summary>
        /// If true, each value will be written in text on the plot.
        /// </summary>
        public bool ShowAxisValues { get; set; } = true;

        /// <summary>
        /// If true, each category name will be written in text at every corner of the radar
        /// </summary>
        public bool ShowCategoryLabels { get; set; } = true;

        /// <summary>
        /// Controls rendering style of the concentric circles (ticks) of the web
        /// </summary>
        public RadarAxis AxisType { get; set; } = RadarAxis.Circle;

        /// <summary>
        /// Determines the width of each spoke and the axis lines.
        /// </summary>
        public int LineWidth { get; set; } = 1;

        /// <summary>
        /// Determines the width of the line at the edge of each area polygon.
        /// </summary>
        public float OutlineWidth { get; set; } = 1;

        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        public RadarPlot(double[,] values, System.Drawing.Color[] lineColors, System.Drawing.Color[] fillColors, bool independentAxes, double[] maxValues = null)
        {
            LineColors = lineColors;
            FillColors = fillColors;
            IndependentAxes = independentAxes;
            Update(values, independentAxes, maxValues);
        }

        public override string ToString() =>
            $"PlottableRadar with {PointCount} points and {Norm.GetUpperBound(1) + 1} categories.";

        /// <summary>
        /// Replace the data values with new ones.
        /// </summary>
        /// <param name="values">2D array of groups (rows) of values for each category (columns)</param>
        /// <param name="independentAxes">Controls if values along each category axis are scaled independently or uniformly across all axes</param>
        /// <param name="maxValues">If provided, these values will be used to normalize each category (columns)</param>
        public void Update(double[,] values, bool independentAxes = false, double[] maxValues = null)
        {
            IndependentAxes = independentAxes;
            Norm = new double[values.GetLength(0), values.GetLength(1)];
            Array.Copy(values, 0, Norm, 0, values.Length);

            if (IndependentAxes)
                NormMaxes = NormalizeSeveralInPlace(Norm, maxValues);
            else
                NormMax = NormalizeInPlace(Norm, maxValues);
        }

        public void ValidateData(bool deep = false)
        {
            if (GroupLabels != null && GroupLabels.Length != Norm.GetLength(0))
                throw new InvalidOperationException("group names must match size of values");

            if (CategoryLabels != null && CategoryLabels.Length != Norm.GetLength(1))
                throw new InvalidOperationException("category names must match size of values");
        }

        /// <summary>
        /// Normalize a 2D array by dividing all values by the maximum value.
        /// </summary>
        /// <returns>maximum value in the array before normalization</returns>
        private double NormalizeInPlace(double[,] input, double[] maxValues = null)
        {
            double max;
            if (maxValues != null && maxValues.Length == 1)
            {
                max = maxValues[0];
            }
            else
            {
                max = input[0, 0];
                for (int i = 0; i < input.GetLength(0); i++)
                    for (int j = 0; j < input.GetLength(1); j++)
                        max = Math.Max(max, input[i, j]);
            }

            for (int i = 0; i < input.GetLength(0); i++)
                for (int j = 0; j < input.GetLength(1); j++)
                    input[i, j] /= max;

            return max;
        }

        /// <summary>
        /// Normalize each row of a 2D array independently by dividing all values by the maximum value.
        /// </summary>
        /// <returns>maximum value in each row of the array before normalization</returns>
        private double[] NormalizeSeveralInPlace(double[,] input, double[] maxValues = null)
        {
            double[] maxes;
            if (maxValues != null && input.GetLength(1) == maxValues.Length)
            {
                maxes = maxValues;
            }
            else
            {
                maxes = new double[input.GetLength(1)];
                for (int i = 0; i < input.GetLength(1); i++)
                {
                    double max = input[0, i];
                    for (int j = 0; j < input.GetLength(0); j++)
                    {
                        max = Math.Max(input[j, i], max);
                    }
                    maxes[i] = max;
                }
            }

            for (int i = 0; i < input.GetLength(0); i++)
            {
                for (int j = 0; j < input.GetLength(1); j++)
                {
                    if (maxes[j] == 0)
                        input[i, j] = 0;
                    else
                        input[i, j] /= maxes[j];
                }
            }

            return maxes;
        }

        public LegendItem[] GetLegendItems()
        {
            if (GroupLabels is null)
                return Array.Empty<LegendItem>();

            List<LegendItem> legendItems = new List<LegendItem>();
            for (int i = 0; i < GroupLabels.Length; i++)
            {
                var item = new LegendItem(this)
                {
                    label = GroupLabels[i],
                    color = FillColors[i],
                    lineWidth = 10,
                    markerShape = MarkerShape.none,
                    hatchStyle = HatchOptions?[i].Pattern ?? Drawing.HatchStyle.None,
                    hatchColor = HatchOptions?[i].Color ?? System.Drawing.Color.Black
                };
                legendItems.Add(item);
            }

            return legendItems.ToArray();
        }

        public AxisLimits GetAxisLimits()
        {
            return (GroupLabels != null)
                ? new AxisLimits(-3.5, 3.5, -3.5, 3.5)
                : new AxisLimits(-2.5, 2.5, -2.5, 2.5);
        }

        public int PointCount { get => Norm.Length; }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            int numGroups = Norm.GetUpperBound(0) + 1;
            int numCategories = Norm.GetUpperBound(1) + 1;
            double sweepAngle = 2 * Math.PI / numCategories;
            double minScale = new double[] { dims.PxPerUnitX, dims.PxPerUnitX }.Min();
            PointF origin = new PointF(dims.GetPixelX(0), dims.GetPixelY(0));

            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (Pen pen = GDI.Pen(WebColor, OutlineWidth))
            using (StringFormat sf = new StringFormat() { LineAlignment = StringAlignment.Center })
            using (StringFormat sf2 = new StringFormat())
            using (System.Drawing.Font font = GDI.Font(Font))
            using (Brush fontBrush = GDI.Brush(Font.Color))
            {
                RenderAxis(gfx, dims, bmp, lowQuality);

                for (int i = 0; i < numGroups; i++)
                {
                    PointF[] points = new PointF[numCategories];
                    for (int j = 0; j < numCategories; j++)
                        points[j] = new PointF(
                            (float)(Norm[i, j] * Math.Cos(sweepAngle * j - Math.PI / 2) * minScale + origin.X),
                            (float)(Norm[i, j] * Math.Sin(sweepAngle * j - Math.PI / 2) * minScale + origin.Y));

                    using var brush = GDI.Brush(FillColors[i], HatchOptions?[i].Color, HatchOptions?[i].Pattern ?? Drawing.HatchStyle.None);
                    pen.Color = LineColors[i];
                    gfx.FillPolygon(brush, points);
                    gfx.DrawPolygon(pen, points);
                }
            }
        }

        private StarAxisTick GetTick(double location) =>
            IndependentAxes
                ? new StarAxisTick(location, NormMaxes.Select(x => x * location).ToArray())
                : new StarAxisTick(location, NormMax);

        private void RenderAxis(Graphics gfx, PlotDimensions dims, Bitmap bmp, bool lowQuality)
        {
            double[] tickLocations = new[] { 0.25, 0.5, 1 };
            StarAxisTick[] ticks = tickLocations.Select(x => GetTick(x)).ToArray();

            StarAxis axis = new()
            {
                Ticks = ticks,
                CategoryLabels = CategoryLabels,
                CategoryImages = CategoryImages,
                NumberOfSpokes = Norm.GetLength(1),
                AxisType = AxisType,
                WebColor = WebColor,
                LineWidth = LineWidth,
                ShowCategoryLabels = ShowCategoryLabels,
                LabelEachSpoke = IndependentAxes,
                ShowAxisValues = ShowAxisValues,
                Graphics = gfx,
                ImagePlacement = ImagePlacement.Outside
            };

            axis.Render(dims, bmp, lowQuality);
        }
    }
}
