namespace ScottPlot.Plottable
{
    public interface IUsesAxes
    {
        int HorizontalAxisIndex { get; set; }
        int VerticalAxisIndex { get; set; }
        (double xMin, double xMax, double yMin, double yMax) GetAxisLimits();
    }
}
