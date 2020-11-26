using ScottPlot.Drawing;
using System;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Plottable
{
    public class ErrorBars : IPlottable
    {
        public readonly double[] Xs;
        public readonly double[] Ys;
        public readonly double[] XErrorPositive;
        public readonly double[] XErrorNegative;
        public readonly double[] YErrorPositive;
        public readonly double[] YErrorNegative;
        public int HorizontalAxisIndex { get; set; } = 0;
        public int VerticalAxisIndex { get; set; } = 0;
        public string label;

        public float CapSize = 3;
        public float LineWidth = 1;
        public Color Color = Color.Black;
        public LineStyle LineStyle = LineStyle.Solid;
        public bool IsVisible { get; set; } = true;

        public ErrorBars(double[] xs, double[] ys,
                                  double[] xPositiveError, double[] xNegativeError,
                                  double[] yPositiveError, double[] yNegativeError)
        {
            Xs = xs;
            Ys = ys;
            XErrorPositive = xPositiveError;
            XErrorNegative = xNegativeError;
            YErrorPositive = yPositiveError;
            YErrorNegative = yNegativeError;
        }

        public int PointCount { get => Ys.Length; }

        public override string ToString()
        {
            string label = string.IsNullOrWhiteSpace(this.label) ? "" : $" ({this.label})";
            return $"PlottableErrorBars{label} with {PointCount} points";
        }

        public AxisLimits GetAxisLimits()
        {
            double xMin = double.PositiveInfinity;
            double yMin = double.PositiveInfinity;
            double xMax = double.NegativeInfinity;
            double yMax = double.NegativeInfinity;

            if (XErrorNegative is null)
            {
                xMin = Xs.Min();
            }
            else
            {
                for (int i = 0; i < Xs.Length; i++)
                    xMin = Math.Min(xMin, Xs[i] - XErrorNegative[i]);
            }

            if (XErrorPositive is null)
            {
                xMax = Xs.Max();
            }
            else
            {
                for (int i = 0; i < Xs.Length; i++)
                    xMax = Math.Max(xMax, Xs[i] + XErrorPositive[i]);
            }

            if (YErrorNegative is null)
            {
                yMin = Ys.Min();
            }
            else
            {
                for (int i = 0; i < Xs.Length; i++)
                    yMin = Math.Min(yMin, Ys[i] - YErrorNegative[i]);
            }

            if (YErrorPositive is null)
            {
                yMax = Ys.Max();
            }
            else
            {
                for (int i = 0; i < Xs.Length; i++)
                    yMax = Math.Max(yMax, Ys[i] + YErrorPositive[i]);
            }

            return new AxisLimits(xMin, xMax, yMin, yMax);
        }

        public LegendItem[] GetLegendItems()
        {
            var singleLegendItem = new LegendItem()
            {
                label = label,
                color = Color,
                markerShape = MarkerShape.none
            };
            return new LegendItem[] { singleLegendItem };
        }

        public void ValidateData(bool deepValidation = false)
        {
            Validate.AssertHasElements("xs", Xs);
            Validate.AssertHasElements("ys", Ys);
            Validate.AssertEqualLength("xs and ys", Xs, Ys);

            if (XErrorNegative != null) Validate.AssertEqualLength("xs and xNegativeError", Xs, XErrorNegative);
            if (YErrorNegative != null) Validate.AssertEqualLength("xs and yNegativeError", Xs, YErrorNegative);
            if (XErrorPositive != null) Validate.AssertEqualLength("xs and xPositiveError", Xs, XErrorPositive);
            if (YErrorPositive != null) Validate.AssertEqualLength("xs and yPositiveError", Xs, YErrorPositive);

            if (deepValidation)
            {
                Validate.AssertAllReal("xs", Xs);
                Validate.AssertAllReal("ys", Ys);
                if (XErrorNegative != null) Validate.AssertAllReal("xNegativeError", XErrorNegative);
                if (YErrorNegative != null) Validate.AssertAllReal("yNegativeError", YErrorNegative);
                if (XErrorPositive != null) Validate.AssertAllReal("xPositiveError", XErrorPositive);
                if (YErrorPositive != null) Validate.AssertAllReal("yPositiveError", YErrorPositive);
            }
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using (var gfx = GDI.Graphics(bmp, dims, lowQuality))
            using (var pen = GDI.Pen(Color, LineWidth, LineStyle, true))
            {
                if (XErrorPositive != null) DrawErrorBar(dims, gfx, pen, XErrorPositive, true, true);
                if (XErrorNegative != null) DrawErrorBar(dims, gfx, pen, XErrorNegative, true, false);
                if (YErrorPositive != null) DrawErrorBar(dims, gfx, pen, YErrorPositive, false, true);
                if (YErrorNegative != null) DrawErrorBar(dims, gfx, pen, YErrorNegative, false, false);
            }
        }

        public void DrawErrorBar(PlotDimensions dims, Graphics gfx, Pen penLine, double[] errorArray, bool xError, bool positiveError)
        {
            if (errorArray is null)
                return;

            for (int i = 0; i < Xs.Length; i++)
            {
                float centerPixelX = dims.GetPixelX(Xs[i]);
                float centerPixelY = dims.GetPixelY(Ys[i]);
                float errorSize = positiveError ? (float)errorArray[i] : -(float)errorArray[i];
                if (xError)
                {
                    float xWithError = dims.GetPixelX(Xs[i] + errorSize);
                    gfx.DrawLine(penLine, centerPixelX, centerPixelY, xWithError, centerPixelY);
                    gfx.DrawLine(penLine, xWithError, centerPixelY - CapSize, xWithError, centerPixelY + CapSize);
                }
                else
                {
                    float yWithError = dims.GetPixelY(Ys[i] + errorSize);
                    gfx.DrawLine(penLine, centerPixelX, centerPixelY, centerPixelX, yWithError);
                    gfx.DrawLine(penLine, centerPixelX - CapSize, yWithError, centerPixelX + CapSize, yWithError);
                }
            }
        }
    }
}
