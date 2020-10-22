using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using ScottPlot.Drawing;
using ScottPlot.plottables;

namespace ScottPlot
{
    public class PlottableScatterHighlight : PlottableScatter, IExportable, IHighlightable, IPlottable
    {
        public MarkerShape highlightedShape = MarkerShape.openCircle;
        public float highlightedMarkerSize = 10;
        public Color highlightedColor = Color.Red;
        protected bool[] isHighlighted;

        public PlottableScatterHighlight(double[] xs, double[] ys, double[] xErr = null, double[] yErr = null) :
                                    base(xs, ys, xErr, yErr) => HighlightClear();

        public new void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            base.Render(dims, bmp, lowQuality);

            if (isHighlighted is null || isHighlighted.Length == 0)
                return;

            using (var gfx = Graphics.FromImage(bmp))
            {
                gfx.SmoothingMode = lowQuality ? SmoothingMode.HighSpeed : SmoothingMode.AntiAlias;
                gfx.TextRenderingHint = lowQuality ? TextRenderingHint.SingleBitPerPixelGridFit : TextRenderingHint.AntiAliasGridFit;

                var highlightedIndexes = Enumerable.Range(0, isHighlighted.Length).Where(x => isHighlighted[x]);
                foreach (int i in highlightedIndexes)
                {
                    PointF pt = new PointF(dims.GetPixelX(xs[i]), dims.GetPixelY(ys[i]));
                    MarkerTools.DrawMarker(gfx, pt, highlightedShape, highlightedMarkerSize, highlightedColor);
                }
            }
        }

        public void HighlightClear()
        {
            isHighlighted = new bool[xs.Length];
        }

        public (double x, double y, int index) HighlightPoint(int index)
        {
            // if the size of xs changed, reset isHighlighted to match its new size
            if (isHighlighted.Length != xs.Length)
                HighlightClear();

            if (index < 0 || index >= isHighlighted.Length)
                throw new ArgumentException("Invalid index");
            isHighlighted[index] = true;
            return (xs[index], ys[index], index);
        }

        public (double x, double y, int index) GetPointNearestX(double x)
        {
            double minDistance = Math.Abs(xs[0] - x);
            int minIndex = 0;
            for (int i = 1; i < xs.Length; i++)
            {
                double currDistance = Math.Abs(xs[i] - x);
                if (currDistance < minDistance)
                {
                    minIndex = i;
                    minDistance = currDistance;
                }
            }

            return (xs[minIndex], ys[minIndex], minIndex);
        }

        public (double x, double y, int index) GetPointNearestY(double y)
        {
            double minDistance = Math.Abs(ys[0] - y);
            int minIndex = 0;
            for (int i = 1; i < ys.Length; i++)
            {
                double currDistance = Math.Abs(ys[i] - y);
                if (currDistance < minDistance)
                {
                    minIndex = i;
                    minDistance = currDistance;
                }
            }

            return (xs[minIndex], ys[minIndex], minIndex);
        }

        public (double x, double y, int index) GetPointNearest(double x, double y)
        {
            List<(double x, double y)> points = xs.Zip(ys, (first, second) => (first, second)).ToList();

            double pointDistanceSquared(double x1, double y1) => (x1 - x) * (x1 - x) + (y1 - y) * (y1 - y);

            double minDistance = pointDistanceSquared(points[0].x, points[0].y);
            int minIndex = 0;
            for (int i = 1; i < points.Count; i++)
            {
                double currDistance = pointDistanceSquared(points[i].x, points[i].y);
                if (currDistance < minDistance)
                {
                    minIndex = i;
                    minDistance = currDistance;
                }
            }

            return (xs[minIndex], ys[minIndex], minIndex);
        }

        public (double x, double y, int index) HighlightPointNearestX(double x)
        {
            var point = GetPointNearestX(x);

            // if the size of xs changed, reset isHighlighted to match its new size
            if (isHighlighted.Length != xs.Length)
                HighlightClear();

            isHighlighted[point.index] = true;
            return point;
        }

        public (double x, double y, int index) HighlightPointNearestY(double y)
        {
            var point = GetPointNearestY(y);

            // if the size of xs changed, reset isHighlighted to match its new size
            if (isHighlighted.Length != xs.Length)
                HighlightClear();

            isHighlighted[point.index] = true;
            return point;
        }

        public (double x, double y, int index) HighlightPointNearest(double x, double y)
        {
            var point = GetPointNearest(x, y);

            // if the size of xs changed, reset isHighlighted to match its new size
            if (isHighlighted.Length != xs.Length)
                HighlightClear();

            isHighlighted[point.index] = true;
            return point;
        }
    }
}
