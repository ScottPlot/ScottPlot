using ScottPlot.Plottable.DataStreamerViews;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ScottPlot.Plottable;

#nullable enable

public class DataStreamer : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public int XAxisIndex { get; set; } = 0;
    public int YAxisIndex { get; set; } = 0;
    public string Label { get; set; } = string.Empty;
    public Color Color { get; set; } = Color.Blue;
    public float LineWidth { get; set; } = 1;
    public Plot Plot { get; private set; }
    public double[] Data { get; }
    public int DataIndex { get; private set; } = 0;
    public int Count => Data.Length;
    public int TotalPoints { get; private set; } = 0;
    public int TotalPointsOnLastRender { get; private set; } = -1;
    public bool RenderNeeded => TotalPointsOnLastRender != TotalPoints;
    public bool AutomaticallyExpandAxisLimits { get; set; } = true;
    private IDataStreamerView View { get; set; } = new Wipe(true);

    public double OffsetX { get; set; } = 0;
    public double OffsetY { get; set; } = 0;
    public double SamplePeriod { get; set; } = 1;
    public double SampleRate
    {
        get => 1.0 / SamplePeriod;
        set => SamplePeriod = 1.0 / value;
    }

    public double DataMin { get; private set; } = double.PositiveInfinity;
    public double DataMax { get; private set; } = double.NegativeInfinity;

    public DataStreamer(Plot plot, double[] data)
    {

        Plot = plot;
        Data = data;
    }

    public void Add(double value)
    {
        Data[DataIndex] = value;
        DataIndex += 1;

        if (DataIndex >= Data.Length)
            DataIndex = 0;

        DataMin = Math.Min(value, DataMin);
        DataMax = Math.Max(value, DataMax);

        TotalPoints += 1;
    }

    public void Add(IEnumerable<double> values)
    {
        foreach (double value in values)
        {
            Add(value);
        }
    }

    public void ResetMinMax()
    {
        DataMin = double.PositiveInfinity;
        DataMax = double.NegativeInfinity;
    }

    public void ExpandAxisLimits()
    {
        if (double.IsInfinity(DataMin) || double.IsInfinity(DataMax))
            return;

        AxisLimits limits = Plot.GetAxisLimits(XAxisIndex, YAxisIndex);

        if (DataMin < limits.YMin)
        {
            Plot.SetAxisLimits(yMin: DataMin);
        }

        if (DataMax > limits.YMax)
        {
            Plot.SetAxisLimits(yMax: DataMax);
        }
    }

    public void ValidateData(bool deep = false) => Validate.Pass();

    public AxisLimits GetAxisLimits()
    {
        double xMin = OffsetX;
        double xMax = OffsetX + Data.Length * SamplePeriod;

        double yMin = double.PositiveInfinity;
        double yMax = double.NegativeInfinity;

        for (int i = 0; i < Data.Length; i++)
        {
            yMin = Math.Min(yMin, Data[i]);
            yMax = Math.Max(yMax, Data[i]);
        }

        return new AxisLimits(xMin, xMax, yMin, yMax);
    }

    public LegendItem[] GetLegendItems() => LegendItem.Single(this, Label, Color);

    public void ViewWipeRight() => View = new Wipe(true);
    public void ViewWipeLeft() => View = new Wipe(false);
    public void ViewScrollLeft() => View = new Scroll(true);
    public void ViewScrollRight() => View = new Scroll(false);
    public void ViewCustom(IDataStreamerView view) => View = view;

    public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
    {
        if (AutomaticallyExpandAxisLimits)
            ExpandAxisLimits();

        using var gfx = ScottPlot.Drawing.GDI.Graphics(bmp, dims, lowQuality);
        using var pen = ScottPlot.Drawing.GDI.Pen(Color, LineWidth, LineStyle.Solid);
        View.Render(this, dims, gfx, pen);
    }
}
