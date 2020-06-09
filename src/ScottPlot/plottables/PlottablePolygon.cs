using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ScottPlot.Config;

namespace ScottPlot
{
    public class PlottablePolygon : Plottable
    {
        public readonly double[] xs;
        public readonly double[] ys;
        public string label;

        public double lineWidth;
        System.Drawing.Color lineColor;
        public System.Drawing.Pen pen;
        public bool fill;
        System.Drawing.Color fillColor;
        public System.Drawing.Brush brush;

        public PlottablePolygon(double[] xs, double[] ys, string label,
            double lineWidth, System.Drawing.Color lineColor,
            bool fill, System.Drawing.Color fillColor, double fillAlpha)
        {
            if (xs is null || ys is null)
                throw new ArgumentException("xs and ys cannot be null");

            if (xs.Length != ys.Length)
                throw new ArgumentException("xs and ys must have the same number of elements");

            if (xs.Length < 3)
                throw new ArgumentException("polygons must contain at least 3 points");

            this.xs = xs;
            this.ys = ys;
            this.label = label;
            this.lineWidth = lineWidth;
            this.lineColor = lineColor;
            this.fill = fill;

            pen = new System.Drawing.Pen(lineColor, (float)lineWidth)
            {
                LineJoin = System.Drawing.Drawing2D.LineJoin.Round, // prevent sharp corners
            };
            this.fillColor = System.Drawing.Color.FromArgb((int)(255 * fillAlpha), fillColor.R, fillColor.G, fillColor.B);
            brush = new System.Drawing.SolidBrush(this.fillColor);
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottablePolygon{label} with {GetPointCount()} points";
        }

        public override int GetPointCount()
        {
            return xs.Length;
        }

        public override AxisLimits2D GetLimits()
        {
            double xMin = xs[0];
            double xMax = xs[0];
            double yMin = ys[0];
            double yMax = ys[0];

            for (int i = 1; i < xs.Length; i++)
            {
                xMin = Math.Min(xMin, xs[i]);
                xMax = Math.Max(xMax, xs[i]);
                yMin = Math.Min(yMin, ys[i]);
                yMax = Math.Max(yMax, ys[i]);
            }

            return new AxisLimits2D(xMin, xMax, yMin, yMax);
        }

        public override LegendItem[] GetLegendItems()
        {
            if (fill)
                return new LegendItem[] { new LegendItem(label, fillColor, lineWidth: 10, markerShape: MarkerShape.none) };
            else
                return new LegendItem[] { new LegendItem(label, lineColor, lineWidth: lineWidth, markerShape: MarkerShape.none) };
        }

        public override void Render(Settings settings)
        {
            var pointList = new List<PointF>(xs.Length);
            for (int i = 0; i < xs.Length; i++)
            {
                if (double.IsNaN(xs[i]) || double.IsNaN(ys[i]))
                    continue;

                var thisPoint = new PointF(
                    x: (float)settings.GetPixelX(xs[i]),
                    y: (float)settings.GetPixelY(ys[i]));

                pointList.Add(thisPoint);
            }
            PointF[] points = pointList.ToArray();

            if (fill)
                settings.gfxData.FillPolygon(brush, points);
            if (lineWidth > 0)
                settings.gfxData.DrawPolygon(pen, points);
        }
    }
}
