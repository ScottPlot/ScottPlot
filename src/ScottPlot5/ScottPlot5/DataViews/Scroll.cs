using ScottPlot.Plottables;

namespace ScottPlot.DataViews;

public class Scroll : IDataStreamerView
{
    private readonly bool NewOnRight;

    public DataStreamer Streamer { get; }

    public Scroll(DataStreamer streamer, bool newOnRight)
    {
        Streamer = streamer;
        NewOnRight = newOnRight;
    }

    public void Render(RenderPack rp)
    {
        foreach (Pixel[] seg in GetSegments(rp))
        {
            Drawing.DrawLines(rp.Canvas, rp.Paint, seg, Streamer.LineStyle);
            Drawing.DrawMarkers(rp.Canvas, rp.Paint, seg, Streamer.MarkerStyle);
        }
    }

    public IReadOnlyList<Pixel[]> GetSegments(RenderPack rp)
    {
        int cap = Streamer.Data.Length;
        int samples = Math.Min(Streamer.Data.CountTotal, cap);
        if (samples < 2)
            return Array.Empty<Pixel[]>();

        int nextIdx = Streamer.Data.NextIndex;
        bool wrapped = samples == cap;
        int oldCnt = wrapped ? cap - nextIdx : 0;

        Span<double> vals = stackalloc double[samples];
        int dst = 0;

        if (wrapped && oldCnt > 0)
        {
            for (int i = 0; i < oldCnt; i++, dst++)
                vals[dst] = Streamer.Data.Data[nextIdx + i];
        }

        for (int i = 0; i < nextIdx && dst < samples; i++, dst++)
            vals[dst] = Streamer.Data.Data[i];

        int xShift = NewOnRight ? cap - samples : 0;

        Pixel[] px = new Pixel[samples];
        for (int screenX = 0; screenX < samples; screenX++)
        {
            int valIdx = NewOnRight
                ? screenX
                : samples - 1 - screenX;

            int plotIdx = screenX + xShift;
            double xVal = plotIdx * Streamer.Data.SamplePeriod + Streamer.Data.OffsetX;
            double yVal = vals[valIdx] + Streamer.Data.OffsetY;

            px[screenX] = new Pixel(
                Streamer.Axes.GetPixelX(xVal),
                Streamer.Axes.GetPixelY(yVal));
        }

        return new[] { px };
    }
}
