using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using ScottPlot.Config;
using ScottPlot.Drawing;
using ScottPlot.Statistics;

namespace ScottPlot
{
#pragma warning disable CS0618 // Type or member is obsolete
    public class PlottableVectorField : Plottable
    {
        public Vector2[,] vectors;
        public double[] xs;
        public double[] ys;
        public string label;
        public Color color;
        private Colormap colormap;

        private Pen pen;
        private Color[] arrowColors;
        private double scaleFactor;

        public PlottableVectorField(Vector2[,] vectors, double[] xs, double[] ys, string label, Color color, Colormap colormap, double scaleFactor)
        {
            //the magnitude squared is faster to compute than the magnitude
            double minMagnitudeSquared = vectors[0, 0].Length();
            double maxMagnitudeSquared = vectors[0, 0].Length();
            for (int i = 0; i < xs.Length; i++)
            {
                for (int j = 0; j < ys.Length; j++)
                {
                    if (vectors[i, j].LengthSquared() > maxMagnitudeSquared)
                    {
                        maxMagnitudeSquared = vectors[i, j].LengthSquared();
                    }
                    else if (vectors[i, j].LengthSquared() < minMagnitudeSquared)
                    {
                        minMagnitudeSquared = vectors[i, j].LengthSquared();
                    }
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
                    {
                        intensities[i, j] = (vectors[i, j].Length() - minMagnitude) / (maxMagnitude - minMagnitude);
                    }
                    vectors[i, j] = Vector2.Multiply(vectors[i, j], (float)(scaleFactor / (maxMagnitude * 1.2))); //This is not a true normalize
                }
            }

            if (colormap != null)
            {
                double[] flattenedIntensities = intensities.Cast<double>().ToArray();
                arrowColors = Colormap.GetColors(flattenedIntensities, colormap);
            }

            this.vectors = vectors;
            this.xs = xs;
            this.ys = ys;
            this.label = label;
            this.color = color;
            this.colormap = colormap;
            this.scaleFactor = scaleFactor;

            pen = new Pen(color);
            pen.CustomEndCap = new AdjustableArrowCap(2, 2);
        }

        public override LegendItem[] GetLegendItems()
        {
            return new LegendItem[] { new LegendItem(label, color, lineWidth: 10, markerShape: MarkerShape.none) };
        }

        public override AxisLimits2D GetLimits()
        {
            return new AxisLimits2D(xs.Min() - 1, xs.Max() + 1, ys.Min() - 1, ys.Max() + 1);
        }

        public override int GetPointCount()
        {
            return vectors.Length;
        }

        public override void Render(Settings settings)
        {
            for (int i = 0; i < xs.Length; i++)
            {
                for (int j = 0; j < ys.Length; j++)
                {
                    if (this.colormap != null)
                    {
                        pen.Color = arrowColors[i * ys.Length + j];
                    }
                    PlotVector(vectors[i, j], xs[i], ys[j], settings);
                }
            }
        }

        private void PlotVector(Vector2 v, double tailX, double tailY, Settings settings)
        {
            PointF tail = settings.GetPixel(tailX - v.X / 2, tailY - v.Y / 2);
            PointF end = settings.GetPixel(tailX + v.X / 2, v.Y + tailY - v.Y / 2);

            settings.gfxData.DrawLine(pen, tail, end);
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableVectorField{label} with {GetPointCount()} vectors";
        }
    }
}
