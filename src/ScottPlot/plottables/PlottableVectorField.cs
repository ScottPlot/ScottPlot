using ScottPlot.Config;
using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using System.Drawing;
using System.Linq;
using System.Drawing.Drawing2D;

namespace ScottPlot
{
    public class PlottableVectorField : Plottable
    {
        public Vector2[,] vectors;
        public double[] xs;
        public double[] ys;
        public string label;
        public Color color;

        private Pen pen;

        public PlottableVectorField(Vector2[,] vectors, double[] xs, double[] ys, string label, Color color)
        {
            //the magnitude squared is faster to compute than the magnitude
            float maxMagnitudeSquared = vectors[0, 0].Length();
            for (int i = 0; i < xs.Length; i++)
            {
                for (int j = 0; j < ys.Length; j++)
                {
                    if (vectors[i, j].LengthSquared() > maxMagnitudeSquared)
                    {
                        maxMagnitudeSquared = vectors[i, j].LengthSquared();
                    }
                }
            }
            for (int i = 0; i < xs.Length; i++)
            {
                for (int j = 0; j < ys.Length; j++)
                {
                    vectors[i, j] = Vector2.Multiply(vectors[i, j], (float)(1 / (Math.Sqrt(maxMagnitudeSquared) * 1.2))); //This is not a true normalize
                }
            }
            this.vectors = vectors;
            this.xs = xs;
            this.ys = ys;
            this.label = label;
            this.color = color;

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
                    PlotVector(vectors[i, j], xs[i], ys[j], settings);
                }
            }
        }

        private void PlotVector(Vector2 v, double tailX, double tailY, Settings settings)
        {
            PointF tail = settings.GetPixel(tailX, tailY - v.Y / 2);
            PointF end = settings.GetPixel(tailX + v.X, v.Y + tailY - v.Y / 2);

            settings.gfxData.DrawLine(pen, tail, end);
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableVectorField{label} with {GetPointCount()} vectors";
        }
    }
}
