namespace ScottPlot.Plottables;

public class LollipopPlot : IPlottable, IHasLine, IHasMarker
{
    public bool IsVisible { get; set; } = true;

    public IAxes Axes { get; set; } = new Axes();

    public string LegendText { get; set; } = string.Empty;

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(LegendText, LineStyle, MarkerStyle);

    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public MarkerStyle MarkerStyle { get; set; } = new() { Size = 0 };
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }
    public Color MarkerLineColor { get => MarkerStyle.LineColor; set => MarkerStyle.LineColor = value; }
    public Color MarkerColor { get => MarkerStyle.MarkerColor; set => MarkerStyle.MarkerColor = value; }
    public float MarkerLineWidth { get => MarkerStyle.LineWidth; set => MarkerStyle.LineWidth = value; }

    public Color Color
    {
        get => LineStyle.Color;
        set
        {
            LineStyle.Color = value;
            MarkerStyle.FillColor = value;
        }
    }

    public double[] Data { get; }

    public double[] Positions { get; }

    public Orientation Orientation { get; set; } = Orientation.Vertical;

    public LollipopPlot(IEnumerable<double> data, IEnumerable<double>? positions = null)
    {
        Data = data.ToArray();
        Positions =
            (positions ?? Enumerable.Range(0, Data.Length).Select(i => (double)i))
            .ToArray();
    }

    public AxisLimits GetAxisLimits()
    {
        return Orientation == Orientation.Vertical
            ? new AxisLimits(Positions.Min(), Positions.Max(), Data.Max(), 0)
            : new AxisLimits(0, Data.Max(), Positions.Max(), Positions.Min());
    }

    public virtual void Render(RenderPack rp)
    {
        using SKPaint paint = new();
        for (int i = 0; i < Data.Length; i++)
        {
            Coordinates Start;
            Coordinates End;
            if (Orientation == Orientation.Vertical)
            {
                Start = new(Positions[i], 0);
                End = new(Positions[i], Data[i]);
            }
            else
            {
                Start = new(0, Positions[i]);
                End = new(Data[i], Positions[i]);
            }

            PixelLine pxLine = Axes.GetPixelLine(new(Start, End));

            Drawing.DrawLine(rp.Canvas, paint, pxLine, LineStyle);
            Drawing.DrawMarker(rp.Canvas, paint, Axes.GetPixel(End), MarkerStyle);
        }
    }
}
