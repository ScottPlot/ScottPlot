namespace ScottPlot.Plottables;

public class Arrow : IPlottable, IHasArrow, IHasLegendText
{
    public Coordinates Base { get; set; } = Coordinates.Origin;
    public Coordinates Tip { get; set; } = Coordinates.Origin;

    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => [new LegendItem() { ArrowStyle = ArrowStyle, LabelText = LegendText }];
    public string LegendText { get; set; } = string.Empty;

    public ArrowStyle ArrowStyle { get; set; } = new() { LineWidth = 2, LineColor = Colors.Black };
    public Color ArrowLineColor { get => ArrowStyle.LineStyle.Color; set => ArrowStyle.LineStyle.Color = value; }
    public float ArrowLineWidth { get => ArrowStyle.LineStyle.Width; set => ArrowStyle.LineStyle.Width = value; }
    public Color ArrowFillColor { get => ArrowStyle.FillStyle.Color; set => ArrowStyle.FillStyle.Color = value; }
    public float ArrowMinimumLength { get => ArrowStyle.MinimumLength; set => ArrowStyle.MinimumLength = value; }
    public float ArrowMaximumLength { get => ArrowStyle.MaximumLength; set => ArrowStyle.MaximumLength = value; }
    public float ArrowOffset { get => ArrowStyle.Offset; set => ArrowStyle.Offset = value; }
    public ArrowAnchor ArrowAnchor { get => ArrowStyle.Anchor; set => ArrowStyle.Anchor = value; }
    public float ArrowWidth { get => ArrowStyle.ArrowWidth; set => ArrowStyle.ArrowWidth = value; }
    public float ArrowheadAxisLength { get => ArrowStyle.ArrowheadAxisLength; set => ArrowStyle.ArrowheadAxisLength = value; }
    public float ArrowheadLength { get => ArrowStyle.ArrowheadLength; set => ArrowStyle.ArrowheadLength = value; }
    public float ArrowheadWidth { get => ArrowStyle.ArrowheadWidth; set => ArrowStyle.ArrowheadWidth = value; }

    #region obsolete

    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }

    [Obsolete("use ArrowLineColor or ArrowFillColor", true)]
    public Color ArrowColor { get; set; }

    [Obsolete("use ArrowLineColor or ArrowFillColor", true)]
    public Color Color { get; set; }

    [Obsolete("use ArrowOffset", true)]
    public float Offset { get; set; }

    [Obsolete("use ArrowMinimumLength", true)]
    public float MinimumLength { get; set; }

    [Obsolete("use ArrowLineWidth", true)]
    public float LineWidth { get; set; }

    [Obsolete("use ArrowLineWidth, ArrowLineColor, or ArrowStyle.LineStyle", true)]
    public LineStyle LineStyle { get => ArrowStyle.LineStyle; set => ArrowStyle.LineStyle = value; }

    #endregion

    public AxisLimits GetAxisLimits() => new(
        Math.Min(Base.X, Tip.X),
        Math.Max(Base.X, Tip.X),
        Math.Min(Base.Y, Tip.Y),
        Math.Max(Base.Y, Tip.Y));

    public IArrowShape ArrowShape { get; set; } = new ArrowShapes.Single();

    public virtual void Render(RenderPack rp)
    {
        Pixel pxBase = Axes.GetPixel(Base);
        Pixel pxTip = Axes.GetPixel(Tip);
        PixelLine pxLine = new(pxBase, pxTip);

        if (ArrowOffset != 0)
            pxLine = pxLine.BackedUpBy(ArrowOffset);

        ArrowShape.Render(rp, pxLine, ArrowStyle);
    }
}
