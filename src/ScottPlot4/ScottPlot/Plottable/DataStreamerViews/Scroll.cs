using System.Drawing;

namespace ScottPlot.Plottable.DataStreamerViews;

internal class Scroll : IDataStreamerView
{
    private readonly bool NewOnRight;

    public DataStreamer Streamer { get; }

    public Scroll(DataStreamer streamer, bool newOnRight)
    {
        Streamer = streamer;
        NewOnRight = newOnRight;
    }

    public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
    {
        PointF[] points = new PointF[Streamer.Data.Length];

        int oldPointCount = Streamer.Data.Length - Streamer.NextIndex;

        for (int i = 0; i < Streamer.Data.Length; i++)
        {
            bool isNewPoint = i < oldPointCount;
            int sourceIndex = isNewPoint ? Streamer.NextIndex + i : i - oldPointCount;
            int targetIndex = NewOnRight ? i : Streamer.Data.Length - 1 - i;
            points[targetIndex] = new(
                x: dims.GetPixelX(targetIndex * Streamer.SamplePeriod + Streamer.OffsetX),
                y: dims.GetPixelY(Streamer.Data[sourceIndex] + Streamer.OffsetY));
        }

        using var gfx = ScottPlot.Drawing.GDI.Graphics(bmp, dims, lowQuality);
        using var pen = ScottPlot.Drawing.GDI.Pen(Streamer.Color, Streamer.LineWidth, Streamer.LineStyle);

        if (points.Length > 1)
            gfx.DrawLines(pen, points);
    }
}
