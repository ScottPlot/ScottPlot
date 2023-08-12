using ScottPlot.Drawing;
using System.Drawing;

namespace ScottPlot.Plottable;

public class RectanglePlot : IPlottable, IHasArea, IHasColor
{
    public bool IsVisible { get; set; } = true;
    public int XAxisIndex { get; set; } = 0;
    public int YAxisIndex { get; set; } = 0;
    public Color Color { get; set; } = Color.FromArgb(50, Color.Red);
    public Color BorderColor { get; set; } = Color.Red;
    public float BorderLineWidth { get; set; } = 1;
    public LineStyle BorderLineStyle { get; set; } = LineStyle.Solid;
    public Color HatchColor { get; set; } = Color.Magenta;
    public HatchStyle HatchStyle { get; set; } = HatchStyle.None;
    public string Label { get; set; } = string.Empty;

    public CoordinateRect Rectangle { get; set; }

    public RectanglePlot(CoordinateRect rect)
    {
        Rectangle = rect;
    }

    public void ValidateData(bool deep = false) { }

    public AxisLimits GetAxisLimits()
    {
        return new AxisLimits(Rectangle.XMin, Rectangle.XMax, Rectangle.YMin, Rectangle.YMax);
    }

    public LegendItem[] GetLegendItems()
    {
        return LegendItem.Single(this, Label, Color);
    }

    public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
    {
        using Graphics gfx = GDI.Graphics(bmp, dims, lowQuality);
        using Brush fillBrush = GDI.Brush(Color, HatchColor, HatchStyle);
        using Pen outlinePen = GDI.Pen(BorderColor, (float)BorderLineWidth, BorderLineStyle);

        RectangleF rect = dims.GetRect(Rectangle);
        gfx.FillRectangle(fillBrush, rect);
        gfx.DrawRectangle(outlinePen, rect);
    }
}
