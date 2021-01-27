using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ScottPlot.Drawing;

namespace ScottPlot.Plottable
{
    [Obsolete("SCATTER PLOTS HAVE MOST OF THESE FUNCTIONS - THIS CLASS WILL BE DELETED SHORTLY")]
    public class ScatterPlotHighlight : ScatterPlot, IPlottable, IExportable, IHasPoints, IHasHighlightablePoints
    {
        public MarkerShape highlightedShape = MarkerShape.openCircle;
        public float highlightedMarkerSize = 10;
        public Color highlightedColor = Color.Red;
        protected HashSet<int> isHighlighted = new HashSet<int>();

        public ScatterPlotHighlight(double[] xs, double[] ys, double[] xErr = null, double[] yErr = null) :
                                    base(xs, ys, xErr, yErr) => HighlightClear();

        public new void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
        {
            base.Render(dims, bmp, lowQuality);

            if (isHighlighted is null || isHighlighted.Count == 0)
                return;

            using (var gfx = GDI.Graphics(bmp, dims, lowQuality))
            {
                foreach (int i in isHighlighted)
                {
                    PointF pt = new PointF(dims.GetPixelX(Xs[i]), dims.GetPixelY(Ys[i]));
                    MarkerTools.DrawMarker(gfx, pt, highlightedShape, highlightedMarkerSize, highlightedColor);
                }
            }
        }

        public void HighlightClear()
        {
            isHighlighted.Clear();
        }

        public (double x, double y, int index) HighlightPoint(int index)
        {
            if (index < 0 || index >= Xs.Length)
                throw new ArgumentException("Invalid index");
            isHighlighted.Add(index);
            return (Xs[index], Ys[index], index);
        }

        public (double x, double y, int index) HighlightPointNearestX(double x)
        {
            var point = GetPointNearestX(x);

            isHighlighted.Add(point.index);
            return point;
        }

        public (double x, double y, int index) HighlightPointNearestY(double y)
        {
            var point = GetPointNearestY(y);

            isHighlighted.Add(point.index);
            return point;
        }

        public (double x, double y, int index) HighlightPointNearest(double x, double y)
        {
            var point = GetPointNearest(x, y);

            isHighlighted.Add(point.index);
            return point;
        }
    }
}
