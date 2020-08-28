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
        public bool Visible { get; set; }
        public int PointCount { get; }
        public AxisLimits Limits { get; }

        public PlotLayer Layer => PlotLayer.Data;

        public double[] Xs { get { return xs; } }
        public double[] Ys { get { return ys; } }

        private double[] xs;
        private double[] ys;

        public void Render(Bitmap bmp, PlotInfo info)
        {
            Debug.WriteLine("Plotting scatter");
        }

        public void ReplaceXs(double[] xs)
        {
            if (xs.Length != ys.Length)
                throw new ArgumentException("new Xs must have the same length as existing Ys");

            this.xs = xs;
        }

        public void ReplaceYs(double[] ys)
        {
            if (xs.Length != ys.Length)
                throw new ArgumentException("new Ys must have the same length as existing Xs");

            this.ys = ys;
        }

        public void ReplaceXsAndYs(double[] xs, double[] ys)
        {
            if (xs is null || ys is null)
                throw new ArgumentException("Xs and Ys must not be null");

            if (xs.Length != ys.Length)
                throw new ArgumentException("xs and ys must have the same length");

            this.xs = xs;
            this.ys = ys;
        }
    }
}
