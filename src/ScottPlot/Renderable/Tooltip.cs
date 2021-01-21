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

            using (var gfx = GDI.Graphics(bmp, lowQuality))
            using (var font = GDI.Font(Font))
            using (var fillBrush = GDI.Brush(FillColor))
            using (var fontBrush = GDI.Brush(Font.Color))
            using (var pen = GDI.Pen(BorderColor, BorderWidth))
            {
                PointF topLeftCorner = new PointF(dims.GetPixelX(dims.XMin), dims.GetPixelY(dims.YMax));
                SizeF size = new SizeF(dims.GetPixelX(dims.XMax) - topLeftCorner.X, dims.GetPixelY(dims.YMin) - topLeftCorner.Y);
                var clipRect = new RectangleF(topLeftCorner, size);
                gfx.Clip = new Region(clipRect);

                SizeF contentSize = gfx.MeasureString(Label, font);

                // Negative draws in reverse, i.e. the content is left of the point
                int sign = Math.Sign(dims.DataWidth - dims.GetPixelX(X) - contentSize.Width); 

                PointF arrowHeadLocation = new PointF(dims.GetPixelX(X), dims.GetPixelY(Y));

                float contentBoxInsideEdgeX = arrowHeadLocation.X + sign * arrowHeadSideLength;

                PointF upperArrowVertex = new PointF(contentBoxInsideEdgeX, arrowHeadLocation.Y - arrowHeadSideLength);
                PointF lowerArrowVertex = new PointF(contentBoxInsideEdgeX, arrowHeadLocation.Y + arrowHeadSideLength);

                float contentBoxTopEdge = upperArrowVertex.Y - padding;
                float contentBoxBottomEdge = Math.Max(contentBoxTopEdge + contentSize.Height, lowerArrowVertex.Y) + 2 * padding;

                PointF[] points =
                {
                    arrowHeadLocation,
                    upperArrowVertex,
                    new PointF(contentBoxInsideEdgeX, upperArrowVertex.Y - padding),
                    new PointF(contentBoxInsideEdgeX + sign * (contentSize.Width + padding), upperArrowVertex.Y - padding),
                    new PointF(contentBoxInsideEdgeX + sign * (contentSize.Width + padding), contentBoxBottomEdge),
                    new PointF(contentBoxInsideEdgeX, contentBoxBottomEdge),
                    lowerArrowVertex,
                    arrowHeadLocation
                };

                byte[] pathPointTypes = Enumerable.Range(0, points.Length).Select(_ => (byte)System.Drawing.Drawing2D.PathPointType.Line).ToArray();

                var path = new System.Drawing.Drawing2D.GraphicsPath(points, pathPointTypes);

                gfx.DrawPath(pen, path);
                gfx.FillPath(fillBrush, path);

                float xOffset = sign != -1 ? 0 : -contentSize.Width;
                gfx.DrawString(Label, font, fontBrush, new PointF(contentBoxInsideEdgeX + xOffset + sign * padding / 2, upperArrowVertex.Y));
            }
        }
    }
}
