﻿namespace ScottPlot;

/// <summary>
/// Represents a single bar in a bar chart
/// </summary>
public class Bar
{
    public double Position;
    public double Value;
    public double ValueBase = 0;
    public double Error = 0;

    public Color FillColor = Colors.Gray;
    public Color BorderColor = Colors.Black;
    public Color ErrorColor = Colors.Black;

    public double Size = 0.8; // coordinate units
    public double ErrorSize = 0.2; // coordinate units
    public float BorderLineWidth = 1;
    public float ErrorLineWidth = 0;

    public LinePattern BarLinePattern { get; set; } = LinePattern.Solid;

    // TODO: something like ErrorInDirectionOfValue?
    // Maybe ErrorPosition should be an enum containing: None, Upward, Downward, Both, or Extend
    public bool ErrorPositive = true;
    public bool ErrorNegative = true;

    public Orientation Orientation = Orientation.Vertical;

    internal CoordinateRect Rect
    {
        get
        {
            return Orientation == Orientation.Vertical
                ? new CoordinateRect(
                    left: Position - Size / 2,
                    right: Position + Size / 2,
                    bottom: ValueBase,
                    top: Value)
                : new CoordinateRect(
                    left: ValueBase,
                    right: Value,
                    bottom: Position - Size / 2,
                    top: Position + Size / 2);
        }
    }

    internal IEnumerable<CoordinateLine> ErrorLines
    {
        get
        {
            CoordinateLine center, top, bottom;
            if (Orientation == Orientation.Vertical)
            {
                center = new(Position, Value - Error, Position, Value + Error);
                top = new(Position - ErrorSize / 2, Value + Error, Position + ErrorSize / 2, Value + Error);
                bottom = new(Position - ErrorSize / 2, Value - Error, Position + ErrorSize / 2, Value - Error);
            }
            else
            {
                center = new(Value - Error, Position, Value + Error, Position);
                top = new(Value + Error, Position - ErrorSize / 2, Value + Error, Position + ErrorSize / 2);
                bottom = new(Value - Error, Position - ErrorSize / 2, Value - Error, Position + ErrorSize / 2);
            }

            return new List<CoordinateLine>() { center, top, bottom };
        }
    }

    internal AxisLimits AxisLimits
    {
        get
        {
            return Orientation == Orientation.Vertical
                ? new AxisLimits(
                    left: Position - Size / 2,
                    right: Position + Size / 2,
                    bottom: Math.Min(ValueBase, Value - Error),
                    top: Value + Error)
                : new AxisLimits(
                    left: Math.Min(ValueBase, Value - Error),
                    right: Value + Error,
                    bottom: Position - Size / 2,
                    top: Position + Size / 2);
        }
    }

    public void Render(RenderPack rp, IAxes axes, SKPaint paint)
    {
        PixelRect rect = axes.GetPixelRect(Rect);
        Drawing.Fillectangle(rp.Canvas, rect, FillColor);
        Drawing.DrawRectangle(rp.Canvas, rect, BorderColor, BorderLineWidth, BarLinePattern);

        if (Error == 0)
            return;

        foreach (CoordinateLine line in ErrorLines)
        {
            Pixel pt1 = axes.GetPixel(line.Start);
            Pixel pt2 = axes.GetPixel(line.End);
            Drawing.DrawLine(rp.Canvas, paint, pt1, pt2, BorderColor, BorderLineWidth, true, BarLinePattern);
        }
    }
}
