using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia_Demo.ViewModels.Demos;
using ScottPlot;
using System;
using System.ComponentModel;

namespace Avalonia_Demo.Demos;

public class ScatterVsSignalPlotDemo : IDemo
{
    public string Title => "Scatter Plot, Signal Plot, and SignalConst";
    public string Description => "Demonstrates performance of Scatter plots, " +
        "Signal Plots, and SignalConst on large datasets.";

    public Window GetWindow()
    {
        return new ScatterVsSignalPlotWindow();
    }
}

public partial class ScatterVsSignalPlotWindow : Window
{
    private ScatterVsSignalViewModel TypedDataContext => (DataContext as ScatterVsSignalViewModel) ?? throw new ArgumentException(nameof(DataContext));

    public ScatterVsSignalPlotWindow()
    {
        InitializeComponent();

        DataContext = new ScatterVsSignalViewModel();
        TypedDataContext.PropertyChanged += HandleDataContextChanged;

        TypedDataContext.PlotType = PlotType.Signal;

        Replot();
    }

    private void HandleDataContextChanged(object? sender, PropertyChangedEventArgs e)
    {
        switch (e.PropertyName)
        {
            case nameof(TypedDataContext.PlotType): // fallthrough
            case nameof(TypedDataContext.NumberOfPoints):
                Replot();
                break;
        }
    }

    private void Replot()
    {
        AvaPlot.Plot.Clear();

        (double[] xs, double[] ys) = GetData(TypedDataContext.NumberOfPoints);

        switch (TypedDataContext.PlotType)
        {
            case PlotType.Scatter:
                AvaPlot.Plot.Add.ScatterLine(xs, ys);
                AvaPlot.Plot.Title($"Scatter Plot with {ys.Length:N0} Points");
                break;
            case PlotType.Signal:
                AvaPlot.Plot.Add.Signal(ys);
                AvaPlot.Plot.Title($"Signal Plot with {ys.Length:N0} Points");
                break;
            case PlotType.SignalConst:
                AvaPlot.Plot.Add.SignalConst(ys);
                AvaPlot.Plot.Title($"SignalConst with {ys.Length:N0} Points");
                break;
            default:
                throw new NotImplementedException(TypedDataContext.PlotType.ToString());
        }

        AvaPlot.Plot.Axes.AutoScale();
        AvaPlot.Refresh();
    }

    private (double[] xs, double[] ys) GetData(int count = 1_000_000)
    {
        double[] xs = Generate.Consecutive(count);
        double[] ys = Generate.Sin(count);
        Generate.AddNoiseInPlace(ys);
        return (xs, ys);
    }
}
