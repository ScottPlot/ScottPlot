using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// A tooltip displays a text bubble pointing to a specific location in X/Y space.
    /// The position of the bubble moves according to the axis limits to best display the text in the data area.
    /// </summary>
    public class Tooltip : IPlottable, IHasColor, IHittable
    {
        public string Label { get; set; }
        public bool IsVisible { get; set; } = true;
        public Color BorderColor { get; set; } = Color.DarkGray;
        public float BorderWidth { get; set; } = 2;
        public Color FillColor { get; set; } = Color.White;
        public Color Color { get => FillColor; set => FillColor = value; }
        public int XAxisIndex { get; set; }
        public int YAxisIndex { get; set; }

        public readonly Drawing.Font Font = new();
        public int ArrowSize { get; set; } = 5;
        public int LabelPadding { get; set; } = 10;

        /// <summary>
        /// Tooltip position in coordinate space
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// Tooltip position in coordinate space
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Cursor to display when the tooltip is beneath the mouse
        /// </summary>
        public Cursor HitCursor { get; set; } = Cursor.Hand;

        public bool HitTestEnabled { get; set; } = true;

        public LegendItem[] GetLegendItems() => LegendItem.None;

        public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

        public void ValidateData(bool deep = false)
        {
            if (string.IsNullOrEmpty(Label))
                throw new InvalidOperationException("Label may not be empty");

            if (double.IsNaN(X) || double.IsInfinity(X))
                throw new InvalidOperationException("X must be a real number");

            if (double.IsNaN(Y) || double.IsInfinity(Y))
                throw new InvalidOperationException("Y must be a real number");
        }

        /// <summary>
        /// Bounding box of the tooltip the last time is was rendered (in coordinate units)
        /// </summary>
        private CoordinateRect LastRenderRect = new(0, 0, 0, 0);

        public bool HitTest(Coordinate coord) => HitTestEnabled ? LastRenderRect.Contains(coord) : false;

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            if (!IsVisible)
                return;

            using (var gfx = GDI.Graphics(bmp, dims, lowQuality, clipToDataArea: true))
            using (var font = GDI.Font(Font))
            using (var fillBrush = GDI.Brush(FillColor))
            using (var fontBrush = GDI.Brush(Font.Color))
            using (var pen = GDI.Pen(BorderColor, BorderWidth))
            {
                SizeF labelSize = gfx.MeasureString(Label, font);

                bool labelIsOnRight = dims.DataWidth - dims.GetPixelX(X) - labelSize.Width > 0;
                int sign = labelIsOnRight ? 1 : -1;

                PointF arrowHeadLocation = new PointF(dims.GetPixelX(X), dims.GetPixelY(Y));

                float contentBoxInsideEdgeX = arrowHeadLocation.X + sign * ArrowSize;
                PointF upperArrowVertex = new PointF(contentBoxInsideEdgeX, arrowHeadLocation.Y - ArrowSize);
                PointF lowerArrowVertex = new PointF(contentBoxInsideEdgeX, arrowHeadLocation.Y + ArrowSize);

                float contentBoxTopEdge = upperArrowVertex.Y - LabelPadding;
                float contentBoxBottomEdge = Math.Max(contentBoxTopEdge + labelSize.Height, lowerArrowVertex.Y) + 2 * LabelPadding;

                PointF[] points =
                {
                    arrowHeadLocation,
                    upperArrowVertex,
                    new PointF(contentBoxInsideEdgeX, upperArrowVertex.Y - LabelPadding),
                    new PointF(contentBoxInsideEdgeX + sign * (labelSize.Width + LabelPadding), upperArrowVertex.Y - LabelPadding),
                    new PointF(contentBoxInsideEdgeX + sign * (labelSize.Width + LabelPadding), contentBoxBottomEdge),
                    new PointF(contentBoxInsideEdgeX, contentBoxBottomEdge),
                    lowerArrowVertex,
                    arrowHeadLocation,
                    // add one more point to prevent render artifacts where thick line ends meet
                    upperArrowVertex,
                };

                byte[] pathPointTypes = Enumerable.Range(0, points.Length).Select(_ => (byte)System.Drawing.Drawing2D.PathPointType.Line).ToArray();

                var path = new System.Drawing.Drawing2D.GraphicsPath(points, pathPointTypes);

                gfx.FillPath(fillBrush, path);
                gfx.DrawPath(pen, path);

                float labelOffsetX = labelIsOnRight ? 0 : -labelSize.Width;
                float labelX = contentBoxInsideEdgeX + labelOffsetX + sign * LabelPadding / 2;
                float labelY = upperArrowVertex.Y;
                gfx.DrawString(Label, font, fontBrush, labelX, labelY);

                // calculate where the tooltip is in coordinate units and save it for later hit detection
                Coordinate[] corners = points.Select(pt => dims.GetCoordinate(pt.X, pt.Y)).ToArray();
                LastRenderRect = CoordinateRect.BoundingBox(corners);
            }
        }
    }
}
