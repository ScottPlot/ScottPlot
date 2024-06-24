using ScottPlot.AxisLimitManagers;
using ScottPlot.DataSources;
using System.Runtime.CompilerServices;

namespace ScottPlot.Plottables;

public class DataStreamer : IPlottable, IManagesAxisLimits, IHasLine, IHasLegendText
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = ScottPlot.Axes.Default;
    public IEnumerable<LegendItem> LegendItems => LegendItem.Single(LegendText, LineStyle);

    [Obsolete("use LegendText")]
    public string Label { get => LegendText; set => LegendText = value; }
    public string LegendText { get; set; } = string.Empty;

    public LineStyle LineStyle { get; set; } = new() { Width = 1 };
    public float LineWidth { get => LineStyle.Width; set => LineStyle.Width = value; }
    public LinePattern LinePattern { get => LineStyle.Pattern; set => LineStyle.Pattern = value; }
    public Color LineColor { get => LineStyle.Color; set => LineStyle.Color = value; }

    public Color Color { get => LineStyle.Color; set => LineStyle.Color = value; }

    public DataStreamerSource Data { get; set; }
    public int Count => Data.Length;
    public int CountTotal => Data.CountTotal;

    public double Period { get => Data.SamplePeriod; set => Data.SamplePeriod = value; }

    /// <summary>
    /// Returns true if data has been added since the last render
    /// </summary>
    public bool HasNewData => Data.CountTotal != Data.CountTotalOnLastRender;

    /// <summary>
    /// If enabled, axis limits will be adjusted automatically if new data runs off the screen.
    /// </summary>
    public bool ManageAxisLimits { get; set; } = true;

    [Obsolete("set Plot.Axes.ContinuouslyAutoscale", true)]
    public bool ContinuouslyAutoscale { get; set; } = false;

    /// <summary>
    /// Contains logic for automatically adjusting axis limits if new data runs off the screen.
    /// Only used if <see cref="ManageAxisLimits"/> is true.
    /// </summary>
    private IAxisLimitManager AxisManager { get; set; } = new FixedWidth();

    /// <summary>
    /// Used to obtain the current axis limits so <see cref="AxisManager"/> can adjust them if needed.
    /// </summary>
    private readonly Plot Plot;

    /// <summary>
    /// Logic for displaying the fixed-length Y values in <see cref="Data"/>
    /// </summary>
    public DataViews.IDataStreamerView Renderer { get; set; }

    public DataStreamer(Plot plot, double[] data)
    {
        Plot = plot;
        Data = new DataStreamerSource(data);
        Renderer = new DataViews.Wipe(this, true);
    }

    /// <summary>
    /// Shift in a new Y value
    /// </summary>
    public void Add(double value)
    {
        Data.Add(value);
    }

    public void Add(double[] ys)
    {
        if (ys is null)
            throw new ArgumentException($"{nameof(ys)} must not be null");

        for (int i = 0; i < ys.Length; i++)
        {
            Data.Add(ys[i]);
        }
    }

    /// <summary>
    /// Shift in a collection of new Y values
    /// </summary>
    public void AddRange(IEnumerable<double> values)
    {
        Data.AddRange(values);
    }

    /// <summary>
    /// Clear the buffer by setting all Y points to the given value
    /// </summary>
    public void Clear(double value = 0)
    {
        Data.Clear(value);
    }

    /// <summary>
    /// Display the data using a view where new data overlaps old data from left to right.
    /// </summary>
    public void ViewWipeRight(double blankFraction = 0)
    {
        Renderer = new DataViews.Wipe(this, true) { BlankFraction = blankFraction };
    }

    /// <summary>
    /// Display the data using a view where new data overlaps old data from right to left.
    /// </summary>
    public void ViewWipeLeft()
    {
        Renderer = new DataViews.Wipe(this, false);
    }

    /// <summary>
    /// Display the data using a view that continuously shifts data to the left, placing the newest data on the right.
    /// </summary>
    public void ViewScrollLeft()
    {
        Renderer = new DataViews.Scroll(this, true);
    }

    /// <summary>
    /// Display the data using a view that continuously shifts data to the right, placing the newest data on the left.
    /// </summary>
    public void ViewScrollRight()
    {
        Renderer = new DataViews.Scroll(this, false);
    }

    /*
    // TODO: slide axes
    /// <summary>
    /// Display the data using a view that continuously shifts data to the left, 
    /// placing the newest data on the right, and sliding the horizontal axis
    /// to track the latest data coming in.
    /// </summary>
    public void ViewSlideLeft()
    {
        Renderer = new DataViews.Scroll(this, true);
    }
    */

    /// <summary>
    /// Display the data using a custom rendering function
    /// </summary>
    public void ViewCustom(DataViews.IDataStreamerView view)
    {
        Renderer = view;
    }

    public AxisLimits GetAxisLimits()
    {
        return Data.GetAxisLimits();
    }

    public void UpdateAxisLimits(Plot plot)
    {
        if (Data.CountTotal == 0)
            return;

        AxisLimits limits = Plot.Axes.GetLimits(Axes);
        AxisLimits dataLimits = GetAxisLimits();
        AxisLimits newLimits = AxisManager.GetAxisLimits(limits, dataLimits);
        Plot.Axes.SetLimits(newLimits, Axes.XAxis, Axes.YAxis);
    }

    public virtual void Render(RenderPack rp)
    {
        Renderer.Render(rp);
        Data.CountTotalOnLastRender = Data.CountTotal;
    }
}
