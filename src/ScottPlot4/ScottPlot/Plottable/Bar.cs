using System;
using System.Drawing;

namespace ScottPlot.Plottable;

public class Bar
{
    public double Position { get; set; }
    public double Value { get; set; }
    public double ValueBase { get; set; }
    public string Label { get; set; }
    public readonly Drawing.Font Font = new();
    public double Thickness { get; set; } = .8;
    public Color FillColor { get; set; } = Color.Gray;
    public Color LineColor { get; set; } = Color.Black;
    public float LineWidth { get; set; } = 0;
    public bool IsVertical { get; set; } = true;

    public AxisLimits GetLimits()
    {
        double top = Math.Max(Value, ValueBase);
        double bottom = Math.Min(Value, ValueBase);
        double left = Position - Thickness / 2;
        double right = Position + Thickness / 2;

        return IsVertical
            ? new AxisLimits(left, right, bottom, top)
            : new AxisLimits(bottom, top, left, right);
    }
}
