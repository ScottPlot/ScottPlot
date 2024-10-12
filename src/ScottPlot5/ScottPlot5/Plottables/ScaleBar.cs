namespace ScottPlot.Plottables;

public class ScaleBar : IPlottable, IHasLine
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public AxisLimits GetAxisLimits() => AxisLimits.NoLimits;

    /// <summary>
    /// Distance of the corner of the scalebar from the corner of the data area
    /// </summary>
    public PixelPadding EdgePadding { get; set; } = new(10);

    /// <summary>
    /// Padding to add between labels and scalebar lines
    /// </summary>
    public PixelPadding LabelPadding { get; set; } = new(3);

    public double Width { get; set; } = 1;
    public double Height { get; set; } = 1;

    public LineStyle LineStyle { get; set; } = new()
    {
        IsVisible = true,
        Width = 1,
        Color = Colors.Black,
    };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public LabelStyle XLabelStyle { get; set; } = new() { Alignment = Alignment.UpperCenter };
    public LabelStyle YLabelStyle { get; set; } = new() { Alignment = Alignment.MiddleLeft };
    public string XLabel { get; set; } = string.Empty;
    public string YLabel { get; set; } = string.Empty;

    public void Render(RenderPack rp)
    {
        Pixel corner = Axes.GetPixel(new(Axes.XAxis.Max, Axes.YAxis.Min));

        // offset for padding
        corner = corner.WithOffset(-EdgePadding.Left, -EdgePadding.Bottom);

        // offset for labels
        PixelSize xLabelSize = XLabelStyle.Measure(XLabel).Size;
        PixelSize yLabelSize = YLabelStyle.Measure(YLabel).Size;
        corner = corner.WithOffset(-yLabelSize.Width, -xLabelSize.Height);

        double pxPerUnitX = rp.DataRect.Width / Axes.XAxis.Width;
        double pxPerUnitY = rp.DataRect.Height / Axes.YAxis.Height;
        double pxWidth = pxPerUnitX * Width;
        double pxHeight = pxPerUnitY * Height;
        PixelLine hLine = new(corner, new(corner.X - pxWidth, corner.Y));
        PixelLine vLine = new(corner, new(corner.X, corner.Y - pxHeight));

        using SKPaint paint = new();

        // TODO: use a path instead of two distinct lines to prevent artifacts at the corner

        if (Width > 0)
        {
            Drawing.DrawLine(rp.Canvas, paint, hLine, LineStyle);
            Pixel xCenter = new((hLine.X1 + hLine.X2) / 2, hLine.Y1 + LabelPadding.Top);
            XLabelStyle.Render(rp.Canvas, xCenter, paint, XLabel);
        }

        if (Height > 0)
        {
            Drawing.DrawLine(rp.Canvas, paint, vLine, LineStyle);
            Pixel yCenter = new(vLine.X1 + LabelPadding.Right, (vLine.Y1 + vLine.Y2) / 2);
            YLabelStyle.Render(rp.Canvas, yCenter, paint, YLabel);
        }
    }
}
