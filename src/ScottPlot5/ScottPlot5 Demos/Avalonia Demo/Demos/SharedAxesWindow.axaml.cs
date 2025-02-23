using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia_Demo.ViewModels.Demos;
using ScottPlot;
using System;
using System.ComponentModel;

namespace Avalonia_Demo.Demos;

public class SharedAxesDemo : IDemo
{
    public string Title => "Shared Axes";
    public string Description => "Link two controls together so they share an axis and have aligned layouts";

    public Window GetWindow()
    {
        return new SharedAxesWindow();
    }
}

public partial class SharedAxesWindow : Window
{
    private SharedAxesViewModel TypedDataContext => (DataContext as SharedAxesViewModel) ?? throw new ArgumentException(nameof(DataContext));

    public SharedAxesWindow()
    {
        InitializeComponent();

        DataContext = new SharedAxesViewModel();
        TypedDataContext.PropertyChanged += HandleDataContextChanged;

        // add data to both plots
        AvaPlot1.Plot.Add.Signal(Generate.Sin());
        AvaPlot2.Plot.Add.Signal(Generate.Cos());

        // use fixed layout so plots remain perfectly aligned
        PixelPadding padding = new(50, 20, 30, 5);
        AvaPlot1.Plot.Layout.Fixed(padding);
        AvaPlot2.Plot.Layout.Fixed(padding);

        UpdateLinkedPlots();
    }

    private void HandleDataContextChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(TypedDataContext.ShareX) || e.PropertyName == nameof(TypedDataContext.ShareY)) {
            UpdateLinkedPlots();
        }
    }

    private void UpdateLinkedPlots()
    {
        // clear old link rules
        AvaPlot1.Plot.Axes.UnlinkAll();
        AvaPlot2.Plot.Axes.UnlinkAll();

        // add new link rules based on what is checked
        AvaPlot1.Plot.Axes.Link(AvaPlot2, x: TypedDataContext.ShareX, y: TypedDataContext.ShareY);
        AvaPlot2.Plot.Axes.Link(AvaPlot1, x: TypedDataContext.ShareX, y: TypedDataContext.ShareY);

        // reset axis limits and refresh both plots
        AvaPlot1.Plot.Axes.AutoScale();
        AvaPlot2.Plot.Axes.AutoScale();
        AvaPlot1.Refresh();
        AvaPlot2.Refresh();
    }
}
