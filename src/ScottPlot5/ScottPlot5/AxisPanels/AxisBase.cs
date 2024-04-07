namespace ScottPlot.AxisPanels;

public abstract class AxisBase
{
    public bool IsVisible { get; set; } = true;

    public abstract Edge Edge { get; }

    public virtual CoordinateRangeMutable Range { get; private set; } = CoordinateRangeMutable.NotSet;
    public float MinimumSize { get; set; } = 0;
    public float MaximumSize { get; set; } = float.MaxValue;
    public float SizeWhenNoData { get; set; } = 15;

    public double Min
    {
        get => Range.Min;
        set => Range.Min = value;
    }

    public double Max
    {
        get => Range.Max;
        set => Range.Max = value;
    }

    public override string ToString()
    {
        return base.ToString() + " " + Range.ToString();
    }

    public virtual ITickGenerator TickGenerator { get; set; } = null!;

    public Label Label { get; private set; } = new()
    {
        Text = string.Empty,
        FontSize = 16,
        Bold = true,
        Rotation = -90,
    };

    public bool ShowDebugInformation { get; set; } = false;

    public LineStyle FrameLineStyle { get; } = new();

    public TickMarkStyle MajorTickStyle { get; set; } = new()
    {
        Length = 4,
        Width = 1,
        Color = Colors.Black
    };

    public TickMarkStyle MinorTickStyle { get; set; } = new()
    {
        Length = 2,
        Width = 1,
        Color = Colors.Black
    };

    public Label TickLabelStyle { get; set; } = new()
    {
        Alignment = Alignment.MiddleCenter
    };

    /// <summary>
    /// Apply a single color to all axis components: label, tick labels, tick marks, and frame
    /// </summary>
    public void Color(Color color)
    {
        Label.ForeColor = color;
        TickLabelStyle.ForeColor = color;
        MajorTickStyle.Color = color;
        MinorTickStyle.Color = color;
        FrameLineStyle.Color = color;
    }

    /// <summary>
    /// Draw a line along the edge of an axis on the side of the data area
    /// </summary>
    public static void DrawFrame(RenderPack rp, PixelRect panelRect, Edge edge, LineStyle lineStyle)
    {
        PixelLine pxLine = edge switch
        {
            Edge.Left => new(panelRect.Right, panelRect.Bottom, panelRect.Right, panelRect.Top),
            Edge.Right => new(panelRect.Left, panelRect.Bottom, panelRect.Left, panelRect.Top),
            Edge.Bottom => new(panelRect.Left, panelRect.Top, panelRect.Right, panelRect.Top),
            Edge.Top => new(panelRect.Left, panelRect.Bottom, panelRect.Right, panelRect.Bottom),
            _ => throw new NotImplementedException(edge.ToString()),
        };

        using SKPaint paint = new();
        Drawing.DrawLine(rp.Canvas, paint, pxLine, lineStyle);
    }

    private static void DrawTicksHorizontalAxis(RenderPack rp, Label label, PixelRect panelRect, IEnumerable<Tick> ticks, IAxis axis, TickMarkStyle majorStyle, TickMarkStyle minorStyle)
    {
        if (axis.Edge != Edge.Bottom && axis.Edge != Edge.Top)
        {
            throw new InvalidOperationException();
        }

        using SKPaint paint = new();
        label.ApplyToPaint(paint);

        paint.TextAlign = SKTextAlign.Center;

        foreach (Tick tick in ticks)
        {
            // draw tick
            paint.Color = tick.IsMajor ? majorStyle.Color.ToSKColor() : minorStyle.Color.ToSKColor();
            paint.StrokeWidth = tick.IsMajor ? majorStyle.Width : minorStyle.Width;
            float tickLength = tick.IsMajor ? majorStyle.Length : minorStyle.Length;
            float xPx = axis.GetPixel(tick.Position, panelRect);
            float y = axis.Edge == Edge.Bottom ? panelRect.Top : panelRect.Bottom;
            float yEdge = axis.Edge == Edge.Bottom ? y + tickLength : y - tickLength;
            PixelLine pxLine = new(xPx, y, xPx, yEdge);
            Drawing.DrawLine(rp.Canvas, paint, pxLine);

            // draw label
            if (!string.IsNullOrWhiteSpace(tick.Label) && label.IsVisible)
            {
                float fontSpacing = axis.Edge == Edge.Bottom ? paint.TextSize : -4;
                foreach (string line in tick.Label.Split('\n'))
                {
                    label.Text = line;
                    Pixel px = new(xPx, yEdge + fontSpacing);
                    label.Render(rp.Canvas, px);
                    fontSpacing += paint.TextSize;
                }
            }
        }
    }

    private static void DrawTicksVerticalAxis(RenderPack rp, Label label, PixelRect panelRect, IEnumerable<Tick> ticks, IAxis axis, TickMarkStyle majorStyle, TickMarkStyle minorStyle)
    {
        if (axis.Edge != Edge.Left && axis.Edge != Edge.Right)
        {
            throw new InvalidOperationException();
        }

        using SKPaint paint = new();
        label.ApplyToPaint(paint);
        label.Alignment = axis.Edge == Edge.Left ? Alignment.MiddleRight : Alignment.MiddleLeft;

        foreach (Tick tick in ticks)
        {
            // draw tick
            paint.Color = tick.IsMajor ? majorStyle.Color.ToSKColor() : minorStyle.Color.ToSKColor();
            paint.StrokeWidth = tick.IsMajor ? majorStyle.Width : minorStyle.Width;
            float tickLength = tick.IsMajor ? majorStyle.Length : minorStyle.Length;
            float x = axis.Edge == Edge.Left ? panelRect.Right : panelRect.Left;
            float y = axis.GetPixel(tick.Position, panelRect);
            float xEdge = axis.Edge == Edge.Left ? x - tickLength : x + tickLength;
            PixelLine pxLine = new(x, y, xEdge, y);
            Drawing.DrawLine(rp.Canvas, paint, pxLine);

            // draw label
            float majorTickLabelPadding = 7;
            float labelPos = axis.Edge == Edge.Left ? x - majorTickLabelPadding : x + majorTickLabelPadding;
            if (!string.IsNullOrWhiteSpace(tick.Label) && label.IsVisible)
            {
                string[] lines = tick.Label.Split('\n');
                double fontSpacing = -paint.TextSize * (lines.Length - 1) / 2;
                foreach (string line in lines)
                {
                    label.Text = line;
                    Pixel px = new(labelPos, y + fontSpacing);
                    label.Render(rp.Canvas, px);
                    fontSpacing += paint.TextSize;
                }
            }
        }
    }

    public static void DrawTicks(RenderPack rp, Label label, PixelRect panelRect, IEnumerable<Tick> ticks, IAxis axis, TickMarkStyle majorStyle, TickMarkStyle minorStyle)
    {
        if (axis.Edge.IsVertical())
            DrawTicksVerticalAxis(rp, label, panelRect, ticks, axis, majorStyle, minorStyle);
        else
            DrawTicksHorizontalAxis(rp, label, panelRect, ticks, axis, majorStyle, minorStyle);
    }
}
