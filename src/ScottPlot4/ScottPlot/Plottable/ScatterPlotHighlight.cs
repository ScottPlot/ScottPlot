using System;
using System.Drawing;

namespace ScottPlot.Plottable
{
    [Obsolete("This plot type is deprecated: Use a regular scatter plot and call GetPointNearest(). See examples in documentation.", true)]
    public class ScatterPlotHighlight : ScatterPlot, IPlottable, IHasPoints, IHasHighlightablePoints
    {
        public MarkerShape highlightedShape { get; set; } = MarkerShape.openCircle;
        public float highlightedMarkerSize { get; set; } = 10;
        public Color highlightedColor { get; set; } = Color.Red;
        protected bool[] isHighlighted { get; set; }

        public ScatterPlotHighlight(double[] xs, double[] ys, double[] xErr = null, double[] yErr = null) :
                                    base(xs, ys, xErr, yErr) => HighlightClear();

        public new void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false) => throw new NotImplementedException();
        public void HighlightClear() => throw new NotImplementedException();
        public (double x, double y, int index) HighlightPoint(int index) => throw new NotImplementedException();
        public (double x, double y, int index) HighlightPointNearestX(double x) => throw new NotImplementedException();
        public (double x, double y, int index) HighlightPointNearestY(double y) => throw new NotImplementedException();
        public (double x, double y, int index) HighlightPointNearest(double x, double y) => throw new NotImplementedException();
    }
}
