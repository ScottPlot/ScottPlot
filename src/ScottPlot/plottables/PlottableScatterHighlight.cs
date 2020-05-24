using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using ScottPlot.Config;
using ScottPlot.Drawing;
using System.Diagnostics;
using ScottPlot.plottables;

namespace ScottPlot
{
    public class PlottableScatterHighlight : PlottableScatter, IExportable, IHighlightable
    {
        public MarkerShape highlightedShape;
        public float highlightedMarkerSize;
        public Color highlightedColor;
        private Pen penLineError;
        private bool[] isHighlighted;

        public PlottableScatterHighlight(double[] xs, double[] ys, Color color, double lineWidth, double markerSize, string label,
            double[] errorX, double[] errorY, double errorLineWidth, double errorCapSize, bool stepDisplay, MarkerShape markerShape, LineStyle lineStyle, MarkerShape highlightedShape, Color highlightedColor, double highlightedMarkerSize)
            : base(xs, ys, color, lineWidth, markerSize, label, errorX, errorY, errorLineWidth, errorCapSize, stepDisplay, markerShape, lineStyle)
        {
            this.highlightedColor = highlightedColor;
            this.highlightedMarkerSize = (float)highlightedMarkerSize;
            this.highlightedShape = highlightedShape;
            this.isHighlighted = new bool[xs.Length];
        }

        protected override void DrawPoint(Settings settings, List<PointF> points, int i)
        {
            if (isHighlighted[i])
            {
                MarkerTools.DrawMarker(settings.gfxData, points[i], highlightedShape, highlightedMarkerSize, highlightedColor);
            }
            else
            {
                base.DrawPoint(settings, points, i);
            }
        }

        public void HighlightClear()
        {
            isHighlighted = new bool[xs.Length];
        }

        public void HighlightPoint(int index)
        {
            isHighlighted[index] = true;
        }

        private int GetIndexNearestX(double x)
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
            return minIndex;
        }

        private int GetIndexNearestY(double y)
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
            return minIndex;
        }

        private int GetIndexNearestPoint(double x, double y)
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
            return minIndex;
        }

        public void HighlightPointNearestX(double x)
        {
            HighlightPoint(GetIndexNearestX(x));
        }

        public void HighlightPointNearestY(double y)
        {
            HighlightPoint(GetIndexNearestY(y));
        }

        public void HighlightPointNearest(double x, double y)
        {
            HighlightPoint(GetIndexNearestPoint(x, y));
        }

        public (double x, double y) GetPointNearestX(double x)
        {
            int index = GetIndexNearestX(x);
            return (xs[index], ys[index]);
        }

        public (double x, double y) GetPointNearestY(double y)
        {
            int index = GetIndexNearestY(y);
            return (xs[index], ys[index]);
        }

        public (double x, double y) GetPointNearest(double x, double y)
        {
            int index = GetIndexNearestPoint(x, y);
            return (xs[index], ys[index]);
        }
    }
}
