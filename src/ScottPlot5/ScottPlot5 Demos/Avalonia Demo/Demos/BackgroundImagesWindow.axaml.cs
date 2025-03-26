using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia_Demo.ViewModels.Demos;
using ScottPlot;
using ScottPlot.Plottables;
using System;
using System.ComponentModel;
using System.Linq;

namespace Avalonia_Demo.Demos;

public class BackgroundImagesDemo: IDemo
{
    public string Title => "Background Images";
    public string Description => "Use a bitmap image for the background of the figure or data area";

    public Window GetWindow()
    {
        return new BackgroundImagesWindow();
    }
}

public partial class BackgroundImagesWindow : Window
{
    private BackgroundImagesViewModel TypedDataContext => (DataContext as BackgroundImagesViewModel) ?? throw new ArgumentException(nameof(DataContext));

    public BackgroundImagesWindow()
    {
        InitializeComponent();

        DataContext = new BackgroundImagesViewModel();
        TypedDataContext.PropertyChanged += HandleDataContextChanged;

        ResetPlot();
    }

    private void HandleDataContextChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TypedDataContext.ShowFigureBackground) || e.PropertyName == nameof(TypedDataContext.ShowDataBackground) || e.PropertyName == nameof(TypedDataContext.SelectedImagePosition)) {
            ResetPlot();
        }
    }

    private void ResetPlot()
    {
        AvaPlot.Reset();

        // add sample data
        var sig1 = AvaPlot.Plot.Add.Signal(Generate.Sin());
        var sig2 = AvaPlot.Plot.Add.Signal(Generate.Cos());
        sig1.LineWidth = 5;
        sig2.LineWidth = 5;
        AvaPlot.Plot.YLabel("Vertical Axis");
        AvaPlot.Plot.XLabel("Horizontal Axis");
        AvaPlot.Plot.Title("Plot with Image Background");

        // assign the bitmap image
        AvaPlot.Plot.FigureBackground.Image =TypedDataContext.ShowFigureBackground ? SampleImages.ScottPlotLogo() : null;
        AvaPlot.Plot.DataBackground.Image = TypedDataContext.ShowDataBackground? SampleImages.MonaLisa() : null;

        // set the scaling mode
        AvaPlot.Plot.FigureBackground.ImagePosition = TypedDataContext.SelectedImagePosition;
        AvaPlot.Plot.DataBackground.ImagePosition = TypedDataContext.SelectedImagePosition;

        // force a redraw
        AvaPlot.Refresh();
    }
}
