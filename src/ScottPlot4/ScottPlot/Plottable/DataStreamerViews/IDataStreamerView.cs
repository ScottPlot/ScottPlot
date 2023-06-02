using System.Drawing;

namespace ScottPlot.Plottable.DataStreamerViews;

public interface IDataStreamerView
{
    void Render(DataStreamer streamer, PlotDimensions dims, Graphics gfx, Pen pen);
}
