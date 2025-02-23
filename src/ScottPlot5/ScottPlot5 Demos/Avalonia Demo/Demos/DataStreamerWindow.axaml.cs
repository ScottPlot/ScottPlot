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

public class DataStreamerDemo : IDemo
{
    public string Title => "Data Streamer";
    public string Description => "Plots live streaming data as a fixed-width line plot, " +
        "shifting old data out as new data comes in.";

    public Window GetWindow()
    {
        return new DataStreamerWindow();
    }
}

public partial class DataStreamerWindow : Window
{
    readonly DispatcherTimer AddNewDataTimer = new() { Interval = TimeSpan.FromMilliseconds(10) };
    readonly DispatcherTimer UpdatePlotTimer = new() { Interval = TimeSpan.FromMilliseconds(50) };

    readonly ScottPlot.Plottables.DataStreamer Streamer1;
    readonly ScottPlot.Plottables.DataStreamer Streamer2;

    readonly ScottPlot.DataGenerators.RandomWalker Walker1 = new(0);
    readonly ScottPlot.DataGenerators.RandomWalker Walker2 = new(1);

    readonly ScottPlot.Plottables.VerticalLine VLine;

    private DataStreamerViewModel TypedDataContext => (DataContext as DataStreamerViewModel) ?? throw new ArgumentException(nameof(DataContext));

    public DataStreamerWindow()
    {
        InitializeComponent();

        DataContext = new DataStreamerViewModel();
        TypedDataContext.PropertyChanged += HandleDataContextChanged;

        Streamer1 = AvaPlot.Plot.Add.DataStreamer(1000);
        Streamer2 = AvaPlot.Plot.Add.DataStreamer(1000);
        VLine = AvaPlot.Plot.Add.VerticalLine(0, 2, ScottPlot.Colors.Red);

        // disable mouse interaction by default
        AvaPlot.UserInputProcessor.Disable();

        // setup a timer to add data to the streamer periodically
        AddNewDataTimer.Tick += (s, e) =>
        {
            int count = 5;

            // add new sample data
            Streamer1.AddRange(Walker1.Next(count));
            Streamer2.AddRange(Walker2.Next(count));

            // slide marker to the left
            AvaPlot.Plot.GetPlottables<Marker>()
                .ToList()
                .ForEach(m => m.X -= count);

            // remove off-screen marks
            AvaPlot.Plot.GetPlottables<Marker>()
                .Where(m => m.X < 0)
                .ToList()
                .ForEach(m => AvaPlot.Plot.Remove(m));
        };

        // setup a timer to request a render periodically
        UpdatePlotTimer.Tick += (s, e) =>
        {
            if (Streamer1.HasNewData)
            {
                AvaPlot.Plot.Title($"Processed {Streamer1.Data.CountTotal:N0} points");
                VLine.IsVisible = Streamer1.Renderer is ScottPlot.DataViews.Wipe;
                VLine.Position = Streamer1.Data.NextIndex * Streamer1.Data.SamplePeriod + Streamer1.Data.OffsetX;
                AvaPlot.Refresh();
            }
        };

        AddNewDataTimer.Start();
        UpdatePlotTimer.Start();
    }



    private void HandleDataContextChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(TypedDataContext.SelectedDataStreamerViewMode))
        {
            UpdateStreamerMode();
        }
        else if (args.PropertyName == nameof(TypedDataContext.SelectedDataStreamerAxisOption))
        {
            UpdateAxisMode();
        }
    }

    private void UpdateStreamerMode()
    {
        if (TypedDataContext.SelectedDataStreamerViewMode == DataStreamerViewMode.Scroll)
        {
            Streamer1.ViewScrollLeft();
            Streamer2.ViewScrollLeft();
        }
        else if (TypedDataContext.SelectedDataStreamerViewMode == DataStreamerViewMode.Wipe)
        {
            Streamer1.ViewWipeRight(0.1);
            Streamer2.ViewWipeRight(0.1);
            AvaPlot.Plot.Remove<Marker>();
        }
        else
        {
            throw new NotImplementedException(TypedDataContext.SelectedDataStreamerViewMode.ToString());
        }
    }

    private void UpdateAxisMode()
    {
        if (TypedDataContext.SelectedDataStreamerAxisOption == DataStreamerAxisOptions.ManageAxisLimits)
        {
            AvaPlot.Plot.Axes.ContinuouslyAutoscale = false;
            Streamer1.ManageAxisLimits = true;
            Streamer2.ManageAxisLimits = true;
        }
        else if (TypedDataContext.SelectedDataStreamerAxisOption == DataStreamerAxisOptions.ContinuouslyAutoscale)
        {
            AvaPlot.Plot.Axes.ContinuouslyAutoscale = true;
            Streamer1.ManageAxisLimits = false;
            Streamer2.ManageAxisLimits = false;
        }
    }

    private void OnMarkButtonClick(Object sender, RoutedEventArgs e)
    {
        double x1 = Streamer1.Count;
        double y1 = Streamer1.Data.NewestPoint;
        var marker1 = AvaPlot.Plot.Add.Marker(x1, y1);
        marker1.Size = 20;
        marker1.Shape = MarkerShape.OpenCircle;
        marker1.Color = Streamer1.LineColor;
        marker1.LineWidth = 2;

        double x2 = Streamer2.Count;
        double y2 = Streamer2.Data.NewestPoint;
        var marker2 = AvaPlot.Plot.Add.Marker(x2, y2);
        marker2.Size = 20;
        marker2.Shape = MarkerShape.OpenCircle;
        marker2.Color = Streamer2.LineColor;
        marker2.LineWidth = 2;
    }
}
