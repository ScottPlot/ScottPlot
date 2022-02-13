using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScottPlot;

public abstract class AxisLabel
{
    public readonly Edge Edge;
    public Label Label = new();
    public float Size = 10;

    public AxisLabel(Edge edge, float size, string label)
    {
        Edge = edge;
        Label.Text = label;
        Size = size;
    }

    public abstract void Draw(ICanvas canvas, PlotInfo info);
}

public class HorizontalAxisLabel : AxisLabel
{
    public HorizontalAxisLabel(Edge edge, float size, string label) : base(edge, size, label)
    {
        if (!(edge == Edge.Bottom || edge == Edge.Top))
            throw new ArgumentException("invalid edge for a horizontal axis");
    }

    public override void Draw(ICanvas canvas, PlotInfo info)
    {
        float xCenter = info.DataRect.HorizontalCenter;
        float yBottom = info.Height;

        canvas.DrawString(Label.Text, xCenter, yBottom, HorizontalAlignment.Center);
    }
}

public class VerticalAxisLabel : AxisLabel
{
    public VerticalAxisLabel(Edge edge, float size, string label) : base(edge, size, label)
    {
        if (!(edge == Edge.Left || edge == Edge.Right))
            throw new ArgumentException("invalid edge for a vertical axis");
    }

    public override void Draw(ICanvas canvas, PlotInfo info)
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