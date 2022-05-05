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

            var xs = Xs.Select(x => Convert.ToDouble(x));
            var ys = Ys.Select(y => Convert.ToDouble(y));

            return new AxisLimits(xs.Min(), xs.Max(), ys.Min(), ys.Max());
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
            using var linePen = GDI.Pen(Color, LineWidth, LineStyle, true);

            if (LineStyle != LineStyle.None && LineWidth > 0 && Count > 1)
            {
                gfx.DrawLines(linePen, points);
            }

            if (MarkerShape != MarkerShape.none && MarkerSize > 0 && Count > 0)
            {
                foreach (PointF point in points)
                    MarkerTools.DrawMarker(gfx, point, MarkerShape, MarkerSize, Color);
            }
        }

        public LegendItem[] GetLegendItems()
        {
            var singleLegendItem = new LegendItem(this)
            {
                label = Label,
                color = Color,
                lineStyle = LineStyle,
                lineWidth = LineWidth,
                markerShape = MarkerShape,
                markerSize = MarkerSize
            };
            return new LegendItem[] { singleLegendItem };
        }
    }
}
