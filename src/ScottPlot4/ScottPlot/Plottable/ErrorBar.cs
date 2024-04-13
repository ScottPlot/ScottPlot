using ScottPlot.Drawing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottable
{
    public class ErrorBar : IPlottable, IHasLine, IHasMarker, IHasColor
    {
        public double[] Xs { get; set; }
        public double[] Ys { get; set; }
        public double[] XErrorsPositive { get; set; }
        public double[] XErrorsNegative { get; set; }
        public double[] YErrorsPositive { get; set; }
        public double[] YErrorsNegative { get; set; }
        public int CapSize { get; set; } = 3;

        public bool IsVisible { get; set; } = true;
        public int XAxisIndex { get; set; } = 0;
        public int YAxisIndex { get; set; } = 0;
        public double LineWidth { get; set; } = 1;
        public Color Color { get; set; } = Color.Gray;
        public Color LineColor { get => Color; set { Color = value; } }
        public LineStyle LineStyle { get; set; } = LineStyle.Solid;
        public MarkerShape MarkerShape { get; set; } = MarkerShape.filledCircle;
        public float MarkerLineWidth { get; set; } = 1;
        public float MarkerSize { get; set; } = 0;
        public Color MarkerColor { get => Color; set { Color = value; } }
        public string Label { get; set; } = string.Empty;

        public ErrorBar(double[] xs, double[] ys, double[] xErrorsPositive, double[] xErrorsNegative, double[] yErrorsPositive, double[] yErrorsNegative)
        {
            Xs = xs;
            Ys = ys;
            XErrorsPositive = xErrorsPositive;
            XErrorsNegative = xErrorsNegative;
            YErrorsPositive = yErrorsPositive;
            YErrorsNegative = yErrorsNegative;
        }

        public LegendItem[] GetLegendItems()
        {
            LegendItem singleItem = new(this)
            {
                lineStyle = LineStyle,
                label = Label
            };

            return LegendItem.Single(singleItem);
        }

        public AxisLimits GetAxisLimits()
        {
            double xMin = double.PositiveInfinity;
            double xMax = double.NegativeInfinity;
            double yMin = double.PositiveInfinity;
            double yMax = double.NegativeInfinity;

            for (int i = 0; i < Xs.Length; i++)
            {
                xMin = Math.Min(xMin, Xs[i] - (XErrorsNegative?[i] ?? 0));
                xMax = Math.Max(xMax, Xs[i] + (XErrorsPositive?[i] ?? 0));
                yMin = Math.Min(yMin, Ys[i] - (YErrorsNegative?[i] ?? 0));
                yMax = Math.Max(yMax, Ys[i] + (YErrorsPositive?[i] ?? 0));
            }

            return new AxisLimits(xMin, xMax, yMin, yMax);
        }

        public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality, clipToDataArea: true);
            using Pen pen = GDI.Pen(Color, LineWidth, LineStyle, true);

            if (XErrorsPositive is not null && XErrorsNegative is not null)
            {
                DrawErrorBars(dims, gfx, pen, XErrorsPositive, XErrorsNegative, true);
            }

            if (YErrorsPositive is not null && YErrorsNegative is not null)
            {
                DrawErrorBars(dims, gfx, pen, YErrorsPositive, YErrorsNegative, false);
            }

            if (MarkerSize > 0 && MarkerShape != MarkerShape.none)
            {
                DrawMarkers(dims, gfx);
            }
        }

        private void DrawErrorBars(PlotDimensions dims, Graphics gfx, Pen pen, double[] errorPositive, double[] errorNegative, bool onXAxis)
        {
            for (int i = 0; i < Xs.Length; i++)
            {
                // Pixel centre = dims.GetPixel(new Coordinate(Xs[i], Ys[i]));

                if (onXAxis)
                {
                    Pixel left = dims.GetPixel(new Coordinate(Xs[i] - errorNegative[i], Ys[i]));
                    Pixel right = dims.GetPixel(new Coordinate(Xs[i] + errorPositive[i], Ys[i]));
                    if (left == right)
                        continue;

                    gfx.DrawLine(pen, left.X, left.Y, right.X, right.Y);
                    gfx.DrawLine(pen, left.X, left.Y - CapSize, left.X, left.Y + CapSize);
                    gfx.DrawLine(pen, right.X, right.Y - CapSize, right.X, right.Y + CapSize);
                }
                else
                {
                    Pixel top = dims.GetPixel(new Coordinate(Xs[i], Ys[i] - errorNegative[i]));
                    Pixel bottom = dims.GetPixel(new Coordinate(Xs[i], Ys[i] + errorPositive[i]));
                    if (top == bottom)
                        continue;

                    gfx.DrawLine(pen, top.X, top.Y, bottom.X, bottom.Y);
                    gfx.DrawLine(pen, top.X - CapSize, top.Y, top.X + CapSize, top.Y);
                    gfx.DrawLine(pen, bottom.X - CapSize, bottom.Y, bottom.X + CapSize, bottom.Y);
                }
            }
        }

        private void DrawMarkers(PlotDimensions dims, Graphics gfx)
        {
            PointF[] pixels = new PointF[Xs.Length];
            for (int i = 0; i < Xs.Length; i++)
            {
                float xPixel = dims.GetPixelX(Xs[i]);
                float yPixel = dims.GetPixelY(Ys[i]);
                pixels[i] = new(xPixel, yPixel);
            }
            MarkerTools.DrawMarkers(gfx, pixels, MarkerShape, MarkerSize, Color, MarkerLineWidth);
        }

        public void ValidateData(bool deep = false)
        {
            Validate.AssertHasElements(nameof(Xs), Xs);
            Validate.AssertHasElements(nameof(Ys), Ys);
            Validate.AssertEqualLength($"{nameof(Xs)}, {nameof(Ys)}", Xs, Ys);

            if (XErrorsPositive is not null || XErrorsNegative is not null)
            {
                Validate.AssertHasElements(nameof(XErrorsPositive), XErrorsPositive);
                Validate.AssertHasElements(nameof(XErrorsNegative), XErrorsNegative);
                Validate.AssertEqualLength($"{Xs} {nameof(XErrorsPositive)}, {nameof(XErrorsNegative)}", Xs, XErrorsPositive, XErrorsNegative);
            }

            if (YErrorsPositive is not null || YErrorsNegative is not null)
            {
                Validate.AssertHasElements(nameof(YErrorsPositive), YErrorsPositive);
                Validate.AssertHasElements(nameof(YErrorsNegative), YErrorsNegative);
                Validate.AssertEqualLength($"{Xs} {nameof(YErrorsPositive)}, {nameof(YErrorsNegative)}", Xs, YErrorsPositive, YErrorsNegative);
            }

            if (deep)
            {
                Validate.AssertAllReal(nameof(Xs), Xs);
                Validate.AssertAllReal(nameof(Ys), Ys);

                if (XErrorsPositive is not null && XErrorsNegative is not null)
                {
                    Validate.AssertAllReal(nameof(XErrorsPositive), XErrorsPositive);
                    Validate.AssertAllReal(nameof(XErrorsNegative), XErrorsNegative);
                }

                if (YErrorsPositive is not null && YErrorsNegative is not null)
                {
                    Validate.AssertAllReal(nameof(YErrorsPositive), YErrorsPositive);
                    Validate.AssertAllReal(nameof(YErrorsNegative), YErrorsNegative);
                }

                // TODO: Should we validate that errors are all positive?
            }
        }
    }
}
