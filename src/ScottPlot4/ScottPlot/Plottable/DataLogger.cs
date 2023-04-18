/* DataLogger: An experimental plot type for live, growing data
 * https://github.com/ScottPlot/ScottPlot/issues/2377
 * 
 * Ideas:
 * Support different view modes (show all, latest N samples, N-sample sweeps)
 * Events that can trigger renders upon suffecient new data
 * Methods to generate smarter axis limits for AxisAuto() that "snap" rather than fit tightly
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Plottable;

public class DataLogger : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public int XAxisIndex { get; set; } = 0;
    public int YAxisIndex { get; set; } = 0;
    public Color Color { get; set; } = Color.Blue;
    public float LineWidth { get; set; } = 1;
    public int Count => Ys.Count;
    private int CountOnLastRender { get; set; } = -1;
    public bool CountChangedSinceLastRender => Count != CountOnLastRender;

    // TODO: multi-channel support
    // TODO: user-defined Xs?

    private List<double> Ys { get; } = new();

    private double SamplePeriod { get; }

    public DataLogger(double samplePeriod = 1)
    {
        SamplePeriod = samplePeriod;
    }

    public void Add(double value)
    {
        Ys.Add(value);
    }

    public void AddRange(IEnumerable<double> values)
    {
        Ys.AddRange(values);
    }

    public void ValidateData(bool deep = false) { }

    public AxisLimits GetAxisLimits()
    {
        // TODO: intelligent min/max tracking
        return new AxisLimits(0, Ys.Count * SamplePeriod, Ys.Min(), Ys.Max());
    }

    public bool UpdateAxisLimits(Plot plt)
    {
        bool changedX = UpdateAxisLimitsX(plt);
        bool changedY = UpdateAxisLimitsY(plt);
        return changedX || changedY;
    }

    private bool UpdateAxisLimitsX(Plot plt, double expandFrac = 1.25)
    {
        double xMax = Ys.Count * SamplePeriod;

        if (plt.GetAxisLimits().XMax < xMax)
        {
            plt.SetAxisLimitsX(0, xMax * expandFrac);
            return true;
        }

        return false;
    }

    private bool UpdateAxisLimitsY(Plot plt, double expandFrac = 1.25)
    {
        double yMin = Ys.Min();
        double yMax = Ys.Max();

        var currentLimits = plt.GetAxisLimits();
        if (currentLimits.YMin > yMin || currentLimits.YMax < yMax)
        {
            double yCenter = (yMin + yMax) / 2;
            double ySpanHalf = (yMax - yMin) * expandFrac / 2;
            plt.SetAxisLimitsY(yCenter - ySpanHalf, yCenter + ySpanHalf);
            return true;
        }

        return false;
    }

    public LegendItem[] GetLegendItems() => LegendItem.None;

    public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
    {
        PointF[] points = GetPoints(dims);
        using var gfx = ScottPlot.Drawing.GDI.Graphics(bmp, dims, lowQuality);
        using var pen = ScottPlot.Drawing.GDI.Pen(Color, LineWidth, LineStyle.Solid); // TODO: rounded?
        gfx.DrawLines(pen, points);
        CountOnLastRender = points.Length;
    }

    /// <summary>
    /// Return a new array containing pixel locations for each point of the scatter plot
    /// </summary>
    private PointF[] GetPoints(PlotDimensions dims)
    {
        return Enumerable.Range(0, Ys.Count)
            .Select(i => Coordinate.FromGeneric(i * SamplePeriod, Ys[i]))
            .Select(coord => coord.ToPixel(dims))
            .Select(px => new PointF(px.X, px.Y))
            .ToArray();
    }
}
