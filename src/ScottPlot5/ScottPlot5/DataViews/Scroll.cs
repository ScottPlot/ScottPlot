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
        Pixel[] points = new Pixel[Streamer.DataSource.Length];

        int oldPointCount = Streamer.DataSource.Length - Streamer.DataSource.NextIndex;

        for (int i = 0; i < Streamer.DataSource.Length; i++)
        {
            bool isNewPoint = i < oldPointCount;
            int sourceIndex = isNewPoint ? Streamer.DataSource.NextIndex + i : i - oldPointCount;
            int targetIndex = NewOnRight ? i : Streamer.DataSource.Data.Length - 1 - i;
            points[targetIndex] = new(
                x: Streamer.Axes.GetPixelX(targetIndex * Streamer.DataSource.SamplePeriod + Streamer.DataSource.OffsetX),
                y: Streamer.Axes.GetPixelY(Streamer.DataSource.Data[sourceIndex] + Streamer.DataSource.OffsetY));
        }

        using SKPaint paint = new();
        Drawing.DrawLines(rp.Canvas, paint, points, Streamer.LineStyle);
    }
}
