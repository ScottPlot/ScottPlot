using Microsoft.Maui.Graphics;
using System;

namespace ScottPlot.Axes;

public class TopAxis : HorizontalAxis, IAxis
{
    public Edge Edge => Edge.Top;

    public TopAxis(string text = "")
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
        //TODO: render top axis
    }
}