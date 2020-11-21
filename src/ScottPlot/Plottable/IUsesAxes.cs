namespace ScottPlot.Plottable
{
    public interface IUsesAxes
    {
        int HorizontalAxisIndex { get; set; }
        int VerticalAxisIndex { get; set; }
        AxisLimits GetAxisLimits();
    }
}
