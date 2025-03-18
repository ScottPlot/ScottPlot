using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia_Demo.ViewModels.Demos;
using ScottPlot;
using ScottPlot.Plottables;
using ScottPlotCookbook.Recipes.PlotTypes;
using System;

namespace Avalonia_Demo.Demos;

public abstract partial class ShowValueUnderMouseWindow : Window
{
    private ShowValueUnderMouseViewModel TypedDataContext => (DataContext as ShowValueUnderMouseViewModel) ?? throw new ArgumentException(nameof(DataContext));

    protected abstract IGetNearest Plottable { get; }
    private readonly ScottPlot.Plottables.Crosshair Crosshair;

    public ShowValueUnderMouseWindow()
    {
        InitializeComponent();

        DataContext = new ShowValueUnderMouseViewModel();

        double[] xs = Generate.RandomSample(100);
        double[] ys = Generate.RandomSample(100);

        Crosshair = AvaPlot.Plot.Add.Crosshair(0, 0);
        Crosshair.IsVisible = false;
        Crosshair.MarkerShape = MarkerShape.OpenCircle;
        Crosshair.MarkerSize = 15;
    }

    private void OnMouseMove(object? sender, PointerEventArgs e)
    {
        // determine where the mouse is and get the nearest point
        var pos = e.GetPosition(AvaPlot);
        Pixel mousePixel = new(pos.X, pos.Y);
        Coordinates mouseLocation = AvaPlot.Plot.GetCoordinates(mousePixel);

        // Don't consider matches beyond this point
        var maxDistance = 15;
        DataPoint nearest = TypedDataContext.ClosestValueMode == ClosestValueMode.NearestXY
            ? Plottable.GetNearest(mouseLocation, AvaPlot.Plot.LastRender, maxDistance)
            : Plottable.GetNearestX(mouseLocation, AvaPlot.Plot.LastRender, maxDistance);

        // place the crosshair over the highlighted point
        if (nearest.IsReal)
        {
            Crosshair.IsVisible = true;
            Crosshair.Position = nearest.Coordinates;
            AvaPlot.Refresh();
            Title = $"Selected Index={nearest.Index}, X={nearest.X:0.##}, Y={nearest.Y:0.##}";
        }

        // hide the crosshair when no point is selected
        if (!nearest.IsReal && Crosshair.IsVisible)
        {
            Crosshair.IsVisible = false;
            AvaPlot.Refresh();
            Title = $"No point selected";
        }
    }
}
