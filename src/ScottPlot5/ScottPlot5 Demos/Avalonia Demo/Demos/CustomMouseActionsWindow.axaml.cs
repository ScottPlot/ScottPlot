using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ScottPlot;
using ScottPlot.Avalonia;
using System;

namespace Avalonia_Demo.Demos;

public class CustomMouseActionDemo : IDemo
{
    public string Title => "Custom Mouse Actions";
    public string Description => "Demonstrates how to disable the mouse or changes what the button actions are";

    public Window GetWindow()
    {
        return new CustomMouseActionWindow();
    }

}

public partial class CustomMouseActionWindow : Window
{
    public CustomMouseActionWindow()
    {
        InitializeComponent();

        AvaPlot.Plot.Add.Signal(ScottPlot.Generate.Sin());
        AvaPlot.Plot.Add.Signal(ScottPlot.Generate.Cos());
        AvaPlot.Refresh();

        ResetToDefault();
    }

    private void ResetToDefault()
    {
        MouseActionDescription.Text = "left-click-drag pan, right-click-drag zoom, middle-click autoscale, " +
                "middle-click-drag zoom rectangle, alt+left-click-drag zoom rectangle, right-click menu, " +
                "double-click benchmark, scroll wheel zoom, arrow keys pan, " +
                "shift or alt with arrow keys pans more or less, ctrl+arrow keys zoom";

        AvaPlot.UserInputProcessor.IsEnabled = true;
        AvaPlot.UserInputProcessor.Reset();
    }

    private void OnDefaultButtonClicked(object? sender, RoutedEventArgs e)
    {
        ResetToDefault();
    }

    private void OnDisableButtonClicked(object? sender, RoutedEventArgs e)
    {
        MouseActionDescription.Text = "Mouse and keyboard events are disabled";
        AvaPlot.UserInputProcessor.IsEnabled = false;
    }

    private void OnCustomButtonClicked(object? sender, RoutedEventArgs e)
    {
        MouseActionDescription.Text = "middle-click-drag pan, right-click-drag zoom rectangle, " +
                "right-click autoscale, left-click menu, Q key autoscale, WASD keys pan";

        AvaPlot.UserInputProcessor.IsEnabled = true;

        // remove all existing responses so we can create and add our own
        AvaPlot.UserInputProcessor.UserActionResponses.Clear();

        // middle-click-drag pan
        var panButton = ScottPlot.Interactivity.StandardMouseButtons.Middle;
        var panResponse = new ScottPlot.Interactivity.UserActionResponses.MouseDragPan(panButton);
        AvaPlot.UserInputProcessor.UserActionResponses.Add(panResponse);

        // right-click-drag zoom rectangle
        var zoomRectangleButton = ScottPlot.Interactivity.StandardMouseButtons.Right;
        var zoomRectangleResponse = new ScottPlot.Interactivity.UserActionResponses.MouseDragZoomRectangle(zoomRectangleButton);
        AvaPlot.UserInputProcessor.UserActionResponses.Add(zoomRectangleResponse);

        // right-click autoscale
        var autoscaleButton = ScottPlot.Interactivity.StandardMouseButtons.Right;
        var autoscaleResponse = new ScottPlot.Interactivity.UserActionResponses.SingleClickAutoscale(autoscaleButton);
        AvaPlot.UserInputProcessor.UserActionResponses.Add(autoscaleResponse);

        // left-click menu
        var menuButton = ScottPlot.Interactivity.StandardMouseButtons.Left;
        var menuResponse = new ScottPlot.Interactivity.UserActionResponses.SingleClickContextMenu(menuButton);
        AvaPlot.UserInputProcessor.UserActionResponses.Add(menuResponse);

        // Q key autoscale too
        var autoscaleKey = new ScottPlot.Interactivity.Key("Q");
        Action<ScottPlot.IPlotControl, ScottPlot.Pixel> autoscaleAction = (plotControl, pixel) => plotControl.Plot.Axes.AutoScale();
        var autoscaleKeyResponse = new ScottPlot.Interactivity.UserActionResponses.KeyPressResponse(autoscaleKey, autoscaleAction);
        AvaPlot.UserInputProcessor.UserActionResponses.Add(autoscaleKeyResponse);

        // WASD keys pan
        var keyPanResponse = new ScottPlot.Interactivity.UserActionResponses.KeyboardPanAndZoom()
        {
            PanUpKey = new ScottPlot.Interactivity.Key("W"),
            PanLeftKey = new ScottPlot.Interactivity.Key("A"),
            PanDownKey = new ScottPlot.Interactivity.Key("S"),
            PanRightKey = new ScottPlot.Interactivity.Key("D"),
        };
        AvaPlot.UserInputProcessor.UserActionResponses.Add(keyPanResponse);
    }
}
