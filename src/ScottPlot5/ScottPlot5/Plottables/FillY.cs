using ScottPlot.Axis;

namespace ScottPlot.Plottables;

public class FillY : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get => Poly.Axes; set => Poly.Axes = value; }

    public IEnumerable<LegendItem> LegendItems => Enumerable.Empty<LegendItem>();

    private Polygon Poly { get; set; } = Polygon.Empty;

    public FillStyle FillStyle { get => Poly.FillStyle; }
    public LineStyle LineStyle { get => Poly.LineStyle; }
    public MarkerStyle MarkerStyle { get => Poly.MarkerStyle; }

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

        Poly = new Polygon(all);
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

        Poly = new Polygon(all);
    }

    public AxisLimits GetAxisLimits()
    {
        if (Poly is null)
            return AxisLimits.NoLimits;

        return Poly.GetAxisLimits();
    }

    public void Render(RenderPack rp)
    {
        Poly.Render(rp);
    }
}

