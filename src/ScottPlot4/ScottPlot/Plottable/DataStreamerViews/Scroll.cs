using System.Drawing;

namespace ScottPlot.Plottable.DataStreamerViews;

internal class Scroll : IDataStreamerView
{
    private readonly bool newOnRight;

    public Scroll(bool newOnRight)
    {
        this.newOnRight = newOnRight;
    }

    public void Render(DataStreamer streamer, PlotDimensions dims, Graphics gfx, Pen pen)
    {
        PointF[] points = new PointF[streamer.Data.Length];

        int oldPointCount = streamer.Data.Length - streamer.DataIndex;

        for (int i = 0; i < streamer.Data.Length; i++)
        {
            bool isNewPoint = i < oldPointCount;
            int sourceIndex = isNewPoint ? streamer.DataIndex + i : i - oldPointCount;
            int targetIndex = newOnRight ? i : streamer.Data.Length - 1 - i;
            points[targetIndex] = new(
                x: dims.GetPixelX(targetIndex * streamer.SamplePeriod + streamer.OffsetX),
                y: dims.GetPixelY(streamer.Data[sourceIndex] + streamer.OffsetY));
        }

        gfx.DrawLines(pen, points);
    }
}
