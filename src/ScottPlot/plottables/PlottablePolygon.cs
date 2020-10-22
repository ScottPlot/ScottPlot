using ScottPlot.Config;
using ScottPlot.Diagnostic.Attributes;
using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ScottPlot
{
    public class PlottablePolygon : Plottable, IPlottable
    {
        public double[] xs;
        public double[] ys;
        public string label;

        public double lineWidth = 2;
        public Color lineColor = Color.Black;
        public bool fill = true;
        public Color fillColor = Color.Gray;
        public double fillAlpha = 0.5;

        public PlottablePolygon(double[] xs, double[] ys)
        {
            this.xs = xs;
            this.ys = ys;
        }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottablePolygon{label} with {GetPointCount()} points";
        }

        public override int GetPointCount() => xs.Length;

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

        public override LegendItem[] GetLegendItems() =>
            new LegendItem[] {
                new LegendItem(
                    label: label,
                    color: fill ? fillColor : lineColor,
                    lineWidth: fill ? 10 : lineWidth,
                    markerShape: MarkerShape.none
                )
            };

        public string ValidationErrorMessage { get; private set; }
        public bool IsValidData(bool deepValidation = false)
        {
            try
            {
                Validate.AssertHasElements("xs", xs);
                Validate.AssertHasElements("ys", ys);
                Validate.AssertEqualLength("xs and ys", xs, ys);

                if (xs.Length < 3)
                    throw new ArgumentException("polygons must contain at least 3 points");

                if (deepValidation)
                {
                    Validate.AssertAllReal("xs", xs);
                    Validate.AssertAllReal("ys", ys);
                }

                ValidationErrorMessage = "";
                return true;
            }
            catch (ArgumentException e)
            {
                ValidationErrorMessage = e.Message;
                return false;
            }
        }

        public override void Render(Settings settings) => throw new InvalidOperationException("use new Render()");

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            PointF[] points = new PointF[xs.Length];
            for (int i = 0; i < xs.Length; i++)
                points[i] = new PointF(dims.GetPixelX(xs[i]), dims.GetPixelY(ys[i]));

            using (Graphics gfx = GDI.Graphics(bmp, lowQuality))
            using (Brush fillBrush = GDI.Brush(Color.FromArgb((byte)(255 * fillAlpha), fillColor)))
            using (Pen outlinePen = GDI.Pen(lineColor, (float)lineWidth))
            {
                if (fill)
                    gfx.FillPolygon(fillBrush, points);

                if (lineWidth > 0)
                    gfx.DrawPolygon(outlinePen, points);
            }
        }
    }
}
