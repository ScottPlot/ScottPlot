
using ScottPlot.AxisManagers;
using ScottPlot.DataSources;

namespace ScottPlot.Plottables;

public class DataLogger : IPlottable
{
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = ScottPlot.Axes.Default;
    public DataLoggerSource DataSource { get; set; } = new();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;

    public bool ManageAxisLimits { get; set; } = true;
    public IAxisManager AxisManager { get; set; } = new Full();

    public AxisLimits GetAxisLimits() => DataSource.GetAxisLimits();
    public LineStyle LineStyle = new();
    public Color Color { get => LineStyle.Color; set => LineStyle.Color = value; }

    /// <summary>
    /// Returns true if data has been added since the last render
    /// </summary>
    public bool HasNewData => DataSource.CountTotal != DataSource.CountOnLastRender;

    private void UpdateAxisLimits(Plot plot, bool force = false)
    {
        AxisLimits viewLimits = force ? AxisLimits.NoLimits : plot.Axes.GetLimits(Axes);
        AxisLimits dataLimits = GetAxisLimits();
        AxisLimits newLimits = AxisManager.GetAxisLimits(viewLimits, dataLimits);
        plot.Axes.SetLimits(newLimits);

        if (force)
            UpdateAxisLimits(plot);
    }

    public void Add(double y)
    {
        DataSource.Add(y);
    }

    public void Add(IEnumerable<double> ys)
    {
        foreach (double y in ys)
        {
            DataSource.Add(y);
        }
    }

    public void Render(RenderPack rp)
    {
        if (ManageAxisLimits)
        {
            UpdateAxisLimits(rp.Plot);
        }

        Pixel[] points = DataSource.Coordinates.Select(Axes.GetPixel).ToArray();

        if (points.Length > 1)
        {
            using SKPaint paint = new();
            Drawing.DrawLines(rp.Canvas, paint, points, LineStyle);
        }

        DataSource.CountOnLastRender = DataSource.CountTotal;
    }

    /// <summary>
    /// Automatically expand the axis as needed to ensure the full dataset is visible before each render.
    /// </summary>
    public void ViewFull()
    {
        ManageAxisLimits = true;
        AxisManager = new Full();
        DataSource.CountOnLastRender = -1;
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
        DataSource.CountOnLastRender = -1;
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
        DataSource.CountOnLastRender = -1;
    }

}
