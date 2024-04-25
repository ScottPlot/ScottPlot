namespace ScottPlot.Plottables;

public class FillY : IPlottable, IHasLine, IHasFill, IHasMarker, IHasLegendText
{
    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get; set; } = string.Empty;
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get => Poly.Axes; set => Poly.Axes = value; }

    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(LegendText, FillStyle, LineStyle);

    private Polygon Poly { get; set; } = Polygon.Empty;

    public FillStyle FillStyle { get => Poly.FillStyle; set => Poly.FillStyle = value; }
    public Color FillColor { get => FillStyle.Color; set => FillStyle.Color = value; }
    public Color FillHatchColor { get => FillStyle.HatchColor; set => FillStyle.HatchColor = value; }
    public IHatch? FillHatch { get => FillStyle.Hatch; set => FillStyle.Hatch = value; }

    public LineStyle LineStyle { get => Poly.LineStyle; set => Poly.LineStyle = value; }
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public MarkerStyle MarkerStyle { get => Poly.MarkerStyle; set => Poly.MarkerStyle = value; }
    public MarkerShape MarkerShape { get => MarkerStyle.Shape; set => MarkerStyle.Shape = value; }
    public float MarkerSize { get => MarkerStyle.Size; set => MarkerStyle.Size = value; }
    public Color MarkerFillColor { get => MarkerStyle.FillColor; set => MarkerStyle.FillColor = value; }
    public Color MarkerLineColor { get => MarkerStyle.LineColor; set => MarkerStyle.LineColor = value; }
    public Color MarkerColor { get => MarkerStyle.MarkerColor; set => MarkerStyle.MarkerColor = value; }
    public float MarkerLineWidth { get => MarkerStyle.LineWidth; set => MarkerStyle.LineWidth = value; }

    /// <summary>
    /// Creates an empty RangePlot plot, call SetDataSource() to set the coordinates.
    /// </summary>
    public FillY()
    {

    }

    /// <summary>
    /// Creates a RangePlot plot from two scatter plots.
    /// </summary>
    /// <param name="scatter1"></param>
    /// <param name="scatter2"></param>
    public FillY(Scatter scatter1, Scatter scatter2)
    {
        var data1 = scatter1.Data.GetScatterPoints();
        var data2 = scatter2.Data.GetScatterPoints();

        var data = data1.Concat(data2.Reverse()).ToArray();

        Poly = new Polygon(data);
    }

    public void SetDataSource(ICollection<(double X, double Top, double Bottom)> items)
    {
        Coordinates[] all = new Coordinates[items.Count * 2];

        int i = 0;
        foreach (var item in items)
        {
            Coordinates top = new Coordinates(item.X, item.Top);
            Coordinates bottom = new Coordinates(item.X, item.Bottom);

            all[i] = bottom;
            all[items.Count + (items.Count - 1 - i)] = top;

            i++;
        }

        Poly.UpdateCoordinates(all);
    }

    public void SetDataSource<T>(ICollection<T> items, Func<T, (double X, double Top, double Bottom)> coordinateSolver)
    {
        Coordinates[] all = new Coordinates[items.Count * 2];

        int i = 0;
        foreach (var item in items)
        {
            var coords = coordinateSolver(item);

            Coordinates top = new Coordinates(coords.X, coords.Top);
            Coordinates bottom = new Coordinates(coords.X, coords.Bottom);

            all[i] = bottom;
            all[items.Count + (items.Count - 1 - i)] = top;

            i++;
        }

        Poly.UpdateCoordinates(all);
    }

    public AxisLimits GetAxisLimits()
    {
        if (Poly is null)
            return AxisLimits.NoLimits;

        return Poly.GetAxisLimits();
    }

    public virtual void Render(RenderPack rp)
    {
        Poly.Render(rp);
    }
}

