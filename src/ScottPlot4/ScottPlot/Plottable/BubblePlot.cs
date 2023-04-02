using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// Display circles of user-defined sizes and colors at specific X/Y positions
    /// </summary>
    public class BubblePlot : IPlottable
    {
        private struct Bubble
        {
            public double X;
            public double Y;
            public double Radius;
            public Color FillColor;
            public float EdgeWidth;
            public Color EdgeColor;
        }

        private readonly List<Bubble> Bubbles = new();

        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public override string ToString() => $"BubblePlot with {Bubbles.Count} bubbles";

        /// <summary>
        /// Indicates whether the size of each bubble is in pixel units
        /// or in axis units. If using axis units, users may want to enable
        /// the axis scale lock feature to enforce bubble circularity.
        /// </summary>
        public bool RadiusIsPixels { get; set; } = true;

        /// <summary>
        /// Clear all bubbles
        /// </summary>
        public void Clear() => Bubbles.Clear();

        /// <summary>
        /// Add a single bubble
        /// </summary>
        /// <param name="x">horizontal position (in coordinate space)</param>
        /// <param name="y">horizontal vertical (in coordinate space)</param>
        /// <param name="radius">size of the bubble (sized according to <see cref="RadiusIsPixels"/>)</param>
        /// <param name="fillColor">color of the bubble center</param>
        /// <param name="edgeWidth">size of the outline (in pixels)</param>
        /// <param name="edgeColor">color of the bubble outline</param>
        public void Add(double x, double y, double radius, Color fillColor, double edgeWidth, Color edgeColor)
        {
            Bubble bubble = new()
            {
                X = x,
                Y = y,
                Radius = (float)radius,
                FillColor = fillColor,
                EdgeWidth = (float)edgeWidth,
                EdgeColor = edgeColor
            };

            Bubbles.Add(bubble);
        }

        /// <summary>
        /// Add many bubbles with the same size and style
        /// </summary>
        public void Add(double[] xs, double[] ys, double radius, Color fillColor, double edgeWidth, Color edgeColor)
        {
            if (xs is null || ys is null)
                throw new ArgumentException("xs and ys cannot be null");

            if (xs.Length != ys.Length)
                throw new ArgumentException("xs and ys must have the same number of elements");

            for (int i = 0; i < xs.Length; i++)
            {
                Add(xs[i], ys[i], radius, fillColor, edgeWidth, edgeColor);
            }
        }

        public AxisLimits GetAxisLimits()
        {
            if (Bubbles.Count == 0)
                return AxisLimits.NoLimits;

            if (RadiusIsPixels)
            {
                var xs = Bubbles.Select(b => b.X);
                var ys = Bubbles.Select(b => b.Y);
                return new AxisLimits(xs.Min(), xs.Max(), ys.Min(), ys.Max());
            }
            else
            {
                return new AxisLimits(
                    xMin: Bubbles.Select(b => b.X - b.Radius).Min(),
                    xMax: Bubbles.Select(b => b.X + b.Radius).Max(),
                    yMin: Bubbles.Select(b => b.Y - b.Radius).Min(),
                    yMax: Bubbles.Select(b => b.Y + b.Radius).Max());
            }
        }

        public void ValidateData(bool deep = false)
        {
            foreach (Bubble bubble in Bubbles)
            {
                if (double.IsNaN(bubble.X) || double.IsNaN(bubble.Y) || double.IsNaN(bubble.Radius) || double.IsNaN(bubble.EdgeWidth))
                    throw new InvalidOperationException("Bubble positions and sizes must not be NaN");

                if (double.IsInfinity(bubble.X) || double.IsInfinity(bubble.Y) || double.IsInfinity(bubble.Radius) || double.IsInfinity(bubble.EdgeWidth))
                    throw new InvalidOperationException("Bubble position and size must real");

                if (bubble.Radius < 0 || bubble.EdgeWidth < 0)
                    throw new InvalidOperationException("Bubble sizes cannot be negative");
            }
        }

        public LegendItem[] GetLegendItems() => LegendItem.None;

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (IsVisible == false)
                return;

            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality);
            using Brush brush = GDI.Brush(Color.Magenta);
            using Pen pen = GDI.Pen(Color.Black);

            foreach (Bubble bubble in Bubbles)
            {
                RectangleF rect = RadiusIsPixels
                    ? GetRectPixelRadius(dims, bubble)
                    : GetCoordinateRadius(dims, bubble);

                ((SolidBrush)brush).Color = bubble.FillColor;
                gfx.FillEllipse(brush, rect);

                pen.Color = bubble.EdgeColor;
                pen.Width = bubble.EdgeWidth;
                gfx.DrawEllipse(pen, rect);
            }
        }

        private static RectangleF GetRectPixelRadius(PlotDimensions dims, Bubble bubble)
        {
            float pixelX = dims.GetPixelX(bubble.X);
            float pixelY = dims.GetPixelY(bubble.Y);
            float radiusPx = (float)bubble.Radius;
            PointF location = new(pixelX - radiusPx, pixelY - radiusPx);
            SizeF size = new(radiusPx * 2, radiusPx * 2);
            return new RectangleF(location, size);
        }

        private static RectangleF GetCoordinateRadius(PlotDimensions dims, Bubble bubble)
        {
            float left = dims.GetPixelX(bubble.X - bubble.Radius);
            float right = dims.GetPixelX(bubble.X + bubble.Radius);
            float bottom = dims.GetPixelY(bubble.Y - bubble.Radius);
            float top = dims.GetPixelY(bubble.Y + bubble.Radius);
            float width = right - left;
            float height = bottom - top;
            return new RectangleF(left, top, width, height);
        }

        /// <summary>
        /// Return the X/Y coordinates of the point nearest the X position
        /// </summary>
        /// <param name="x">X position in plot space</param>
        /// <returns></returns>
        public (double x, double y, int index) GetPointNearestX(double x)
        {
            if (Bubbles.Count == 0)
                throw new InvalidOperationException("BubblePlot is empty");

            double closestBubbleDistance = double.PositiveInfinity;
            int closestBubbleIndex = 0;
            for (int i = 0; i < Bubbles.Count; i++)
            {
                double currDistance = Math.Abs(Bubbles[i].X - x);
                if (currDistance < closestBubbleDistance)
                {
                    closestBubbleIndex = i;
                    closestBubbleDistance = currDistance;
                }
            }

            return (Bubbles[closestBubbleIndex].X, Bubbles[closestBubbleIndex].Y, closestBubbleIndex);
        }

        /// <summary>
        /// Return the X/Y coordinates of the point nearest the Y position
        /// </summary>
        /// <param name="y">Y position in plot space</param>
        /// <returns></returns>
        public (double x, double y, int index) GetPointNearestY(double y)
        {
            if (Bubbles.Count == 0)
                throw new InvalidOperationException("BubblePlot is empty");

            double closestBubbleDistance = double.PositiveInfinity;
            int closestBubbleIndex = 0;
            for (int i = 0; i < Bubbles.Count; i++)
            {
                double currDistance = Math.Abs(Bubbles[i].Y - y);
                if (currDistance < closestBubbleDistance)
                {
                    closestBubbleIndex = i;
                    closestBubbleDistance = currDistance;
                }
            }

            return (Bubbles[closestBubbleIndex].X, Bubbles[closestBubbleIndex].Y, closestBubbleIndex);
        }

        /// <summary>
        /// Return the position and index of the data point nearest the given coordinate
        /// </summary>
        /// <param name="x">location in coordinate space</param>
        /// <param name="y">location in coordinate space</param>
        /// <param name="xyRatio">Ratio of pixels per unit (X/Y) when rendered</param>
        public (double x, double y, int index) GetPointNearest(double x, double y, double xyRatio = 1)
        {
            if (Bubbles.Count == 0)
                throw new InvalidOperationException("BubblePlot is empty");

            double xyRatioSquared = xyRatio * xyRatio;
            double pointDistanceSquared(double x1, double y1) =>
                (x1 - x) * (x1 - x) * xyRatioSquared + (y1 - y) * (y1 - y);

            double closestBubbleDistance = double.PositiveInfinity;
            int closestBubbleIndex = 0;
            for (int i = 0; i < Bubbles.Count; i++)
            {
                double currDistance = pointDistanceSquared(Bubbles[i].X, Bubbles[i].Y);
                if (currDistance < closestBubbleDistance)
                {
                    closestBubbleIndex = i;
                    closestBubbleDistance = currDistance;
                }
            }

            return (Bubbles[closestBubbleIndex].X, Bubbles[closestBubbleIndex].Y, closestBubbleIndex);
        }
    }
}
