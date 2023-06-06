using System;
using System.Drawing;

namespace ScottPlot.Plottable.DataStreamerViews;

internal class Wipe : IDataStreamerView
{
    private readonly bool WipeRight;

    //TODO: Add a BlankFraction property that adds a gap between old and new data

    public Wipe(bool wipeRight)
    {
        WipeRight = wipeRight;
    }

    public void Render(DataStreamer streamer, PlotDimensions dims, Graphics gfx, Pen pen)
    {
        int newestCount = streamer.DataIndex;
        int oldestCount = streamer.Data.Length - newestCount;

        double xMax = streamer.Data.Length * streamer.SamplePeriod + streamer.OffsetX;

        PointF[] newest = new PointF[newestCount];
        PointF[] oldest = new PointF[oldestCount];

        for (int i = 0; i < newest.Length; i++)
        {
            double xPos = i * streamer.SamplePeriod + streamer.OffsetX;
            float x = dims.GetPixelX(WipeRight ? xPos : xMax - xPos);
            float y = dims.GetPixelY(streamer.Data[i] + streamer.OffsetY);
            newest[i] = new(x, y);
        }

        for (int i = 0; i < oldest.Length; i++)
        {
            double xPos = (i + streamer.DataIndex) * streamer.SamplePeriod + streamer.OffsetX;
            float x = dims.GetPixelX(WipeRight ? xPos : xMax - xPos);
            float y = dims.GetPixelY(streamer.Data[i + streamer.DataIndex] + streamer.OffsetY);
            oldest[i] = new(x, y);
        }

        if (oldest.Length > 1)
            gfx.DrawLines(pen, oldest);

        if (newest.Length > 1)
            gfx.DrawLines(pen, newest);
    }
}
