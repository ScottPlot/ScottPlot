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

        public (float tipScale, float headAngle) GetTipDimensions()
        {
            float tipScale = (float)Math.Sqrt(ScaledArrowheadLength * ScaledArrowheadLength + ScaledArrowheadWidth * ScaledArrowheadWidth);
            float headAngle = (float)Math.Atan2(ScaledArrowheadWidth, ScaledArrowheadLength);
            return (tipScale, headAngle);
        }

        /// <summary>
        /// Render an evenly-spaced 2D vector field.
        /// </summary>
        public void Render(PlotDimensions dims, Graphics gfx, double[] xs, double[] ys, Statistics.Vector2[,] vectors, Color[] colors)
        {
            (float tipScale, float headAngle) = GetTipDimensions(); // precalculate angles for fancy arrows

            using Pen pen = Drawing.GDI.Pen(Color.Black);
            if (!ScaledArrowheads)
                pen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(NonScaledArrowheadWidth, NonScaledArrowheadLength);

            for (int i = 0; i < xs.Length; i++)
            {
                for (int j = 0; j < ys.Length; j++)
                {
                    Statistics.Vector2 v = vectors[i, j];
                    float tailX, tailY, endX, endY;

                    switch (Anchor)
                    {
                        case ArrowAnchor.Base:
                            tailX = dims.GetPixelX(xs[i]);
                            tailY = dims.GetPixelY(ys[j]);
                            endX = dims.GetPixelX(xs[i] + v.X);
                            endY = dims.GetPixelY(ys[j] + v.Y);
                            break;
                        case ArrowAnchor.Center:
                            tailX = dims.GetPixelX(xs[i] - v.X / 2);
                            tailY = dims.GetPixelY(ys[j] - v.Y / 2);
                            endX = dims.GetPixelX(xs[i] + v.X / 2);
                            endY = dims.GetPixelY(ys[j] + v.Y / 2);
                            break;
                        case ArrowAnchor.Tip:
                            tailX = dims.GetPixelX(xs[i] - v.X);
                            tailY = dims.GetPixelY(ys[j] - v.Y);
                            endX = dims.GetPixelX(xs[i]);
                            endY = dims.GetPixelY(ys[j]);
                            break;
                        default:
                            throw new NotImplementedException("unsupported anchor type");
                    }

                    pen.Color = colors[i * ys.Length + j];
                    if (ScaledArrowheads)
                        DrawFancyArrow(gfx, pen, tailX, tailY, endX, endY, headAngle, tipScale);
                    else
                        gfx.DrawLine(pen, tailX, tailY, endX, endY);

                    if (MarkerShape != MarkerShape.none && MarkerSize > 0)
                    {
                        PointF markerPoint = new PointF(dims.GetPixelX(xs[i]), dims.GetPixelY(ys[j]));
                        MarkerTools.DrawMarker(gfx, markerPoint, MarkerShape, MarkerSize, pen.Color);
                    }
                }
            }
        }

        private void DrawFancyArrow(Graphics gfx, Pen pen, float x1, float y1, float x2, float y2, float headAngle, float tipScale)
        {
            var dx = x2 - x1;
            var dy = y2 - y1;
            var arrowAngle = (float)Math.Atan2(dy, dx);
            var sinA1 = (float)Math.Sin(headAngle - arrowAngle);
            var cosA1 = (float)Math.Cos(headAngle - arrowAngle);
            var sinA2 = (float)Math.Sin(headAngle + arrowAngle);
            var cosA2 = (float)Math.Cos(headAngle + arrowAngle);
            var len = (float)Math.Sqrt(dx * dx + dy * dy);
            var hypLen = len * tipScale;

            var corner1X = x2 - hypLen * cosA1;
            var corner1Y = y2 + hypLen * sinA1;
            var corner2X = x2 - hypLen * cosA2;
            var corner2Y = y2 - hypLen * sinA2;

            PointF[] arrowPoints =
            {
                new PointF(x1, y1),
                new PointF(x2, y2),
                new PointF(corner1X, corner1Y),
                new PointF(x2, y2),
                new PointF(corner2X, corner2Y),
            };
            gfx.DrawLines(pen, arrowPoints);
        }
    }
}
