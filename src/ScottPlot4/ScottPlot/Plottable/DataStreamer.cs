using ScottPlot.Plottable.AxisManagers;
using ScottPlot.Plottable.DataStreamerViews;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace ScottPlot.Plottable;

#nullable enable

public class DataStreamer : IPlottable, IHasLine, IHasColor
{
    public bool IsVisible { get; set; } = true;
    public int XAxisIndex { get; set; } = 0;
    public int YAxisIndex { get; set; } = 0;
    public string Label { get; set; } = string.Empty;
    public Color Color { get => LineColor; set { LineColor = value; } }
    public Color LineColor { get; set; } = Color.Blue;
    public double LineWidth { get; set; } = 1;
    public LineStyle LineStyle { get; set; } = LineStyle.Solid;

    /// <summary>
    /// Fixed-length array used as a circular buffer to shift data in at the position defined by <see cref="NextIndex"/>.
    /// Values in this array should not be modified externally if <see cref="ManageAxisLimits"/> is enabled.
    /// </summary>
    public double[] Data { get; }

    /// <summary>
    /// Index in <see cref="Data"/> where the next point will be added
    /// </summary>
    public int NextIndex { get; private set; } = 0;

    /// <summary>
    /// The fied number of visible data points to display
    /// </summary>
    public int Count => Data.Length;

    /// <summary>
    /// The total number of data points added (even though only the latest <see cref="Count"/> are visible)
    /// </summary>
    public int CountTotal { get; private set; } = 0;

    /// <summary>
    /// Total of data points added the last time this plottable was rendered.
    /// This can be compared with <see cref="CountTotal"/> to determine if a new render is required.
    /// </summary>
    public int CountTotalOnLastRender { get; private set; } = -1;

    /// <summary>
    /// If enabled, axis limits will be adjusted automatically if new data runs off the screen.
    /// </summary>
    public bool ManageAxisLimits { get; set; } = true;

    /// <summary>
    /// Contains logic for automatically adjusting axis limits if new data runs off the screen.
    /// Only used if <see cref="ManageAxisLimits"/> is true.
    /// </summary>
    private IAxisManager AxisManager { get; set; } = new FixedWidth();

    /// <summary>
    /// Used to obtain the current axis limits so <see cref="AxisManager"/> can adjust them if needed.
    /// </summary>
    private readonly Plot Plot;

    /// <summary>
    /// Logic for displaying the fixed-length Y values in <see cref="Data"/>
    /// </summary>
    public IDataStreamerView Renderer { get; set; }

    public double OffsetX { get; set; } = 0;
    public double OffsetY { get; set; } = 0;
    public double SamplePeriod { get; set; } = 1;
    public double SampleRate
    {
        get => 1.0 / SamplePeriod;
        set => SamplePeriod = 1.0 / value;
    }

    /// <summary>
    /// Minimum value of all known data (not just the data in view)
    /// </summary>
    public double DataMin { get; private set; } = double.PositiveInfinity;

    /// <summary>
    /// Maximum value of all known data (not just the data in view)
    /// </summary>
    public double DataMax { get; private set; } = double.NegativeInfinity;

    public DataStreamer(Plot plot, double[] data)
    {
        Plot = plot;
        Data = data;
        Renderer = new Wipe(this, true);
    }

    /// <summary>
    /// Shift in a new Y value
    /// </summary>
    public void Add(double value)
    {
        Data[NextIndex] = value;
        NextIndex += 1;

        if (NextIndex >= Data.Length)
            NextIndex = 0;

        DataMin = Math.Min(value, DataMin);
        DataMax = Math.Max(value, DataMax);

        CountTotal += 1;
    }

    /// <summary>
    /// Shift in a collection of new Y values
    /// </summary>
    public void AddRange(IEnumerable<double> values)
    {
        foreach (double value in values)
        {
            Add(value);
        }
    }

    /// <summary>
    /// Clear the buffer by setting all Y points to the given value
    /// </summary>
    public void Clear(double value = 0)
    {
        for (int i = 0; i < Data.Length; i++)
            Data[i] = 0;

        DataMin = value;
        DataMax = value;

        NextIndex = 0;
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

    public LegendItem[] GetLegendItems()
    {
        var singleItem = new LegendItem(this)
        {
            label = Label,
            color = LineColor,
            lineStyle = LineStyle,
            lineWidth = LineWidth,
        };
        return LegendItem.Single(singleItem);
    }

    /// <summary>
    /// Display the data using a view where new data overlapps old data from left to right.
    /// </summary>
    public void ViewWipeRight()
    {
        Renderer = new Wipe(this, true);
    }

    /// <summary>
    /// Display the data using a view where new data overlapps old data from right to left.
    /// </summary>
    public void ViewWipeLeft()
    {
        Renderer = new Wipe(this, false);
    }

    /// <summary>
    /// Display the data using a view that continuously shifts data to the left, placing the newest data on the right.
    /// </summary>
    public void ViewScrollLeft()
    {
        Renderer = new Scroll(this, true);
    }

    /// <summary>
    /// Display the data using a view that continuously shifts data to the right, placing the newest data on the left.
    /// </summary>
    public void ViewScrollRight()
    {
        Renderer = new Scroll(this, false);
    }

    /// <summary>
    /// Display the data using a custom rendering function
    /// </summary>
    public void ViewCustom(IDataStreamerView view)
    {
        Renderer = view;
    }

    public void Render(PlotDimensions dims, Bitmap bmp, bool lowQuality = false)
    {
        if (ManageAxisLimits)
        {
            AxisLimits limits = Plot.GetAxisLimits(XAxisIndex, YAxisIndex);
            AxisLimits dataLimits = GetAxisLimits();
            AxisLimits newLimits = AxisManager.GetAxisLimits(limits, dataLimits);
            Plot.SetAxisLimits(newLimits, XAxisIndex, YAxisIndex);
        }

        Renderer.Render(dims, bmp, lowQuality);
    }
}
