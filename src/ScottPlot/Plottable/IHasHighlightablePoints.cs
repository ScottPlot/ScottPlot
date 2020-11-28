namespace ScottPlot.Plottable
{
    interface IHasHighlightablePoints
    {
        (double x, double y, int index) HighlightPoint(int index);
        (double x, double y, int index) HighlightPointNearestX(double x);
        (double x, double y, int index) HighlightPointNearestY(double y);
        (double x, double y, int index) HighlightPointNearest(double x, double y);
        void HighlightClear();
    }
}
