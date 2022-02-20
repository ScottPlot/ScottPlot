using Microsoft.Maui.Graphics;
using System;

namespace ScottPlot.Axes;

public class BottomAxis : HorizontalAxis, IAxis
{
    public Edge Edge => Edge.Bottom;

    public BottomAxis(string text = "")
    {
        Label.Text = text;
    }

    public PixelSize GetSize(ICanvas canvas)
    {
        float height = MeasureHeight(canvas);
        return new PixelSize(float.NaN, height);
    }

    public void Draw(ICanvas canvas, PlotInfo info)
    {
        float xCenter = info.DataRect.HorizontalCenter;
        float yBottom = info.Height;
        Label.Draw(canvas, xCenter, yBottom, HorizontalAlignment.Center, VerticalAlignment.Bottom);
    }
}
