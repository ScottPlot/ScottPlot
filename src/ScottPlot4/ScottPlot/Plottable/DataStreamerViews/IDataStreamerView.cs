using System.Drawing;

namespace ScottPlot.Plottable.DataStreamerViews;

/// <summary>
/// Contains logic for rendering fixed-length data in a streaming data logger.
/// </summary>
public interface IDataStreamerView
{
    DataStreamer Streamer { get; }
    void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false);
}
