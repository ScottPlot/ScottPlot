using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using NUnit.Framework;
using ScottPlot;
using ScottPlot.MultiplotLayouts;

namespace Avalonia_Demo.Demos;

public class MultiPlotWithAdvancedLayoutDemo : IDemo
{
    public string Title => "Multiplot with Advanced Layout";
    public string Description => "Custom multi-plot layouts may be achieved " +
        "by assigning fractional rectangle dimensions to each subplot";

    public Window GetWindow()
    {
        return new MultiPlotWithAdvancedLayoutWindow();
    }
}

public partial class MultiPlotWithAdvancedLayoutWindow : Window
{
    public MultiPlotWithAdvancedLayoutWindow()
    {
        InitializeComponent();

        // setup a multiplot with 3 subplots
        AvaPlot.Multiplot.AddPlots(3);

        // add sample data to each subplot
        for (int i = 0; i < AvaPlot.Multiplot.Subplots.Count; i++)
        {
            double[] ys = ScottPlot.Generate.Sin(oscillations: i + 1);
            AvaPlot.Multiplot.GetPlot(i).Add.Signal(ys);
        }

        // use a fixed layout to ensure all plots remain aligned
        PixelPadding padding = new(50, 20, 40, 20);
        foreach (Plot plot in AvaPlot.Multiplot.GetPlots())
            plot.Layout.Fixed(padding);
    }

    public void OnRowsClick(object? sender, RoutedEventArgs e)
    {
        AvaPlot.Multiplot.Layout = new Rows();
        AvaPlot.Refresh();
    }

    public void OnColumnsClick(object? sender, RoutedEventArgs e)
    {
        AvaPlot.Multiplot.Layout = new Columns();
        AvaPlot.Refresh();
    }

    public void OnGridClick(object? sender, RoutedEventArgs e)
    {
        AvaPlot.Multiplot.Layout = new ScottPlot.MultiplotLayouts.Grid(2, 2);
        AvaPlot.Refresh();
    }

    public void OnMultColumnSpanClick(object? sender, RoutedEventArgs e)
    {
        CustomGrid customGrid = new();
        customGrid.Set(AvaPlot.Multiplot.GetPlot(0), new GridCell(0, 0, 2, 1));
        customGrid.Set(AvaPlot.Multiplot.GetPlot(1), new GridCell(1, 0, 2, 2));
        customGrid.Set(AvaPlot.Multiplot.GetPlot(2), new GridCell(1, 1, 2, 2));

        AvaPlot.Multiplot.Layout = customGrid;

        AvaPlot.Refresh();
    }

    public void OnPixelSizingClick(object? sender, RoutedEventArgs e)
    {
        AvaPlot.Multiplot.Layout = new FixedTopRowLayout(100);
        AvaPlot.Refresh();
    }

    /// <summary>
    /// Stack 3 plots in rows but force the center row to have a fixed pixel height
    /// </summary>
    class FixedTopRowLayout(int middlePlotHeight = 100) : IMultiplotLayout
    {
        int MiddlePlotHeight = middlePlotHeight;

        public ScottPlot.PixelRect[] GetSubplotRectangles(SubplotCollection subplots, ScottPlot.PixelRect figureRect)
        {
            ScottPlot.PixelRect[] rectangles = new ScottPlot.PixelRect[subplots.Count];

            ScottPlot.PixelSize middlePlotSize = new(figureRect.Width, MiddlePlotHeight);
            ScottPlot.PixelSize otherPlotSize = new(figureRect.Width, (figureRect.Height - MiddlePlotHeight) / 2);

            rectangles[0] = new ScottPlot.PixelRect(otherPlotSize).WithDelta(0, 0);
            rectangles[1] = new ScottPlot.PixelRect(middlePlotSize).WithDelta(0, rectangles[0].Bottom);
            rectangles[2] = new ScottPlot.PixelRect(otherPlotSize).WithDelta(0, rectangles[1].Bottom);

            return rectangles;
        }
    }
}
