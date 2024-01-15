using ScottPlot.Plottables;

namespace ScottPlot.DataViews;

/// <summary>
/// Contains logic for rendering fixed-length data in a streaming data logger.
/// </summary>
public interface IDataStreamerView
{
    DataStreamer Streamer { get; }
    void Render(RenderPack rp);
}
