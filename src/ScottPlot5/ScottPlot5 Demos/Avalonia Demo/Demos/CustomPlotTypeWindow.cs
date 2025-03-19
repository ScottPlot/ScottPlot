using Avalonia.Controls;
using Avalonia.Input;
using ScottPlot;
using ScottPlot.Plottables;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Avalonia_Demo.Demos;

public class CustomPlotTypeDemo : IDemo
{
    public string Title => "Custom Plot Type";
    public string Description => "How to create a custom plot type that implements IPlottable " +
        "to achieve full customization over how data is rendered on a plot.";

    public Window GetWindow()
    {
        return new CustomPlotTypeWindow();
    }

}

public class CustomPlotTypeWindow : SimpleDemoWindow
{
    public CustomPlotTypeWindow() : base("Custom Plot Type")
    {

    }

    protected override void StartDemo()
    {
        double[] xs = Generate.Consecutive(20);
        double[] ys = Generate.Sin(20);
        RainbowPlot rainbowPlot = new(xs, ys);

        AvaPlot.Plot.Add.Plottable(rainbowPlot);
    }
}

internal class RainbowPlot : IPlottable
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

