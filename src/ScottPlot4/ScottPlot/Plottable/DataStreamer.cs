using ScottPlot.Plottable.DataStreamerViews;
using ScottPlot.Plottable.DataViewManagers;
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
    public int CountTotal { get; private set; } = 0;
    public int CountTotalOnLastRender { get; private set; } = -1;
    public bool RenderNeeded => CountTotalOnLastRender != CountTotal;
    public bool ManageAxisLimits { get; set; } = true;
    private IDataStreamerView View { get; set; } = new Wipe(true);
    private IDataViewManager ViewManager { get; set; } = new FixedWidth();

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

        CountTotal += 1;
    }

    public void AddRange(IEnumerable<double> values)
    {
        foreach (double value in values)
        {
            Add(value);
        }
    }

    public void Clear(double value = 0)
    {
        for (int i = 0; i < Data.Length; i++)
            Data[i] = 0;

        DataMin = value;
        DataMax = value;

        DataIndex = 0;
        CountTotal = 0;
    }

    public void ValidateData(bool deep = false) => Validate.Pass();

    public AxisLimits GetAxisLimits()
    {
        if (double.IsInfinity(DataMin) || double.IsInfinity(DataMax))
            return AxisLimits.NoLimits;

        double xMin = OffsetX;
        double xMax = OffsetX + Data.Length * SamplePeriod;
        return new AxisLimits(xMin, xMax, DataMin, DataMax);
    }

    public LegendItem[] GetLegendItems() => LegendItem.Single(this, Label, Color);

    public void ViewWipeRight() => View = new Wipe(true);
    public void ViewWipeLeft() => View = new Wipe(false);
    public void ViewScrollLeft() => View = new Scroll(true);
    public void ViewScrollRight() => View = new Scroll(false);
    public void ViewCustom(IDataStreamerView view) => View = view;

    public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
    {
        if (ManageAxisLimits)
        {
            AxisLimits limits = Plot.GetAxisLimits(XAxisIndex, YAxisIndex);
            AxisLimits dataLimits = GetAxisLimits();
            AxisLimits newLimits = ViewManager.GetAxisLimits(limits, dataLimits);
            Plot.SetAxisLimits(newLimits);
        }

        using var gfx = ScottPlot.Drawing.GDI.Graphics(bmp, dims, lowQuality);
        using var pen = ScottPlot.Drawing.GDI.Pen(Color, LineWidth, LineStyle.Solid);
        View.Render(this, dims, gfx, pen);
    }
}
