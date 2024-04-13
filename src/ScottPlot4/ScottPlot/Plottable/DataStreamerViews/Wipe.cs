using System.Drawing;

namespace ScottPlot.Plottable.DataStreamerViews;

internal class Wipe : IDataStreamerView
{
    private readonly bool WipeRight;

    public DataStreamer Streamer { get; }

    //TODO: Add a BlankFraction property that adds a gap between old and new data

    public Wipe(DataStreamer streamer, bool wipeRight)
    {
        Streamer = streamer;
        WipeRight = wipeRight;
    }

    public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
    {
        int newestCount = Streamer.NextIndex;
        int oldestCount = Streamer.Data.Length - newestCount;

        double xMax = Streamer.Data.Length * Streamer.SamplePeriod + Streamer.OffsetX;

        PointF[] newest = new PointF[newestCount];
        PointF[] oldest = new PointF[oldestCount];

        for (int i = 0; i < newest.Length; i++)
        {
            double xPos = i * Streamer.SamplePeriod + Streamer.OffsetX;
            float x = dims.GetPixelX(WipeRight ? xPos : xMax - xPos);
            float y = dims.GetPixelY(Streamer.Data[i] + Streamer.OffsetY);
            newest[i] = new(x, y);
        }

        for (int i = 0; i < oldest.Length; i++)
        {
            double xPos = (i + Streamer.NextIndex) * Streamer.SamplePeriod + Streamer.OffsetX;
            float x = dims.GetPixelX(WipeRight ? xPos : xMax - xPos);
            float y = dims.GetPixelY(Streamer.Data[i + Streamer.NextIndex] + Streamer.OffsetY);
            oldest[i] = new(x, y);
        }

        using var gfx = ScottPlot.Drawing.GDI.Graphics(bmp, dims, lowQuality);
        using var pen = ScottPlot.Drawing.GDI.Pen(Streamer.Color, Streamer.LineWidth, Streamer.LineStyle);

        if (oldest.Length > 1)
            gfx.DrawLines(pen, oldest);

        if (newest.Length > 1)
            gfx.DrawLines(pen, newest);
    }
}
