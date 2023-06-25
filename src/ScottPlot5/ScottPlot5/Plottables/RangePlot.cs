using ScottPlot.Axis;
using ScottPlot.Axis.StandardAxes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScottPlot.Plottables;

public class RangePlot : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = Axis.Axes.Default;

    public IEnumerable<LegendItem> LegendItems => Enumerable.Empty<LegendItem>();

    private Polygon poly = Polygon.Empty;

    public FillStyle? FillStyle { get => poly.FillStyle; set => poly.FillStyle = value; }
    public LineStyle LineStyle { get => poly.LineStyle; set => poly.LineStyle = value; }
    public MarkerStyle MarkerStyle { get => poly.MarkerStyle; set => poly.MarkerStyle = value; }

    /// <summary>
    /// Creates an empty RangePlot plot, call SetDataSource() to set the coordinates.
    /// </summary>
    public RangePlot()
    {

    }

    /// <summary>
    /// Creates a RangePlot plot from two scatter plots.
    /// </summary>
    /// <param name="scatter1"></param>
    /// <param name="scatter2"></param>
    public RangePlot(Scatter scatter1, Scatter scatter2)
    {
        var data1 = scatter1.Data.GetScatterPoints();
        var data2 = scatter2.Data.GetScatterPoints();

        var data = data1.Concat(data2.Reverse()).ToArray();

        poly = new Polygon(data);
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

        poly = new Polygon(all);
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

        poly = new Polygon(all);
    }

    public AxisLimits GetAxisLimits()
    {
        if (poly is null) return AxisLimits.NoLimits;
        return poly.GetAxisLimits();
    }

    public void Render(SKSurface surface)
    {
        if (poly != null)
        {
            poly.FillStyle = FillStyle;
            poly.LineStyle = LineStyle;
            poly.MarkerStyle = MarkerStyle;
            poly.Axes = Axes;
            poly.Render(surface);
        }
    }
}

