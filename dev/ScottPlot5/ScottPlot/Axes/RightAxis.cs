using Microsoft.Maui.Graphics;
using System;

namespace ScottPlot.Axes;

public class RightAxis : VerticalAxis, IAxis
{
    public Edge Edge => Edge.Right;

    public RightAxis(string text = "")
    {
        Label.Text = text;
    }

    public PixelSize GetSize(ICanvas canvas)
    {
        float width = MeasureWidth(canvas);
        return new PixelSize(width, float.NaN);
    }

    public void Draw(ICanvas canvas, PlotInfo info)
    {
        // TODO: render right axis
    }
}