using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScottPlot.Config;
using ScottPlot.Drawing;
using ScottPlot.plottables;

namespace ScottPlot
{
    public class PlottableScatterHighlight : PlottableScatter, IExportable, IHighlightable
    {
        public MarkerShape highlightedShape;
        public float highlightedMarkerSize;
        public Color highlightedColor;
        protected bool[] isHighlighted;

        public PlottableScatterHighlight(double[] xs, double[] ys, Color color, double lineWidth, double markerSize, string label,
            double[] errorX, double[] errorY, double errorLineWidth, double errorCapSize, bool stepDisplay, MarkerShape markerShape, LineStyle lineStyle, MarkerShape highlightedShape, Color highlightedColor, double highlightedMarkerSize)
            : base(xs, ys, color, lineWidth, markerSize, label, errorX, errorY, errorLineWidth, errorCapSize, stepDisplay, markerShape, lineStyle)
        {
            if (xs.Length != ys.Length)
                throw new ArgumentException("xs and ys must have same length");
            this.highlightedColor = highlightedColor;
            this.highlightedMarkerSize = (float)highlightedMarkerSize;
            this.highlightedShape = highlightedShape;
            HighlightClear();
        }

        protected override void DrawPoint(Settings settings, List<PointF> points, int i)
        {
            // always draw the underlying point
            base.DrawPoint(settings, points, i);

            // if highlighted, draw the highlight marker on top of it
            if (isHighlighted[i])
                MarkerTools.DrawMarker(settings.gfxData, points[i], highlightedShape, highlightedMarkerSize, highlightedColor);
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
