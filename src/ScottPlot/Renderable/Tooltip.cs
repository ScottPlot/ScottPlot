using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Renderable
{
    public class Tooltip : IRenderable
    {
        public string Label { get; set; }
        public bool IsVisible { get; set; } = true;
        public Color BorderColor { get; set; } = Color.DarkGray;
        public float BorderWidth = 2;
        public Color FillColor { get; set; } = Color.White;
        public readonly Drawing.Font Font = new Drawing.Font();

        /// <summary>
        /// Tooltip position in coordinate space
        /// </summary>
        public double X;

        /// <summary>
        /// Tooltip position in coordinate space
        /// </summary>
        public double Y;

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            const int arrowHeadSideLength = 5;
            const int padding = 10;

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

                float contentBoxInsideEdgeX = arrowHeadLocation.X + sign * arrowHeadSideLength;
                PointF upperArrowVertex = new PointF(contentBoxInsideEdgeX, arrowHeadLocation.Y - arrowHeadSideLength);
                PointF lowerArrowVertex = new PointF(contentBoxInsideEdgeX, arrowHeadLocation.Y + arrowHeadSideLength);

                float contentBoxTopEdge = upperArrowVertex.Y - padding;
                float contentBoxBottomEdge = Math.Max(contentBoxTopEdge + labelSize.Height, lowerArrowVertex.Y) + 2 * padding;

                PointF[] points =
                {
                    arrowHeadLocation,
                    upperArrowVertex,
                    new PointF(contentBoxInsideEdgeX, upperArrowVertex.Y - padding),
                    new PointF(contentBoxInsideEdgeX + sign * (labelSize.Width + padding), upperArrowVertex.Y - padding),
                    new PointF(contentBoxInsideEdgeX + sign * (labelSize.Width + padding), contentBoxBottomEdge),
                    new PointF(contentBoxInsideEdgeX, contentBoxBottomEdge),
                    lowerArrowVertex,
                    arrowHeadLocation
                };

                byte[] pathPointTypes = Enumerable.Range(0, points.Length).Select(_ => (byte)System.Drawing.Drawing2D.PathPointType.Line).ToArray();

                var path = new System.Drawing.Drawing2D.GraphicsPath(points, pathPointTypes);

                gfx.DrawPath(pen, path);
                gfx.FillPath(fillBrush, path);

                float labelOffsetX = labelIsOnRight ? 0 : -labelSize.Width;
                float labelX = contentBoxInsideEdgeX + labelOffsetX + sign * padding / 2;
                float labelY = upperArrowVertex.Y;
                gfx.DrawString(Label, font, fontBrush, labelX, labelY);
            }
        }
    }
}
