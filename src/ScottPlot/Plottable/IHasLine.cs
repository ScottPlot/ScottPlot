using System.Drawing;

namespace ScottPlot.Plottable
{
    public interface IHasLine
    {
        LineStyle LineStyle { get; set; }
        double LineWidth { get; set; }
        Color LineColor { get; set; }
    }
}
