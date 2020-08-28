using ScottPlot.Renderable;
using ScottPlot.Space;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Text;

namespace ScottPlot.Plottable
{
    public class Scatter : IPlottable
    {
        public bool Visible { get; set; } = true;
        public bool AntiAlias { get; set; } = true;
        public PlotLayer Layer => PlotLayer.Data;

        public double[] Xs { get; private set; }
        public double[] Ys { get; private set; }

        public int PointCount { get { return Xs is null ? 0 : Xs.Length; } }
        public AxisLimits Limits { get; private set; } = new AxisLimits();

        public void Render(Bitmap bmp, PlotInfo info)
        {
            if (Visible == false)
                return;

            using (Graphics gfx = Graphics.FromImage(bmp))
            using (Pen pen = new Pen(Color.Magenta, 5))
            {
                gfx.SetClip(new RectangleF(info.DataOffsetX, info.DataOffsetY, info.DataWidth, info.DataHeight));

                if (AntiAlias)
                {
                    gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    gfx.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                }

                PointF[] points = new PointF[Xs.Length];
                for (int i = 0; i < Xs.Length; i++)
                    points[i] = new PointF(info.GetPixelX(Xs[i]), info.GetPixelY(Ys[i]));

                gfx.DrawLines(pen, points);
            }
        }

        public void ReplaceXs(double[] xs)
        {
            if (xs.Length != Ys.Length)
                throw new ArgumentException("new Xs must have the same length as existing Ys");

            Xs = xs;
            UpdateLimits(x: true, y: false);
        }

        public void ReplaceYs(double[] ys)
        {
            if (Xs.Length != ys.Length)
                throw new ArgumentException("new Ys must have the same length as existing Xs");

            Ys = ys;
            UpdateLimits(x: false, y: true);
        }

        public void ReplaceXsAndYs(double[] xs, double[] ys)
        {
            if (xs is null || ys is null)
                throw new ArgumentException("Xs and Ys must not be null");

            if (xs.Length != ys.Length)
                throw new ArgumentException("xs and ys must have the same length");

            Xs = xs;
            Ys = ys;
            UpdateLimits(x: true, y: true);
        }

        public void UpdateLimits(bool x, bool y)
        {
            if (PointCount == 0)
            {
                Limits.X1 = double.NaN;
                Limits.X2 = double.NaN;
                Limits.Y1 = double.NaN;
                Limits.Y2 = double.NaN;
                return;
            }

            if (x)
            {
                double xMin = Xs[0];
                double xMax = Xs[0];
                for (int i = 1; i < Xs.Length; i++)
                {
                    xMin = Math.Min(xMin, Xs[i]);
                    xMax = Math.Max(xMax, Xs[i]);
                }
                Limits.X1 = xMin;
                Limits.X2 = xMax;
            }

            if (y)
            {
                double yMin = Ys[0];
                double yMax = Ys[0];
                for (int i = 1; i < Ys.Length; i++)
                {
                    yMin = Math.Min(yMin, Ys[i]);
                    yMax = Math.Max(yMax, Ys[i]);
                }
                Limits.Y1 = yMin;
                Limits.Y2 = yMax;
            }
        }
    }
}
