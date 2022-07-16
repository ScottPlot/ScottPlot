using System.Drawing;

namespace ScottPlot.Plottable
{
    public interface IHasMarker
    {
        float MarkerSize { get; set; }
        float MarkerLineWidth { get; set; }
        MarkerShape MarkerShape { get; set; }
        System.Drawing.Color MarkerColor { get; set; }
    }
}
