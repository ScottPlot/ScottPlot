using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Renderable
{
    public class ArrowStyle
    {
        /// <summary>
        /// Describes which part of the vector line will be placed at the data coordinates.
        /// </summary>
        public ArrowAnchor Anchor = ArrowAnchor.Center;

        /// <summary>
        /// If enabled arrowheads will be drawn as lines scaled to each vector's magnitude.
        /// </summary>
        public bool ScaledArrowheads;

        /// <summary>
        /// When using scaled arrowheads this defines the width of the arrow relative to the vector line's length.
        /// </summary>
        public double ScaledArrowheadWidth = 0.15;

        /// <summary>
        /// When using scaled arrowheads this defines length of the arrowhead relative to the vector line's length.
        /// </summary>
        public double ScaledArrowheadLength = 0.5;

        /// <summary>
        /// Length of the scaled arrowhead
        /// </summary>
        private double ScaledTipLength => Math.Sqrt(ScaledArrowheadLength * ScaledArrowheadLength + ScaledArrowheadWidth * ScaledArrowheadWidth);

        /// <summary>
        /// Width of the scaled arrowhead
        /// </summary>
        private double ScaledHeadAngle => Math.Atan2(ScaledArrowheadWidth, ScaledArrowheadLength);

        /// <summary>
        /// Size of the arrowhead if custom/scaled arrowheads are not in use
        /// </summary>
        public float NonScaledArrowheadWidth = 2;

        /// <summary>
        /// Size of the arrowhead if custom/scaled arrowheads are not in use
        /// </summary>
        public float NonScaledArrowheadLength = 2;

        /// <summary>
        /// Marker drawn at each coordinate
        /// </summary>
        public MarkerShape MarkerShape = MarkerShape.filledCircle;

        /// <summary>
        /// Size of markers to be drawn at each coordinate
        /// </summary>
        public float MarkerSize = 0;

        /// <summary>
        /// Thickness of the arrow lines
        /// </summary>
        public float LineWidth = 1;

        /// <summary>
        /// Render an evenly-spaced 2D vector field.
        /// </summary>
        public void Render(PlotDimensions dims, Graphics gfx, double[] xs, double[] ys, Statistics.Vector2[,] vectors, Color[] colors)
        {

            for (int i = 0; i < xs.Length; i++)
            {
                for (int j = 0; j < ys.Length; j++)
                {
                    Coordinate coordinate = new(xs[i], ys[j]);
                    CoordinateVector vector = new(vectors[i, j].X, vectors[i, j].Y);
                    Color color = colors[i * ys.Length + j];
                    RenderArrow(dims, gfx, coordinate, vector, color);
                }
            }
        }

        /// <summary>
        /// Render a single arrow placed anywhere in coordinace space
        /// </summary>
        public void RenderArrow(PlotDimensions dims, Graphics gfx, Coordinate pt, CoordinateVector vec, Color color)
        {
            double x = pt.X;
            double y = pt.Y;
            double xVector = vec.X;
            double yVector = vec.Y;

            float tailX, tailY, endX, endY;
            switch (Anchor)
            {
                case ArrowAnchor.Base:
                    tailX = dims.GetPixelX(x);
                    tailY = dims.GetPixelY(y);
                    endX = dims.GetPixelX(x + xVector);
                    endY = dims.GetPixelY(y + yVector);
                    break;
                case ArrowAnchor.Center:
                    tailX = dims.GetPixelX(x - xVector / 2);
                    tailY = dims.GetPixelY(y - yVector / 2);
                    endX = dims.GetPixelX(x + xVector / 2);
                    endY = dims.GetPixelY(y + yVector / 2);
                    break;
                case ArrowAnchor.Tip:
                    tailX = dims.GetPixelX(x - xVector);
                    tailY = dims.GetPixelY(y - yVector);
                    endX = dims.GetPixelX(x);
                    endY = dims.GetPixelY(y);
                    break;
                default:
                    throw new NotImplementedException("unsupported anchor type");
            }

            using Pen pen = Drawing.GDI.Pen(color, LineWidth, rounded: true);

            if (ScaledArrowheads)
            {
                DrawFancyArrow(gfx, pen, tailX, tailY, endX, endY);
            }
            else
            {
                DrawStandardArrow(gfx, pen, tailX, tailY, endX, endY);
            }

            DrawMarker(dims, gfx, pen, x, y);
        }

        private void DrawMarker(PlotDimensions dims, Graphics gfx, Pen pen, double x, double y)
        {
            if (MarkerShape != MarkerShape.none && MarkerSize > 0)
            {
                PointF markerPoint = new(dims.GetPixelX(x), dims.GetPixelY(y));
                MarkerTools.DrawMarker(gfx, markerPoint, MarkerShape, MarkerSize, pen.Color);
            }
        }

        private void DrawStandardArrow(Graphics gfx, Pen pen, float x1, float y1, float x2, float y2)
        {
            pen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(NonScaledArrowheadWidth, NonScaledArrowheadLength);
            gfx.DrawLine(pen, x1, y1, x2, y2);
        }

        private void DrawFancyArrow(Graphics gfx, Pen pen, float x1, float y1, float x2, float y2)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            var arrowAngle = (float)Math.Atan2(dy, dx);
            var sinA1 = (float)Math.Sin(ScaledHeadAngle - arrowAngle);
            var cosA1 = (float)Math.Cos(ScaledHeadAngle - arrowAngle);
            var sinA2 = (float)Math.Sin(ScaledHeadAngle + arrowAngle);
            var cosA2 = (float)Math.Cos(ScaledHeadAngle + arrowAngle);
            var len = (float)Math.Sqrt(dx * dx + dy * dy);
            var hypLen = len * ScaledTipLength;

            var corner1X = x2 - hypLen * cosA1;
            var corner1Y = y2 + hypLen * sinA1;
            var corner2X = x2 - hypLen * cosA2;
            var corner2Y = y2 - hypLen * sinA2;

            PointF[] arrowPoints =
            {
                new PointF(x1, y1),
                new PointF(x2, y2),
                new PointF((float)corner1X, (float)corner1Y),
                new PointF(x2, y2),
                new PointF((float)corner2X, (float)corner2Y),
            };

            gfx.DrawLines(pen, arrowPoints);
        }
    }
}
