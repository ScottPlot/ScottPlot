namespace ScottPlot.Plottable
{
    public interface IHasPoints
    {
        (double x, double y, int index) GetPointNearestX(double x);
        (double x, double y, int index) GetPointNearestY(double y);
        (double x, double y, int index) GetPointNearest(double x, double y);
    }
}
