using Microsoft.Maui.Graphics;
using System;

namespace ScottPlot.Axes;

public class LeftAxis : VerticalAxis, IAxis
{
    public Edge Edge => Edge.Left;

    public LeftAxis(string text = "")
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
        // NOTE: use MeasureString() when Maui matures
        float textHeight = 12;
        float xLeft = 0;
        float yCenter = info.DataRect.VerticalCenter;
        canvas.SaveState();
        canvas.Translate(xLeft + textHeight, yCenter);
        canvas.Rotate(-90);
        canvas.DrawString(Label.Text, 0, 0, HorizontalAlignment.Center);
        canvas.ResetState();
    }
}
