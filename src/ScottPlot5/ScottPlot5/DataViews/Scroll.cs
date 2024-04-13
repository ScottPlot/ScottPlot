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
        int dataLength = Streamer.Data.Length;
        int dataNextIndex = Streamer.Data.NextIndex;
        int oldPointCount = dataLength - dataNextIndex;

        Pixel[] points = new Pixel[dataLength];

        for (int i = 0; i < dataLength; i++)
        {
            bool isNewPoint = i < oldPointCount;
            int sourceIndex = isNewPoint ? dataNextIndex + i : i - oldPointCount;
            int targetIndex = NewOnRight ? i : dataLength - 1 - i;
            points[targetIndex] = new(
                x: Streamer.Axes.GetPixelX(targetIndex * Streamer.Data.SamplePeriod + Streamer.Data.OffsetX),
                y: Streamer.Axes.GetPixelY(Streamer.Data.Data[sourceIndex] + Streamer.Data.OffsetY));
        }

        using SKPaint paint = new();
        Drawing.DrawLines(rp.Canvas, paint, points, Streamer.LineStyle);
    }
}
