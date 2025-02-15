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

    public virtual void Render(RenderPack rp)
    {
        Pixel corner = Axes.GetPixel(new(Axes.XAxis.Max, Axes.YAxis.Min));

        // offset for padding
        corner = corner.WithOffset(-EdgePadding.Left, -EdgePadding.Bottom);

        // offset for labels
        PixelSize xLabelSize = XLabelStyle.Measure(XLabel).Size;
        PixelSize yLabelSize = YLabelStyle.Measure(YLabel).Size;
        corner = corner.WithOffset(-yLabelSize.Width, -xLabelSize.Height);

        using SKPaint paint = new();
        SKPath path = new();
        path.MoveTo(corner.ToSKPoint());

        if (Width > 0)
        {
            double pxPerUnitX = rp.DataRect.Width / Axes.XAxis.Width;
            double pxWidth = pxPerUnitX * Width;

            path.RLineTo(new SKPoint((float)-pxWidth, 0));
            path.RLineTo(new SKPoint((float)+pxWidth, 0));

            Pixel xCenter = new(corner.X - pxWidth / 2, corner.Y + LabelPadding.Top);
            XLabelStyle.Render(rp.Canvas, xCenter, paint, XLabel);
        }

        if (Height > 0)
        {
            double pxPerUnitY = rp.DataRect.Height / Axes.YAxis.Height;
            double pxHeight = pxPerUnitY * Height;

            path.RLineTo(new SKPoint(0, (float)-pxHeight));
            path.RLineTo(new SKPoint(0, (float)+pxHeight));

            Pixel yCenter = new(corner.X + LabelPadding.Right, corner.Y - pxHeight / 2);
            YLabelStyle.Render(rp.Canvas, yCenter, paint, YLabel);
        }

        Drawing.DrawPath(rp.Canvas, paint, path, LineStyle);
    }
}
