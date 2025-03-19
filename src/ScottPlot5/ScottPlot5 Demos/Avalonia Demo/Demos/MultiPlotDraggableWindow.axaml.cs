using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ScottPlot;
using System.Collections.Generic;
using System.Linq;

namespace Avalonia_Demo.Demos;

public class MultiPlotDraggableDemo : IDemo
{
    public string Title => "Multiplot with Draggable Subplots";
    public string Description => "Subplots may be placed very close together by setting their padding to zero. " +
        "This example uses an advanced Layout system to enable mouse drag resizing of subplots.";

    public Window GetWindow()
    {
        return new MultiPlotDraggableWindow();
    }
}

public partial class MultiPlotDraggableWindow : Window
{
    private int? DividerBeingDragged = null;
    private readonly ScottPlot.MultiplotLayouts.DraggableRows CustomLayout = new();

    public MultiPlotDraggableWindow()
    {
        InitializeComponent();

        // setup a multiplot with 3 subplots
        AvaPlot.Multiplot.AddPlots(3);

        // set padding so there is no space between the middle and adjacent plots
        AvaPlot.Multiplot.CollapseVertically();

        // add sample price data to the first plot
        Plot pricePlot = AvaPlot.Multiplot.GetPlot(0);
        pricePlot.Axes.Right.Label.Text = "Price";
        List<OHLC> ohlcs = Generate.RandomOHLCs(50);
        var candlestick = pricePlot.Add.Candlestick(ohlcs);
        candlestick.Axes.YAxis = pricePlot.Axes.Right;
        candlestick.Sequential = true;

        // add sample RSI data to the second plot
        Plot rsiPlot = AvaPlot.Multiplot.GetPlot(1);
        rsiPlot.Axes.Right.Label.Text = "RSI";
        double[] rsiValues = Generate.RandomWalk(ohlcs.Count);
        var rsiSig = rsiPlot.Add.Signal(rsiValues);
        rsiSig.Axes.YAxis = rsiPlot.Axes.Right;
        rsiSig.LineWidth = 2;

        // add sample RSI data to the second plot
        Plot volumePlot = AvaPlot.Multiplot.GetPlot(2);
        volumePlot.Axes.Right.Label.Text = "Volume";
        double[] volumes = Generate.RandomSample(50, 10, 90);
        var bars = volumePlot.Add.Bars(volumes);
        bars.Axes.YAxis = volumePlot.Axes.Right;
        volumePlot.Axes.Margins(bottom: 0);

        // use the same size for all right axes to ensure alignment regardless of tick label length
        foreach (Plot plot in AvaPlot.Multiplot.GetPlots())
        {
            plot.Axes.Left.LockSize(10);
            plot.Axes.Right.LockSize(80);
        }

        // update grids to use ticks from the bottom plot
        pricePlot.Grid.XAxis = volumePlot.Axes.Bottom;
        rsiPlot.Grid.XAxis = volumePlot.Axes.Bottom;

        // update grids to use ticks from the right axis
        pricePlot.Grid.YAxis = pricePlot.Axes.Right;
        rsiPlot.Grid.YAxis = rsiPlot.Axes.Right;
        volumePlot.Grid.YAxis = volumePlot.Axes.Right;

        // link horizontal axes across all plots
        AvaPlot.Multiplot.SharedAxes.ShareX([pricePlot, rsiPlot, volumePlot]);

        // use custom logic to tell the multiplot how large to make each plot
        AvaPlot.Multiplot.Layout = CustomLayout;

        // set the initial heights for each plot
        CustomLayout.SetHeights([600, 100, 100]);
    }

    private void OnAddRowClick(object? sender, RoutedEventArgs e)
    {
        Plot plot = AvaPlot.Multiplot.AddPlot();
        plot.Axes.Left.LockSize(10);
        plot.Axes.Right.LockSize(80);
        AvaPlot.Multiplot.CollapseVertically();
        AvaPlot.Refresh();
    }

    private void OnDeleteRowClick(object? sender, RoutedEventArgs e)
    {
        if (AvaPlot.Multiplot.Subplots.Count < 2)
            return;

        Plot plotToRemove = AvaPlot.Multiplot.Subplots.GetPlots().Last();
        AvaPlot.Multiplot.RemovePlot(plotToRemove);

        // revert the collapse of the lower edge of the new bottom plot and use the original tick generator
        Plot newBottomPlot = AvaPlot.Multiplot.Subplots.GetPlots().Last();
        newBottomPlot.Axes.Bottom.ResetSize();
        newBottomPlot.Axes.Bottom.TickGenerator = plotToRemove.Axes.Bottom.TickGenerator;

        AvaPlot.Refresh();
    }

    private void OnMouseDown(object? sender, PointerPressedEventArgs e)
    {
        var pos = e.GetPosition(AvaPlot);

        DividerBeingDragged = CustomLayout.GetDivider((float)pos.Y);
        AvaPlot.UserInputProcessor.IsEnabled = DividerBeingDragged is null;
    }

    private void OnMouseUp(object? sender, PointerReleasedEventArgs e)
    {
        if (DividerBeingDragged is not null)
        {
            DividerBeingDragged = null;
            AvaPlot.UserInputProcessor.IsEnabled = true;
        }
    }

    private void OnMouseMove(object? sender, PointerEventArgs e)
    {
        var pos = e.GetPosition(AvaPlot);
        if (DividerBeingDragged is not null)
        {
            CustomLayout.SetDivider(DividerBeingDragged.Value, (float)pos.Y);
            AvaPlot.Refresh();
        }

        Cursor = CustomLayout.GetDivider((float)pos.Y) is not null ? new(StandardCursorType.SizeNorthSouth) : new(StandardCursorType.Arrow);
    }
}
