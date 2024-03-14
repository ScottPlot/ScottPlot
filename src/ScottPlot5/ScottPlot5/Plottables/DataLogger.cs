
using ScottPlot.AxisLimitCalculators;
using ScottPlot.DataSources;

namespace ScottPlot.Plottables;

public class DataLogger : IPlottable, IManagesAxisLimits
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = ScottPlot.Axes.Default;
    public DataLoggerSource Data { get; set; } = new();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public bool ManageAxisLimits { get; set; } = true;
    public IAxisLimitManager AxisManager { get; set; } = new Full();

    public AxisLimits GetAxisLimits() => Data.GetAxisLimits();
    public LineStyle LineStyle = new();
    public Color Color { get => LineStyle.Color; set => LineStyle.Color = value; }

    /// <summary>
    /// Returns true if data has been added since the last render
    /// </summary>
    public bool HasNewData => Data.CountTotal != Data.CountOnLastRender;

    public void UpdateAxisLimits(Plot plot, bool force = false)
    {
        AxisLimits viewLimits = force ? AxisLimits.NoLimits : plot.Axes.GetLimits(Axes.XAxis, Axes.YAxis);
        AxisLimits dataLimits = GetAxisLimits();
        AxisLimits newLimits = AxisManager.GetAxisLimits(viewLimits, dataLimits);

        Debug.WriteLine("");
        Debug.WriteLine(dataLimits);
        Debug.WriteLine(newLimits);

        plot.Axes.SetLimits(newLimits, Axes.XAxis, Axes.YAxis);

        if (force)
            UpdateAxisLimits(plot);
    }

    public void Add(double y)
    {
        Data.Add(y);
    }

    public void Add(double x, double y)
    {
        Data.Add(x, y);
    }

    public void Add(Coordinates coordinates)
    {
        Data.Add(coordinates);
    }

    public void Add(IReadOnlyList<double> ys)
    {
        foreach (double y in ys)
        {
            Data.Add(y);
        }
    }

    public void Add(IReadOnlyList<Coordinates> coordinates)
    {
        foreach (Coordinates c in coordinates)
        {
            Data.Add(c);
        }
    }

    public void Add(IReadOnlyList<double> xs, IReadOnlyList<double> ys)
    {
        if (xs.Count != ys.Count)
        {
            throw new ArgumentException($"{nameof(xs)} and {nameof(ys)} must have equal size");
        }

        for (int i = 0; i < xs.Count; i++)
        {
            Data.Add(xs[i], ys[i]);
        }
    }

    public void Render(RenderPack rp)
    {
        IEnumerable<Pixel> points = Data.Coordinates.Select(Axes.GetPixel);

        using SKPaint paint = new();
        Drawing.DrawLines(rp.Canvas, paint, points, LineStyle);

        Data.CountOnLastRender = Data.CountTotal;
    }

    /// <summary>
    /// Automatically expand the axis as needed to ensure the full dataset is visible before each render.
    /// </summary>
    public void ViewFull()
    {
        ManageAxisLimits = true;
        AxisManager = new Full();
        Data.CountOnLastRender = -1;
    }

    /// <summary>
    /// Automatically adjust the axis limits to track the newest data as it comes in.
    /// The axis limits will appear to "jump" when new data runs off the screen.
    /// </summary>
    public void ViewJump(double width = 1000, double paddingFraction = .5)
    {
        ManageAxisLimits = true;
        AxisManager = new Slide()
        {
            Width = width,
            PaddingFractionX = paddingFraction,
        };
        Data.CountOnLastRender = -1;
    }

    /// <summary>
    /// Automatically adjust the axis limits to track the newest data as it comes in.
    /// The axis limits will appear to "slide" continuously as new data is added.
    /// </summary>
    public void ViewSlide(double width = 1000)
    {
        ManageAxisLimits = true;
        AxisManager = new Slide()
        {
            Width = width,
            PaddingFractionX = 0,
        };
        Data.CountOnLastRender = -1;
    }

}
