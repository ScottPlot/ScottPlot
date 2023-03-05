using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// A collection of X/Y coordinates that can be displayed as markers and/or connected lines.
    /// Unlike the regular ScatterPlot, this plot type has Add() methods to easily add data.
    /// </summary>
    public class ScatterPlotList<T> : IPlottable
    {
        protected readonly List<T> Xs = new();
        protected readonly List<T> Ys = new();
        public int Count => Xs.Count;

        public string Label { get; set; }
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public Color Color { get; set; } = Color.Black;
        public float LineWidth { get; set; } = 1;
        public LineStyle LineStyle { get; set; } = LineStyle.Solid;
        public float MarkerSize { get; set; } = 3;
        public MarkerShape MarkerShape { get; set; } = MarkerShape.filledCircle;

        /// <summary>
        /// Defines behavior when <see cref="Xs"/> or <see cref="Ys"/> contains <see cref="double.NaN"/>
        /// </summary>
        public ScatterPlot.NanBehavior OnNaN = ScatterPlot.NanBehavior.Throw;

        /// <summary>
        /// If enabled, scatter plot points will be connected by square corners rather than straight diagnal lines
        /// </summary>
        public bool StepDisplay { get; set; } = false;

        /// <summary>
        /// Describes orientation of steps if <see cref="StepDisplay"/> is enabled.
        /// If true, lines will extend to the right before ascending or descending to the level of the following point.
        /// </summary>
        public bool StepDisplayRight { get; set; } = true;

        /// <summary>
        /// If enabled, points will be connected by smooth lines instead of straight diagnal lines.
        /// <see cref="SmoothTension"/> adjusts the smoothnes of the lines.
        /// </summary>
        public bool Smooth = false;

        /// <summary>
        /// Tension to use for smoothing when <see cref="Smooth"/> is enabled
        /// </summary>
        public double SmoothTension = 0.5;

        public void ValidateData(bool deep = false)
        {
            if (Xs.Count != Ys.Count)
                throw new InvalidOperationException("Xs and Ys must be same length");
        }

        /// <summary>
        /// Clear the list of points
        /// </summary>
        public void Clear()
        {
            Xs.Clear();
            Ys.Clear();
        }

        /// <summary>
        /// Add a single point to the list
        /// </summary>
        public void Add(T x, T y)
        {
            Xs.Add(x);
            Ys.Add(y);
        }

        /// <summary>
        /// Add multiple points to the list
        /// </summary>
        public void AddRange(T[] xs, T[] ys)
        {
            if (xs is null)
                throw new ArgumentException("xs must not be null");
            if (ys is null)
                throw new ArgumentException("ys must not be null");
            if (xs.Length != ys.Length)
                throw new ArgumentException("xs and ys must have the same length");

            Xs.AddRange(xs);
            Ys.AddRange(ys);
        }

        public AxisLimits GetAxisLimits()
        {
            if (Count == 0)
                return AxisLimits.NoLimits;

            var xs = Xs.Select(x => NumericConversion.GenericToDouble(ref x));
            var ys = Ys.Select(y => NumericConversion.GenericToDouble(ref y));

            if (xs.Any() == false || ys.Any() == false)
                return AxisLimits.NoLimits;

            if (OnNaN == ScatterPlot.NanBehavior.Throw)
            {
                double xMin = xs.Min();
                double xMax = xs.Max();
                double yMin = ys.Min();
                double yMax = ys.Max();

                if (double.IsNaN(xMin + xMax + yMin + yMax))
                    throw new InvalidOperationException($"Data may not contain NaN unless {nameof(OnNaN)} is changed");

                return new AxisLimits(xMin, xMax, yMin, yMax);
            }
            else
            {
                xs = xs.Where(x => double.IsNaN(x));
                ys = ys.Where(y => double.IsNaN(y));

                if (xs.Any() == false || ys.Any() == false)
                    return AxisLimits.NoLimits;

                double xMin = xs.Min();
                double xMax = xs.Max();
                double yMin = ys.Min();
                double yMax = ys.Max();

                return new AxisLimits(xMin, xMax, yMin, yMax);
            }
        }

        /// <summary>
        /// Return a new array containing pixel locations for each point of the scatter plot
        /// </summary>
        private PointF[] GetPoints(PlotDimensions dims)
        {
            return Enumerable.Range(0, Count)
                .Select(i => Coordinate.FromGeneric(Xs[i], Ys[i]))
                .Select(coord => coord.ToPixel(dims))
                .Select(px => new PointF(px.X, px.Y))
                .ToArray();
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            PointF[] points = GetPoints(dims);

            using var gfx = GDI.Graphics(bmp, dims, lowQuality);
            using var penLine = GDI.Pen(Color, LineWidth, LineStyle, true);

            if (OnNaN == ScatterPlot.NanBehavior.Throw)
            {
                foreach (PointF point in points)
                {
                    if (float.IsNaN(point.X) || float.IsNaN(point.Y))
                        throw new NotImplementedException($"Data must not contain NaN if {nameof(OnNaN)} is {OnNaN}");
                }

                DrawLines(points, gfx, penLine);
            }
            else if (OnNaN == ScatterPlot.NanBehavior.Ignore)
            {
                DrawLinesIngoringNaN(points, gfx, penLine);
            }
            else if (OnNaN == ScatterPlot.NanBehavior.Gap)
            {
                DrawLinesWithGaps(points, gfx, penLine);
            }

            if (MarkerShape != MarkerShape.none && MarkerSize > 0 && Count > 0)
                MarkerTools.DrawMarkers(gfx, points, MarkerShape, MarkerSize, Color);
        }

        private void DrawLines(PointF[] points, Graphics gfx, Pen penLine)
        {
            bool isLineVisible = LineWidth > 0 && points.Length > 1 && LineStyle != LineStyle.None;
            if (!isLineVisible)
                return;

            if (StepDisplay)
            {
                PointF[] pointsStep = ScatterPlot.GetStepDisplayPoints(points, StepDisplayRight);
                gfx.DrawLines(penLine, pointsStep);
            }
            else if (Smooth)
            {
                gfx.DrawCurve(penLine, points, (float)SmoothTension);
            }
            else
            {
                gfx.DrawLines(penLine, points);
            }
        }


        private void DrawLinesIngoringNaN(PointF[] points, Graphics gfx, Pen penLine)
        {
            PointF[] pointsWithoutNaNs = points.Where(pt => !double.IsNaN(pt.X) && !double.IsNaN(pt.Y)).ToArray();
            DrawLines(pointsWithoutNaNs, gfx, penLine);
        }

        private void DrawLinesWithGaps(PointF[] points, Graphics gfx, Pen penLine)
        {
            List<PointF> segment = new();
            for (int i = 0; i < points.Length; i++)
            {
                if (double.IsNaN(points[i].X) || double.IsNaN(points[i].Y))
                {
                    if (segment.Any())
                    {
                        DrawLines(segment.ToArray(), gfx, penLine);
                        segment.Clear();
                    }
                }
                else
                {
                    segment.Add(points[i]);
                }
            }

            if (segment.Any())
            {
                DrawLines(segment.ToArray(), gfx, penLine);
            }
        }

        public LegendItem[] GetLegendItems()
        {
            var singleItem = new LegendItem(this)
            {
                label = Label,
                color = Color,
                lineStyle = LineStyle,
                lineWidth = LineWidth,
                markerShape = MarkerShape,
                markerSize = MarkerSize
            };
            return LegendItem.Single(singleItem);
        }
    }
}
