using System.Drawing;

namespace ScottPlot.Plottable
{
    public interface IHasMarker
    {
        float MarkerSize { get; set; }
        MarkerShape MarkerShape { get; set; }
        Color MarkerColor { get; set; }
    }
}
