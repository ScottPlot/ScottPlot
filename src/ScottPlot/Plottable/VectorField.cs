using ScottPlot.Drawing;
using ScottPlot.Statistics;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// The VectorField displays arrows representing a 2D array of 2D vectors
    /// </summary>
    public class VectorField : IPlottable
    {
        // data
        private readonly double[] Xs;
        private readonly double[] Ys;
        private readonly Vector2[,] Vectors;
        private readonly Color[] VectorColors;

        public string Label;
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        public enum VectorAnchor { Base, Center, Tip, }

        /// <summary>
        /// Describes which part of the vector line will be placed at the data coordinates.
        /// </summary>
        public VectorAnchor Anchor = VectorAnchor.Center;

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
        /// Marker drawn at each coordinate
        /// </summary>
        public MarkerShape MarkerShape = MarkerShape.filledCircle;

        /// <summary>
        /// Size of markers to be drawn at each coordinate
        /// </summary>
        public float MarkerSize = 0;

        public VectorField(Vector2[,] vectors, double[] xs, double[] ys, Colormap colormap, double scaleFactor, Color defaultColor)
        {
            double minMagnitudeSquared = vectors[0, 0].LengthSquared();
            double maxMagnitudeSquared = vectors[0, 0].LengthSquared();
            for (int i = 0; i < xs.Length; i++)
            {
                for (int j = 0; j < ys.Length; j++)
                {
                    if (vectors[i, j].LengthSquared() > maxMagnitudeSquared)
                        maxMagnitudeSquared = vectors[i, j].LengthSquared();
                    else if (vectors[i, j].LengthSquared() < minMagnitudeSquared)
                        minMagnitudeSquared = vectors[i, j].LengthSquared();
                }
            }
            double minMagnitude = Math.Sqrt(minMagnitudeSquared);
            double maxMagnitude = Math.Sqrt(maxMagnitudeSquared);

            double[,] intensities = new double[xs.Length, ys.Length];
            for (int i = 0; i < xs.Length; i++)
            {
                for (int j = 0; j < ys.Length; j++)
                {
                    if (colormap != null)
                        intensities[i, j] = (vectors[i, j].Length() - minMagnitude) / (maxMagnitude - minMagnitude);
                    vectors[i, j] = Vector2.Multiply(vectors[i, j], (float)(scaleFactor / (maxMagnitude * 1.2)));
                }
            }

            double[] flattenedIntensities = intensities.Cast<double>().ToArray();
            VectorColors = colormap is null ?
                Enumerable.Range(0, flattenedIntensities.Length).Select(x => defaultColor).ToArray() :
                Colormap.GetColors(flattenedIntensities, colormap);

            this.Vectors = vectors;
            this.Xs = xs;
            this.Ys = ys;
        }

        public void ValidateData(bool deep = false) { /* validation occurs in constructor */ }

        public LegendItem[] GetLegendItems()
        {
            var singleLegendItem = new LegendItem()
            {
                label = Label,
                color = VectorColors[0],
                lineWidth = 10,
                markerShape = MarkerShape.none
            };
            return new LegendItem[] { singleLegendItem };
        }

        public AxisLimits GetAxisLimits() => new AxisLimits(Xs.Min() - 1, Xs.Max() + 1, Ys.Min() - 1, Ys.Max() + 1);

        public int PointCount { get => Vectors.Length; }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (Pen pen = GDI.Pen(Color.Black))
            {
                float tipScale = (float)Math.Sqrt(ScaledArrowheadLength * ScaledArrowheadLength + ScaledArrowheadWidth * ScaledArrowheadWidth);
                float headAngle = (float)Math.Atan2(ScaledArrowheadWidth, ScaledArrowheadLength);

                if (!ScaledArrowheads)
                    pen.CustomEndCap = new AdjustableArrowCap(2, 2);

                for (int i = 0; i < Xs.Length; i++)
                {
                    for (int j = 0; j < Ys.Length; j++)
                    {
                        Vector2 v = Vectors[i, j];
                        float tailX, tailY, endX, endY;

                        switch (Anchor)
                        {
                            case VectorAnchor.Base:
                                tailX = dims.GetPixelX(Xs[i]);
                                tailY = dims.GetPixelY(Ys[j]);
                                endX = dims.GetPixelX(Xs[i] + v.X);
                                endY = dims.GetPixelY(Ys[j] + v.Y);
                                break;
                            case VectorAnchor.Center:
                                tailX = dims.GetPixelX(Xs[i] - v.X / 2);
                                tailY = dims.GetPixelY(Ys[j] - v.Y / 2);
                                endX = dims.GetPixelX(Xs[i] + v.X / 2);
                                endY = dims.GetPixelY(Ys[j] + v.Y / 2);
                                break;
                            case VectorAnchor.Tip:
                                tailX = dims.GetPixelX(Xs[i] - v.X);
                                tailY = dims.GetPixelY(Ys[j] - v.Y);
                                endX = dims.GetPixelX(Xs[i]);
                                endY = dims.GetPixelY(Ys[j]);
                                break;
                            default:
                                throw new NotImplementedException("unsupported anchor type");
                        }

                        pen.Color = VectorColors[i * Ys.Length + j];
                        if (ScaledArrowheads)
                            DrawFancyArrow(gfx, pen, tailX, tailY, endX, endY, headAngle, tipScale);
                        else
                            gfx.DrawLine(pen, tailX, tailY, endX, endY);

                        if (MarkerShape != MarkerShape.none && MarkerSize > 0)
                        {
                            PointF markerPoint = new PointF(dims.GetPixelX(Xs[i]), dims.GetPixelY(Ys[j]));
                            MarkerTools.DrawMarker(gfx, markerPoint, MarkerShape, MarkerSize, pen.Color);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Draw arrow with tips without using CustomEndCap.
        /// </summary>
        /// <param name="gfx"></param>
        /// <param name="pen"></param>
        /// <param name="baseX"></param>
        /// <param name="baseY"></param>
        /// <param name="tipX"></param>
        /// <param name="tipY"></param>
        /// <param name="headAngle">Determines how 'pointy' the tip is.</param>
        /// <param name="tipScale">Length of tip part, relative to total length.</param>
        private void DrawFancyArrow(
           Graphics gfx, Pen pen,
           float baseX,
           float baseY,
           float tipX,
           float tipY,
           float headAngle,
           float tipScale)
        {
            var dx = tipX - baseX;
            var dy = tipY - baseY;
            var arrowAngle = (float)Math.Atan2(dy, dx);
            var sinA1 = (float)Math.Sin(headAngle - arrowAngle);
            var cosA1 = (float)Math.Cos(headAngle - arrowAngle);
            var sinA2 = (float)Math.Sin(headAngle + arrowAngle);
            var cosA2 = (float)Math.Cos(headAngle + arrowAngle);
            var len = (float)Math.Sqrt(dx * dx + dy * dy);
            var hypLen = len * tipScale;

            var corner1X = tipX - hypLen * cosA1;
            var corner1Y = tipY + hypLen * sinA1;
            var corner2X = tipX - hypLen * cosA2;
            var corner2Y = tipY - hypLen * sinA2;

            PointF[] arrowPoints =
            {
                new PointF(baseX, baseY),
                new PointF(tipX, tipY),
                new PointF(corner1X, corner1Y),
                new PointF(tipX, tipY),
                new PointF(corner2X, corner2Y),
            };
            gfx.DrawLines(pen, arrowPoints);
        }



        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottableVectorField{label} with {PointCount} vectors";
        }
    }
}
