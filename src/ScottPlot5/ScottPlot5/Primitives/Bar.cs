namespace ScottPlot;

/// <summary>
/// Represents a single bar in a bar chart
/// </summary>
public class Bar : IHasFill, IHasLine
{
    public double Position { get; set; }
    public double Value { get; set; }
    public double ValueBase { get; set; }
    public double Size { get; set; } = 0.8; // coordinate units

    /// <summary>
    /// Size of the error bar extending from <see cref="Value"/>
    /// </summary>
    public double Error { get; set; } = 0;

    /// <summary>
    /// Width of the error bar whiskers in axis units (same units as <see cref="Position"/>)
    /// </summary>
    public double ErrorSize { get; set; } = 0.2; // coordinate units

    public bool IsVisible { get; set; } = true;

    public FillStyle FillStyle { get; set; } = new() { Color = Colors.Gray, IsVisible = true };
    public Color FillHatchColor { get => FillStyle.HatchColor; set => FillStyle.HatchColor = value; }
    public IHatch? FillHatch { get => FillStyle.Hatch; set => FillStyle.Hatch = value; }
    public Color FillColor { get => FillStyle.Color; set => FillStyle.Color = value; }

    public LineStyle LineStyle { get; set; } = new() { Color = Colors.Black, Width = 1, IsVisible = true };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    [Obsolete("use LineColor", true)] public Color BorderColor { get; set; }
    [Obsolete("use LineWidth", true)] public float BorderLineWidth { get; set; } = 1;
    [Obsolete("use LineWidth", true)] public float ErrorLineWidth { get; set; } = 0;

    // TODO: something like ErrorInDirectionOfValue?
    // Maybe ErrorPosition should be an enum containing: None, Upward, Downward, Both, or Extend
    public bool ErrorPositive { get; set; } = true;
    public bool ErrorNegative { get; set; } = true;

    public string Label { get; set; } = string.Empty;
    public bool CenterLabel { get; set; } = false;
    public float LabelOffset { get; set; } = 5;

    public Orientation Orientation { get; set; } = Orientation.Vertical;

    public CoordinateRect Rect
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

    public IEnumerable<CoordinateLine> ErrorLines
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

    public AxisLimits AxisLimits
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

    public void Render(RenderPack rp, IAxes axes, SKPaint paint, LabelStyle labelStyle)
    {
        if (!IsVisible)
            return;

        PixelRect rect = axes.GetPixelRect(Rect);
        Drawing.FillRectangle(rp.Canvas, rect, paint, FillStyle);
        Drawing.DrawRectangle(rp.Canvas, rect, paint, LineStyle);

        if (Error != 0)
        {
            foreach (CoordinateLine line in ErrorLines)
            {
                Pixel pt1 = axes.GetPixel(line.Start);
                Pixel pt2 = axes.GetPixel(line.End);
                PixelLine pxLine = new(pt1, pt2);
                Drawing.DrawLine(rp.Canvas, paint, pxLine, LineStyle);
            }
        }

        if (CenterLabel)
        {
            labelStyle.Alignment = Alignment.MiddleCenter;
            labelStyle.Render(rp.Canvas, rect.Center, paint);
            return;
        }

        if (Orientation == Orientation.Vertical)
        {
            float xPx = rect.HorizontalCenter;
            float yPx = rect.Top;
            labelStyle.Alignment = Alignment.LowerCenter;
            Pixel labelPixel = new(xPx, yPx - LabelOffset);
            labelStyle.Render(rp.Canvas, labelPixel, paint);
        }
        else
        {
            MeasuredText measured = labelStyle.Measure(labelStyle.Text, paint);

            if (Value < 0)
            {
                float xPx = rect.LeftCenter.X - (LabelOffset + measured.Width / 2);
                float yPx = rect.LeftCenter.Y + (measured.Height / 2);
                Pixel labelPixel = new(xPx, yPx);
                labelStyle.Render(rp.Canvas, labelPixel, paint);
            }
            else
            {
                float xPx = rect.RightCenter.X + (LabelOffset + measured.Width / 2);
                float yPx = rect.RightCenter.Y + (measured.Height / 2);
                Pixel labelPixel = new(xPx, yPx);
                labelStyle.Render(rp.Canvas, labelPixel, paint);
            }
        }

    }
}
