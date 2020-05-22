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
        private List<int> highlightedIndexes;
        public MarkerShape highlightedShape;
        public float highlightedMarkerSize;
        public Color highlightedColor;
        private Pen penLineError;

        public PlottableScatterHighlight(double[] xs, double[] ys, Color color, double lineWidth, double markerSize, string label,
            double[] errorX, double[] errorY, double errorLineWidth, double errorCapSize, bool stepDisplay, MarkerShape markerShape, LineStyle lineStyle, MarkerShape highlightedShape, Color highlightedColor, double highlightedMarkerSize)
            : base(xs, ys, color, lineWidth, markerSize, label, errorX,  errorY, errorLineWidth, errorCapSize, stepDisplay, markerShape, lineStyle)
        {
            this.highlightedColor = highlightedColor;
            this.highlightedMarkerSize = (float)highlightedMarkerSize;
            this.highlightedShape = highlightedShape;
            this.highlightedIndexes = new List<int>();
        }

		protected override void DrawPoint(Settings settings, List<PointF> points, int i)
		{
            if (highlightedIndexes.BinarySearch(i) >= 0) //Always returns negative number for item not found: https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.list-1.binarysearch?view=netcore-3.1#System_Collections_Generic_List_1_BinarySearch__0_
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
            highlightedIndexes.Clear();
        }

        public void HighlightPoint(int index)
        {
            highlightedIndexes.Add(index);
            highlightedIndexes.Sort();
        }

        private int GetPointNearestIndex(double x) {
            double minDistance = Math.Abs(xs[0] - x);
            int minIndex = 0;
            for (int i = 0; i < xs.Length; i++)
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

        private int GetPointNearestIndex(double x, double y)
        {
            List<(double x, double y)> points = xs.Zip(ys, (first, second) => (first, second)).ToList();

            double pointDistanceSquared(double x1, double y1) => (x1 - x) * (x1 - x) + (y1 - y) * (y1 - y);

            double minDistance = double.PositiveInfinity;
            int minIndex = 0;
            for (int i = 0; i < points.Count; i++)
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

        public void HighlightPointNearest(double x)
        {
            HighlightPoint(GetPointNearestIndex(x));
        }

        public void HighlightPointNearest(double x, double y)
        {
            HighlightPoint(GetPointNearestIndex(x, y));
        }

        public (double x, double y) GetPointNearest(double x) { 
            int index = GetPointNearestIndex(x);
            return (xs[index], ys[index]);
        }

        public (double x, double y) GetPointNearest(double x, double y)
        {
            int index = GetPointNearestIndex(x, y);
            return (xs[index], ys[index]);
        }
    }
}
