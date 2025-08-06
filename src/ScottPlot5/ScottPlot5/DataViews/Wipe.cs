using ScottPlot.Plottables;

namespace ScottPlot.DataViews;

public class Wipe : IDataStreamerView
{
    private readonly bool WipeRight;

    public DataStreamer Streamer { get; }

    public double BlankFraction { get; set; } = 0;

    public Wipe(DataStreamer streamer, bool wipeRight)
    {
        Streamer = streamer;
        WipeRight = wipeRight;
    }

    public void Render(RenderPack rp)
    {
        using SKPaint paint = new();
        foreach (Pixel[] seg in GetSegments(rp))
        {
            Drawing.DrawLines(rp.Canvas, paint, seg, Streamer.LineStyle);
            Drawing.DrawMarkers(rp.Canvas, paint, seg, Streamer.MarkerStyle);
        }
    }

    public IReadOnlyList<Pixel[]> GetSegments(RenderPack rp)
    {
        int newestCount = Streamer.Data.NextIndex;
        int oldestCount = Streamer.Data.Data.Length - newestCount;

        double xMax = Streamer.Data.Data.Length * Streamer.Data.SamplePeriod + Streamer.Data.OffsetX;

        Pixel[] newest = new Pixel[newestCount];
        Pixel[] oldest = new Pixel[oldestCount];

        for (int i = 0; i < newest.Length; i++)
        {
            double xPos = i * Streamer.Data.SamplePeriod + Streamer.Data.OffsetX;
            float x = Streamer.Axes.GetPixelX(WipeRight ? xPos : xMax - xPos);
            float y = Streamer.Axes.GetPixelY(Streamer.Data.Data[i] + Streamer.Data.OffsetY);
            newest[i] = new Pixel(x, y);
        }

        for (int i = 0; i < oldest.Length; i++)
        {
            double xPos = (i + newestCount) * Streamer.Data.SamplePeriod + Streamer.Data.OffsetX;
            float x = Streamer.Axes.GetPixelX(WipeRight ? xPos : xMax - xPos);
            float y = Streamer.Axes.GetPixelY(Streamer.Data.Data[i + newestCount] + Streamer.Data.OffsetY);
            oldest[i] = new Pixel(x, y);
        }

        if (BlankFraction > 0)
        {
            int blank = (int)(BlankFraction * Streamer.Data.Length);

            if (blank <= oldest.Length)
                oldest = oldest.Skip(blank).ToArray();
            else
            {
                newest = newest.Skip(blank - oldest.Length).ToArray();
                oldest = Array.Empty<Pixel>();
            }
        }

        if (!WipeRight)
        {
            System.Array.Reverse(newest);
            System.Array.Reverse(oldest);
        }

        var list = new List<Pixel[]>();

        if (WipeRight)
        {
            if (oldest.Length > 1) list.Add(oldest);
            if (newest.Length > 1) list.Add(newest);
        }
        else
        {
            if (newest.Length > 1) list.Add(newest);
            if (oldest.Length > 1) list.Add(oldest);
        }

        return list;
    }
}
