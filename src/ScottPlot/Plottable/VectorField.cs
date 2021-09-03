using ScottPlot.Drawing;
using ScottPlot.Statistics;
using System;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// The VectorField displays arrows representing a 2D array of 2D vectors
    /// </summary>
    public class VectorField : IPlottable
    {
        private readonly double[] Xs;
        private readonly double[] Ys;
        private readonly Vector2[,] Vectors;
        private readonly Color[] VectorColors;
        public readonly Renderable.ArrowStyle ArrowStyle = new();
        public string Label;
        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;

        [Obsolete("use ArrowStyle.Anchor", true)]
        public ArrowAnchor Anchor;

        [Obsolete("use ArrowStyle.ScaledArrowheads", true)]
        public bool ScaledArrowheads;

        [Obsolete("use ArrowStyle.ScaledArrowheadWidth", true)]
        public double ScaledArrowheadWidth;

        [Obsolete("use ArrowStyle.ScaledArrowheadLength", true)]
        public double ScaledArrowheadLength;

        [Obsolete("use ArrowStyle.MarkerShape", true)]
        public MarkerShape MarkerShape = MarkerShape.filledCircle;

        [Obsolete("use ArrowStyle.MarkerSize", true)]
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
            if (IsVisible == false)
                return;

            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality);

            ArrowStyle.Render(dims, gfx, Xs, Ys, Vectors, VectorColors);
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.Label) ? "" : $" ({this.Label})";
            return $"PlottableVectorField{label} with {PointCount} vectors";
        }
    }
}
