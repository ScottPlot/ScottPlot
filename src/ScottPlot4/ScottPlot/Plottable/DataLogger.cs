/* DataLogger: An experimental plot type for live, growing data
 * https://github.com/ScottPlot/ScottPlot/issues/2377
 * 
 * Ideas:
 * Support different view modes (show all, latest N samples, N-sample sweeps)
 * Events that can trigger renders upon suffecient new data
 * Methods to generate smarter axis limits for AxisAuto() that "snap" rather than fit tightly
 */
using ScottPlot.Renderable;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ScottPlot.Plottable;

public class DataLoggerFullMode : IDataLoggerMode
{
    public List<double> Ys { get; }
    public double SamplePeriod { get; }

    public DataLoggerFullMode(List<double> ys, double samplePeriod)
    {
        Ys = ys;
        SamplePeriod = samplePeriod;
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

    public AxisLimits GetAxisLimits()
    {
        return new AxisLimits(0, Ys.Count * SamplePeriod, Ys.Min(), Ys.Max());
    }

    public void Render(PlotDimensions dims, Graphics gfx, Pen pen)
    {
        PointF[] points = Enumerable.Range(0, Ys.Count)
            .Select(i => Coordinate.FromGeneric(i * SamplePeriod, Ys[i]))
            .Select(coord => coord.ToPixel(dims))
            .Select(px => new PointF(px.X, px.Y))
            .ToArray();

        gfx.DrawLines(pen, points);
    }
}

public class DataLoggerModeLatest : IDataLoggerMode
{
    public List<double> Ys { get; }
    public double SamplePeriod { get; }
    public int Size { get; }

    public DataLoggerModeLatest(List<double> ys, double samplePeriod, int size)
    {
        Ys = ys;
        SamplePeriod = samplePeriod;
        Size = size;
    }

    public bool UpdateAxisLimits(Plot plt)
    {
        bool changedX = UpdateAxisLimitsX(plt);
        bool changedY = UpdateAxisLimitsY(plt);
        return changedX || changedY;
    }

    private bool UpdateAxisLimitsX(Plot plt)
    {
        double xMax = Size * SamplePeriod;

        if (plt.GetAxisLimits().XMax < xMax)
        {
            plt.SetAxisLimitsX(0, xMax);
            return true;
        }

        return false;
    }

    private IEnumerable<double> GetYsInView()
    {
        int ysToSkip = Ys.Count() <= Size ? 0 : Ys.Count - Size;
        return Ys.Skip(ysToSkip);
    }

    private bool UpdateAxisLimitsY(Plot plt, double expandFrac = 1.25)
    {
        double yMin = GetYsInView().Min();
        double yMax = GetYsInView().Max();

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

    public AxisLimits GetAxisLimits()
    {
        return new AxisLimits(0, Size * SamplePeriod, GetYsInView().Min(), GetYsInView().Max());
    }

    public void Render(PlotDimensions dims, Graphics gfx, Pen pen)
    {
        double[] ysInView = GetYsInView().ToArray();

        PointF[] points = Enumerable.Range(0, ysInView.Count())
            .Select(i => Coordinate.FromGeneric(i * SamplePeriod, ysInView[i]))
            .Select(coord => coord.ToPixel(dims))
            .Select(px => new PointF(px.X, px.Y))
            .ToArray();

        gfx.DrawLines(pen, points);
    }
}

public interface IDataLoggerMode
{
    double SamplePeriod { get; }
    List<double> Ys { get; }
    AxisLimits GetAxisLimits();
    bool UpdateAxisLimits(Plot plt);
    void Render(PlotDimensions dims, Graphics gfx, Pen pen);
}

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

    private IDataLoggerMode ViewMode { get; set; }

    // TODO: multi-channel support
    // TODO: user-defined Xs?

    private List<double> Ys { get; } = new();

    private double SamplePeriod { get; }

    public DataLogger(double samplePeriod = 1)
    {
        SamplePeriod = samplePeriod;
        SetViewModeFull();
    }

    public void Add(double value)
    {
        Ys.Add(value);
    }

    public void AddRange(IEnumerable<double> values)
    {
        Ys.AddRange(values);
    }

    public void SetViewModeFull()
    {
        ViewMode = new DataLoggerFullMode(Ys, SamplePeriod);
    }

    public void SetViewModeSweeps(int size, int history)
    {
        //TODO
    }

    public void SetViewModeLatest(int size)
    {
        ViewMode = new DataLoggerModeLatest(Ys, SamplePeriod, size);
    }

    public bool UpdateAxisLimits(Plot plt) => ViewMode.UpdateAxisLimits(plt);

    public void ValidateData(bool deep = false) { }

    public AxisLimits GetAxisLimits() => ViewMode.GetAxisLimits();

    public LegendItem[] GetLegendItems() => LegendItem.None;

    public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
    {
        using var gfx = ScottPlot.Drawing.GDI.Graphics(bmp, dims, lowQuality);
        using var pen = ScottPlot.Drawing.GDI.Pen(Color, LineWidth, LineStyle.Solid); // TODO: rounded?

        ViewMode.Render(dims, gfx, pen);

        CountOnLastRender = Ys.Count;
    }
}
