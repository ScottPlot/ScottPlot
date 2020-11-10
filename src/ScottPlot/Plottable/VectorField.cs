using ScottPlot.Ticks;
using ScottPlot.Drawing;
using ScottPlot.Renderable;
using ScottPlot.Statistics;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace ScottPlot.Plottable
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class VectorField : IRenderable, IHasLegendItems, IUsesAxes
    {
        private readonly Vector2[,] vectors;
        private readonly double[] xs;
        private readonly double[] ys;
        private readonly Color[] arrowColors;
        public string label;
        public bool IsVisible { get; set; } = true;
        public int HorizontalAxisIndex { get; set; } = 0;
        public int VerticalAxisIndex { get; set; } = 0;

        public VectorField(Vector2[,] vectors, double[] xs, double[] ys, Colormap colormap, double scaleFactor, Color defaultColor)
        {
            double minMagnitudeSquared = vectors[0, 0].Length();
            double maxMagnitudeSquared = vectors[0, 0].Length();
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
            arrowColors = colormap is null ?
                Enumerable.Range(0, flattenedIntensities.Length).Select(x => defaultColor).ToArray() :
                Colormap.GetColors(flattenedIntensities, colormap);

            this.vectors = vectors;
            this.xs = xs;
            this.ys = ys;
        }

        public LegendItem[] LegendItems
        {
            get
            {
                var item = new LegendItem()
                {
                    label = label,
                    color = arrowColors[0],
                    lineWidth = 10,
                    markerShape = MarkerShape.none
                };
                return new LegendItem[] { item };
            }
        }

        public AxisLimits2D GetAxisLimits() =>
            new AxisLimits2D(xs.Min() - 1, xs.Max() + 1, ys.Min() - 1, ys.Max() + 1);

        public int PointCount { get => vectors.Length; }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (Graphics gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (Pen pen = GDI.Pen(Color.Black))
            {
                pen.CustomEndCap = new AdjustableArrowCap(2, 2);
                for (int i = 0; i < xs.Length; i++)
                {
                    for (int j = 0; j < ys.Length; j++)
                    {
                        Vector2 v = vectors[i, j];
                        float tailX = dims.GetPixelX(xs[i] - v.X / 2);
                        float tailY = dims.GetPixelY(ys[j] - v.Y / 2);
                        float endX = dims.GetPixelX(xs[i] + v.X / 2);
                        float endY = dims.GetPixelY(v.Y + ys[j] - v.Y / 2);
                        pen.Color = arrowColors[i * ys.Length + j];
                        gfx.DrawLine(pen, tailX, tailY, endX, endY);
                    }
                }
            }
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableVectorField{label} with {PointCount} vectors";
        }
    }
}
