using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SkiaSharp;

namespace ScottPlot.WPF;

#pragma warning disable CS0618 

public abstract class WpfPlotBase : System.Windows.Controls.Control, IPlotControl
{
    public abstract GRContext GRContext { get; }
    public abstract void Refresh();

    public Plot Plot { get; internal set; }

    [Obsolete("Deprecated. Use UserInputProcessor instead. See ScottPlot.NET demo and FAQ for usage details.")]
    public IPlotInteraction Interaction { get; set; }
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
        DisplayScale = DetectDisplayScale();
        Interaction = new Control.Interaction(this); // TODO: remove in an upcoming release
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
    }

    public void ShowContextMenu(Pixel position)
    {
        Menu?.ShowContextMenu(position);
    }

    internal void SKElement_MouseDown(object? sender, MouseButtonEventArgs e)
    {
        Keyboard.Focus(this);
        Interaction.MouseDown(e.ToPixel(PlotFrameworkElement), e.OldToButton());
        UserInputProcessor.ProcessMouseDown(PlotFrameworkElement, e);
        (sender as UIElement)?.CaptureMouse();

        if (e.ClickCount == 2)
            Interaction.DoubleClick();
    }

    internal void SKElement_MouseUp(object? sender, MouseButtonEventArgs e)
    {
        Interaction.MouseUp(e.ToPixel(PlotFrameworkElement), e.OldToButton());
        UserInputProcessor.ProcessMouseUp(PlotFrameworkElement, e);
        (sender as UIElement)?.ReleaseMouseCapture();
    }

    internal void SKElement_MouseMove(object? sender, MouseEventArgs e)
    {
        Interaction.OnMouseMove(e.ToPixel(PlotFrameworkElement));
        UserInputProcessor.ProcessMouseMove(PlotFrameworkElement, e);
    }

    internal void SKElement_MouseWheel(object? sender, MouseWheelEventArgs e)
    {
        Interaction.MouseWheelVertical(e.ToPixel(PlotFrameworkElement), e.Delta);
        UserInputProcessor.ProcessMouseWheel(PlotFrameworkElement, e);
    }

    internal void SKElement_KeyDown(object? sender, KeyEventArgs e)
    {
        Interaction.KeyDown(e.OldToKey());
        UserInputProcessor.ProcessKeyDown(e);
    }

    internal void SKElement_KeyUp(object? sender, KeyEventArgs e)
    {
        Interaction.KeyUp(e.OldToKey());
        UserInputProcessor.ProcessKeyUp(e);
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        Interaction.KeyDown(e.OldToKey());
        UserInputProcessor.ProcessKeyDown(e);
        base.OnKeyDown(e);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        Interaction.KeyUp(e.OldToKey());
        UserInputProcessor.ProcessKeyUp(e);
        base.OnKeyUp(e);
    }

    public float DetectDisplayScale()
    {
        return (float)VisualTreeHelper.GetDpi(this).DpiScaleX;
    }
}
