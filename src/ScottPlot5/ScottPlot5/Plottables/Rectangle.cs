
namespace ScottPlot.Plottables;

public class Rectangle : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(Label, FillStyle, LineStyle);
    public LineStyle LineStyle { get; set; } = new() { Color = Colors.Black, Width = 1 };
    public FillStyle FillStyle { get; set; } = new() { Color = Colors.Red };
    public string Label { get; set; } = string.Empty;
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

    public void Render(RenderPack rp)
    {
        using SKPaint paint = new();

        PixelRect rect = Axes.GetPixelRect(CoordinateRect);
        FillStyle.ApplyToPaint(paint, rect);
        Drawing.FillRectangle(rp.Canvas, rect, paint);

        LineStyle.ApplyToPaint(paint);
        Drawing.DrawRectangle(rp.Canvas, rect, paint);
    }
}
