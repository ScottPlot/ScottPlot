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
        Pixel[] points = new Pixel[Streamer.Data.Length];

        int oldPointCount = Streamer.Data.Length - Streamer.NextIndex;

        for (int i = 0; i < Streamer.Data.Length; i++)
        {
            bool isNewPoint = i < oldPointCount;
            int sourceIndex = isNewPoint ? Streamer.NextIndex + i : i - oldPointCount;
            int targetIndex = NewOnRight ? i : Streamer.Data.Length - 1 - i;
            points[targetIndex] = new(
                x: Streamer.Axes.GetPixelX(targetIndex * Streamer.SamplePeriod + Streamer.OffsetX),
                y: Streamer.Axes.GetPixelY(Streamer.Data[sourceIndex] + Streamer.OffsetY));
        }

        using SKPaint paint = new();
        Drawing.DrawLines(rp.Canvas, paint, points, Streamer.LineStyle);
    }
}
