using System;
using System.Drawing;

namespace ScottPlot.Plottable;

/// <summary>
/// This class represents a single Bar shown as part of a collection of Bars on a <see cref="BarSeries"/> plot
/// </summary>
public class Bar
{
    /// <summary>
    /// Horizontal position of the center of the bar 
    /// (or vertical position if it's a horizontal bar)
    /// </summary>
    public double Position { get; set; }

    /// <summary>
    /// Vertical position of the top of the bar 
    /// (or right edge if it's a horizontal bar)
    /// </summary>
    public double Value { get; set; }

    /// <summary>
    /// Vertical position of the bottom of the bar 
    /// (or left edge if it's a horizontal bar)
    /// </summary>
    public double ValueBase { get; set; }

    /// <summary>
    /// Text to display above the bar
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// Styling of the text displayed above the bar
    /// </summary>
    public readonly Drawing.Font Font = new();

    /// <summary>
    /// Horizontal width of the bar in axis units
    /// (or vertical height if it's a horizontal bar)
    /// </summary>
    public double Thickness { get; set; } = .8;

    /// <summary>
    /// Color filling the rectangular area of the bar
    /// </summary>
    public Color FillColor { get; set; } = Color.Gray;

    /// <summary>
    /// Color of the line outlining the rectangular area of the bar
    /// </summary>
    public Color LineColor { get; set; } = Color.Black;

    /// <summary>
    /// Width of the line outlining the rectangular area of the bar
    /// </summary>
    public float LineWidth { get; set; } = 0;

    /// <summary>
    /// Indicates whether bars extend upward (vertical, default) or two the right (horizontal)
    /// </summary>
    public bool IsVertical { get; set; } = true;

    public AxisLimits GetLimits()
    {
        double top = Math.Max(Value, ValueBase);
        double bottom = Math.Min(Value, ValueBase);
        double left = Position - Thickness / 2;
        double right = Position + Thickness / 2;

        // Make room for labels, if present
        if (Label is not null)
        {
            double span = top - bottom;
            top += 0.1 * span;
            bottom -= 0.1 * span;
        }

        return IsVertical
            ? new AxisLimits(left, right, bottom, top)
            : new AxisLimits(bottom, top, left, right);
    }
}
