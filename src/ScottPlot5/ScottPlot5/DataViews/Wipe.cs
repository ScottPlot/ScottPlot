using ScottPlot.Plottables;

namespace ScottPlot.DataViews;

public class Wipe : IDataStreamerView
{
    private readonly bool WipeRight;

    public DataStreamer Streamer { get; }

    //TODO: Add a BlankFraction property that adds a gap between old and new data

    public Wipe(DataStreamer streamer, bool wipeRight)
    {
        Streamer = streamer;
        WipeRight = wipeRight;
    }

    public void Render(RenderPack rp)
    {
        int newestCount = Streamer.NextIndex;
        int oldestCount = Streamer.Data.Length - newestCount;

        double xMax = Streamer.Data.Length * Streamer.SamplePeriod + Streamer.OffsetX;

        Pixel[] newest = new Pixel[newestCount];
        Pixel[] oldest = new Pixel[oldestCount];

        for (int i = 0; i < newest.Length; i++)
        {
            double xPos = i * Streamer.SamplePeriod + Streamer.OffsetX;
            float x = Streamer.Axes.GetPixelX(WipeRight ? xPos : xMax - xPos);
            float y = Streamer.Axes.GetPixelY(Streamer.Data[i] + Streamer.OffsetY);
            newest[i] = new(x, y);
        }

        for (int i = 0; i < oldest.Length; i++)
        {
            double xPos = (i + Streamer.NextIndex) * Streamer.SamplePeriod + Streamer.OffsetX;
            float x = Streamer.Axes.GetPixelX(WipeRight ? xPos : xMax - xPos);
            float y = Streamer.Axes.GetPixelY(Streamer.Data[i + Streamer.NextIndex] + Streamer.OffsetY);
            oldest[i] = new(x, y);
        }

        using SKPaint paint = new();
        Drawing.DrawLines(rp.Canvas, paint, oldest, Streamer.LineStyle);
        Drawing.DrawLines(rp.Canvas, paint, newest, Streamer.LineStyle);
    }
}
