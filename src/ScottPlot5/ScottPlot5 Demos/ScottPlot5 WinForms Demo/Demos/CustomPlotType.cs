using ScottPlot;
using SkiaSharp;

namespace WinForms_Demo.Demos;

public partial class CustomPlotType : Form, IDemoWindow
{
    public string Title => "Custom Plot Type";
    public string Description => "How to create a custom plot type that implements IPlottable " +
        "to achieve full customization over how data is rendered on a plot.";

    public CustomPlotType()
    {
        InitializeComponent();

        double[] xs = ScottPlot.Generate.Consecutive(20);
        double[] ys = ScottPlot.Generate.Sin(20);
        RainbowPlot rainbowPlot = new(xs, ys);

        formsPlot1.Plot.Add.Plottable(rainbowPlot);
    }
}

class RainbowPlot : IPlottable
{
    // data and customization options
    double[] Xs { get; }
    double[] Ys { get; }
    public float Radius { get; set; } = 10;
    IColormap Colormap { get; set; } = new ScottPlot.Colormaps.Turbo();

    // items required by IPlottable
    public bool IsVisible { get; set; } = true;
    public IAxes Axes { get; set; } = new Axes();
    public IEnumerable<LegendItem> LegendItems => LegendItem.None;
    public AxisLimits GetAxisLimits() => new(Xs.Min(), Xs.Max(), Ys.Min(), Ys.Max());

    public RainbowPlot(double[] xs, double[] ys) { Xs = xs; Ys = ys; }

    public void Render(RenderPack rp)
    {
        FillStyle FillStyle = new();
        using SKPaint paint = new();
        for (int i = 0; i < Xs.Length; i++)
        {
            Coordinates centerCoordinates = new(Xs[i], Ys[i]);
            Pixel centerPixel = Axes.GetPixel(centerCoordinates);
            FillStyle.Color = Colormap.GetColor(i / (Xs.Length - 1.0));
            ScottPlot.Drawing.DrawCircle(rp.Canvas, centerPixel, Radius, FillStyle, paint);
        }
    }
}
