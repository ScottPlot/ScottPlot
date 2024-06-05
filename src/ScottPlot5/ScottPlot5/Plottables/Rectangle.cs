namespace ScottPlot.Plottables;

public class Rectangle : IPlottable, IHasLine, IHasFill, IHasLegendText
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => [new LegendItem()
    {
        LabelText = LegendText,
        FillStyle = FillStyle,
        OutlineStyle = LineStyle,
    }];

    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public FillStyle FillStyle { get; set; } = new();
    public Color FillColor { get => FillStyle.Color; set => FillStyle.Color = value; }
    public Color FillHatchColor { get => FillStyle.HatchColor; set => FillStyle.HatchColor = value; }
    public IHatch? FillHatch { get => FillStyle.Hatch; set => FillStyle.Hatch = value; }


    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get; set; } = string.Empty;

    public double X1 { get; set; }
    public double X2 { get; set; }
    public double Y1 { get; set; }
    public double Y2 { get; set; }

    public CoordinateRect CoordinateRect
    {
        get
        {
            double left = Math.Min(X1, X2);
            double right = Math.Max(X1, X2);
            double bottom = Math.Min(Y1, Y2);
            double top = Math.Max(Y1, Y2);
            return new CoordinateRect(left, right, bottom, top);
        }
        set
        {
            X1 = value.Left;
            X2 = value.Right;
            Y1 = value.Bottom;
            Y2 = value.Top;
        }
    }

    public AxisLimits GetAxisLimits() => new(CoordinateRect);

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();

        PixelRect rect = Axes.GetPixelRect(CoordinateRect);
        FillStyle.ApplyToPaint(paint, rect);
        Drawing.FillRectangle(rp.Canvas, rect, paint);

        LineStyle.ApplyToPaint(paint);
        Drawing.DrawRectangle(rp.Canvas, rect, paint);
    }
}
