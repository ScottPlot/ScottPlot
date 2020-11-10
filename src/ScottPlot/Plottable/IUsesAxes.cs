using ScottPlot.Drawing;

namespace ScottPlot.Plottable
{
    public interface IUsesAxes
    {
        int HorizontalAxisIndex { get; set; }
        int VerticalAxisIndex { get; set; }
        //(float xMin, float xMax, float yMin, float yMax) GetLimits();
        AxisLimits2D GetAxisLimits();
    }
}
