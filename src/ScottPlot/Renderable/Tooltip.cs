using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace ScottPlot.Renderable
{
    public class Tooltip : IRenderable
    {
        public string Contents { get; set; }
        public bool IsVisible { get; set; } = true;
        public Color FontColor { get; set; } = Color.Black;
        public Color BorderColor { get; set; } = Color.DarkGray;
        public Color BackgroundColor { get; set; } = Color.White;
        public string FontName = InstalledFont.Sans();
        public float FontSize = 12;
        public (double X, double Y) Coordinates { get; set; }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            const int arrowHeadSideLength = 5;
            const int padding = 10;

            if (!IsVisible)
                return;

            using (var gfx = GDI.Graphics(bmp, lowQuality))
            using (var font = GDI.Font(FontName, FontSize))
            using (var fillBrush = GDI.Brush(BackgroundColor))
            using (var fontBrush = GDI.Brush(FontColor))
            using (var pen = GDI.Pen(BorderColor))
            {
                PointF topLeftCorner = new PointF(dims.GetPixelX(dims.XMin), dims.GetPixelY(dims.YMax));
                SizeF size = new SizeF(dims.GetPixelX(dims.XMax) - topLeftCorner.X, dims.GetPixelY(dims.YMin) - topLeftCorner.Y);
                var clipRect = new RectangleF(topLeftCorner, size);
                gfx.Clip = new Region(clipRect);

                SizeF contentSize = gfx.MeasureString(Contents, font);
                int sign = Math.Sign(dims.DataWidth - dims.GetPixelX(Coordinates.X) - contentSize.Width); // Negative draws in reverse, i.e. the content is left of the point

                PointF arrowHeadLocation = new PointF(dims.GetPixelX(Coordinates.X), dims.GetPixelY(Coordinates.Y));

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
                gfx.DrawString(Contents, font, fontBrush, new PointF(contentBoxInsideEdgeX + xOffset + sign * padding / 2, upperArrowVertex.Y));
            }
        }
    }
}
