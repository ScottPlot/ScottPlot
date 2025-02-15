using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SkiaSharp;

namespace ScottPlot.WPF;

public abstract class WpfPlotBase : System.Windows.Controls.Control, IPlotControl
{
    public abstract GRContext GRContext { get; }
    public abstract void Refresh();

    public Plot Plot { get; internal set; }
    public IMultiplot Multiplot { get; set; }
    public float DisplayScale { get; set; }
    public IPlotMenu? Menu { get; set; }
    public Interactivity.UserInputProcessor UserInputProcessor { get; }

    protected abstract FrameworkElement PlotFrameworkElement { get; }
    static WpfPlotBase()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            forType: typeof(WpfPlotBase),
            typeMetadata: new FrameworkPropertyMetadata(typeof(WpfPlotBase)));
    }

    public WpfPlotBase()
    {
        Plot = new Plot() { PlotControl = this };
        Multiplot = new Multiplot(Plot);
        DisplayScale = DetectDisplayScale();
        UserInputProcessor = new(this);
        Menu = new WpfPlotMenu(this);
        Focusable = true;
    }

    public void Reset()
    {
        Reset(new Plot());
    }

    public void Reset(Plot newPlot)
    {
        Plot oldPlot = Plot;
        Plot = newPlot;
        oldPlot?.Dispose();
        Plot.PlotControl = this;
        Multiplot.Reset(newPlot);
    }

    public void ShowContextMenu(Pixel position)
    {
        Menu?.ShowContextMenu(position);
    }

    internal void SKElement_MouseDown(object? sender, MouseButtonEventArgs e)
    {
        Keyboard.Focus(this);
        UserInputProcessor.ProcessMouseDown(PlotFrameworkElement, e);
        (sender as UIElement)?.CaptureMouse();
    }

    internal void SKElement_MouseUp(object? sender, MouseButtonEventArgs e)
    {
        UserInputProcessor.ProcessMouseUp(PlotFrameworkElement, e);
        (sender as UIElement)?.ReleaseMouseCapture();
    }

    internal void SKElement_MouseMove(object? sender, MouseEventArgs e)
    {
        UserInputProcessor.ProcessMouseMove(PlotFrameworkElement, e);
    }

    internal void SKElement_MouseWheel(object? sender, MouseWheelEventArgs e)
    {
        UserInputProcessor.ProcessMouseWheel(PlotFrameworkElement, e);
    }

    internal void SKElement_KeyDown(object? sender, KeyEventArgs e)
    {
        UserInputProcessor.ProcessKeyDown(e);
    }

    internal void SKElement_KeyUp(object? sender, KeyEventArgs e)
    {
        UserInputProcessor.ProcessKeyUp(e);
    }

    internal void SKElement_LostFocus(object sender, RoutedEventArgs e)
    {
        UserInputProcessor.ProcessLostFocus();
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        UserInputProcessor.ProcessKeyDown(e);
        base.OnKeyDown(e);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        UserInputProcessor.ProcessKeyUp(e);
        base.OnKeyUp(e);
    }

    public float DetectDisplayScale()
    {
        return (float)VisualTreeHelper.GetDpi(this).DpiScaleX;
    }

    /// <summary>
    /// Returns the position of the mouse pointer relative to Plot drawing surface.
    /// </summary>
    /// <param name="e">Provides data for mouse related routed events</param>
    /// <returns>The x and y coordinates in pixel of the mouse pointer position relative to Plot. The point (0,0) is the upper-left corner of the plot.</returns>
    public Pixel GetPlotPixelPosition(MouseEventArgs e)
    {
        return e.ToPixel(PlotFrameworkElement);
    }

    /// <summary>
    /// Returns the current position of the mouse pointer relative to Plot drawing surface
    /// </summary>
    /// <returns>The x and y coordinates in pixel of the mouse pointer position relative to Plot. The point (0,0) is the upper-left corner of the plot.</returns>
    public Pixel GetCurrentPlotPixelPosition()
    {
        return PlotFrameworkElement.ToPixel(Mouse.GetPosition(PlotFrameworkElement));
    }
}
