using System.Drawing;

namespace ScottPlot.Plottable.DataStreamerViews;

internal class Wipe : IDataStreamerView
{
    private readonly bool LeftToRight;

    public Wipe(bool leftToRight)
    {
        LeftToRight = leftToRight;
    }

    public void Render(DataStreamer streamer, PlotDimensions dims, Graphics gfx, Pen pen)
    {
        int newestCount = streamer.DataIndex;
        int oldestCount = streamer.Data.Length - newestCount;

        PointF[] newest = new PointF[newestCount];
        PointF[] oldest = new PointF[oldestCount];

        for (int i = 0; i < newest.Length; i++)
        {
            float x = dims.GetPixelX(i * streamer.SamplePeriod + streamer.OffsetX);
            float y = dims.GetPixelY(streamer.Data[i] + streamer.OffsetY);
            newest[i] = new(x, y);
        }

        for (int i = 0; i < oldest.Length; i++)
        {
            float x = dims.GetPixelX((i + streamer.DataIndex) * streamer.SamplePeriod + streamer.OffsetX);
            float y = dims.GetPixelY(streamer.Data[i + streamer.DataIndex] + streamer.OffsetY);
            oldest[i] = new(x, y);
        }

        if (oldest.Length > 1)
            gfx.DrawLines(pen, oldest);

        if (newest.Length > 1)
            gfx.DrawLines(pen, newest);
    }
}
