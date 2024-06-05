using ScottPlot.TickGenerators;

namespace ScottPlot.AxisPanels;

public abstract class AxisBase : LabelStyleProperties
{
    public bool IsVisible { get; set; } = true;

    public abstract Edge Edge { get; }

    public virtual CoordinateRangeMutable Range { get; private set; } = CoordinateRangeMutable.NotSet;
    public float MinimumSize { get; set; } = 0;
    public float MaximumSize { get; set; } = float.MaxValue;
    public float SizeWhenNoData { get; set; } = 15;
    public PixelPadding EmptyLabelPadding { get; set; } = new(10, 5);
    public PixelPadding PaddingBetweenTickAndAxisLabels { get; set; } = new(5, 3);
    public PixelPadding PaddingOutsideAxisLabels { get; set; } = new(2, 2);

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

    [Obsolete("use LabelText, LabelFontColor, LabelFontSize, LabelFontName, etc. or properties of LabelStyle", false)]
    public Label Label => LabelStyle;

    public override Label LabelStyle { get; set; } = new()
    {
        Text = string.Empty,
        FontSize = 16,
        Bold = true,
        Rotation = -90,
    };

    public bool ShowDebugInformation { get; set; } = false;

    public LineStyle FrameLineStyle { get; } = new() { Width = 1 };

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
        LabelStyle.ForeColor = color;
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
            if (string.IsNullOrWhiteSpace(tick.Label) || !label.IsVisible)
                continue;
            label.Text = tick.Label;
            float pxDistanceFromTick = 2;
            float pxDistanceFromEdge = tickLength + pxDistanceFromTick;
            float yPx = axis.Edge == Edge.Bottom ? y + pxDistanceFromEdge : y - pxDistanceFromEdge;
            Pixel labelPixel = new(xPx, yPx);
            if (label.Rotation == 0)
                label.Alignment = axis.Edge == Edge.Bottom ? Alignment.UpperCenter : Alignment.LowerCenter;
            label.Render(rp.Canvas, labelPixel, paint);
        }
    }

    private static void DrawTicksVerticalAxis(RenderPack rp, Label label, PixelRect panelRect, IEnumerable<Tick> ticks, IAxis axis, TickMarkStyle majorStyle, TickMarkStyle minorStyle)
    {
        if (axis.Edge != Edge.Left && axis.Edge != Edge.Right)
        {
            throw new InvalidOperationException();
        }

        using SKPaint paint = new();

        foreach (Tick tick in ticks)
        {
            // draw tick
            paint.Color = tick.IsMajor ? majorStyle.Color.ToSKColor() : minorStyle.Color.ToSKColor();
            paint.StrokeWidth = tick.IsMajor ? majorStyle.Width : minorStyle.Width;
            float tickLength = tick.IsMajor ? majorStyle.Length : minorStyle.Length;
            float yPx = axis.GetPixel(tick.Position, panelRect);
            float x = axis.Edge == Edge.Left ? panelRect.Right : panelRect.Left;
            float xEdge = axis.Edge == Edge.Left ? x - tickLength : x + tickLength;
            PixelLine pxLine = new(x, yPx, xEdge, yPx);
            Drawing.DrawLine(rp.Canvas, paint, pxLine);

            // draw label
            if (string.IsNullOrWhiteSpace(tick.Label) || !label.IsVisible)
                continue;
            label.Text = tick.Label; float pxDistanceFromTick = 5;
            float pxDistanceFromEdge = tickLength + pxDistanceFromTick;
            float xPx = axis.Edge == Edge.Left ? x - pxDistanceFromEdge : x + pxDistanceFromEdge;
            Pixel px = new(xPx, yPx);
            if (label.Rotation == 0)
                label.Alignment = axis.Edge == Edge.Left ? Alignment.MiddleRight : Alignment.MiddleLeft;
            label.Render(rp.Canvas, px, paint);
        }
    }

    public static void DrawTicks(RenderPack rp, Label label, PixelRect panelRect, IEnumerable<Tick> ticks, IAxis axis, TickMarkStyle majorStyle, TickMarkStyle minorStyle)
    {
        if (axis.Edge.IsVertical())
            DrawTicksVerticalAxis(rp, label, panelRect, ticks, axis, majorStyle, minorStyle);
        else
            DrawTicksHorizontalAxis(rp, label, panelRect, ticks, axis, majorStyle, minorStyle);
    }

    /// <summary>
    /// Replace the <see cref="TickGenerator"/> with a <see cref="NumericManual"/> pre-loaded with the given ticks.
    /// </summary>
    public void SetTicks(double[] xs, string[] labels)
    {
        if (xs.Length != labels.Length)
            throw new ArgumentException($"{nameof(xs)} and {nameof(labels)} must have equal length");

        NumericManual manualTickGen = new();

        for (int i = 0; i < xs.Length; i++)
        {
            manualTickGen.AddMajor(xs[i], labels[i]);
        }

        TickGenerator = manualTickGen;
    }
}
