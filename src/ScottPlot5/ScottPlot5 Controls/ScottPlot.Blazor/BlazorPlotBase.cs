using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SkiaSharp;

namespace ScottPlot.Blazor;

public abstract class BlazorPlotBase : ComponentBase, IPlotControl
{
    [Parameter]
    public string Style { get; set; } = string.Empty;

    [Parameter]
    public bool EnableRenderLoop { get; set; } = false;

    public Plot Plot { get; private set; }
    public IMultiplot Multiplot { get; set; }

    public IPlotMenu? Menu { get; set; }
    public Interactivity.UserInputProcessor UserInputProcessor { get; }

    public BlazorPlotBase()
    {
        Plot = new() { PlotControl = this };
        Multiplot = new Multiplot(Plot);
        DisplayScale = DetectDisplayScale();
        UserInputProcessor = new(this) { IsEnabled = true };
        Menu = new BlazorPlotMenu();
    }

    public GRContext? GRContext => null;

    public float DisplayScale { get; set; }

    public float DetectDisplayScale()
    {
        // TODO: improve support for DPI scale detection
        // https://github.com/ScottPlot/ScottPlot/issues/2760
        return 1.0f;
    }

    public void Reset() => Reset(new Plot());
    public void Reset(Plot plot)
    {
        Plot oldPlot = Plot;
        Plot = plot;
        oldPlot?.Dispose();
        Plot.PlotControl = this;
        Multiplot.Reset(Plot);
    }

    public virtual void Refresh() { }

    public void ShowContextMenu(Pixel position) => Menu?.ShowContextMenu(position);

    public EventHandler<Pixel>? MouseMoved;
    public EventHandler<(Pixel, ScottPlot.Interactivity.MouseButton)>? MouseButtonPressed;
    public EventHandler<(Pixel, ScottPlot.Interactivity.MouseButton)>? MouseButtonReleased;
    public EventHandler<(Pixel, double)>? MouseWheelChanged;
    public EventHandler<ScottPlot.Interactivity.Key>? KeyPressed;
    public EventHandler<ScottPlot.Interactivity.Key>? KeyReleased;

    public void OnPointerMoved(PointerEventArgs e)
    {
        UserInputProcessor.ProcessMouseMove(e);
        MouseMoved?.Invoke(this, e.ToPixel());
    }

    public void OnPointerPressed(PointerEventArgs e)
    {
        UserInputProcessor.ProcessMouseDown(e);
        MouseButtonPressed?.Invoke(this, (e.ToPixel(), e.ToScottPlotButton()));
    }

    public void OnPointerReleased(PointerEventArgs e)
    {
        UserInputProcessor.ProcessMouseUp(e);
        MouseButtonReleased?.Invoke(this, (e.ToPixel(), e.ToScottPlotButton()));
    }

    public void OnPointerWheelChanged(WheelEventArgs e)
    {
        UserInputProcessor.ProcessMouseWheel(e);
        MouseWheelChanged?.Invoke(this, (e.ToPixel(), e.DeltaY));
    }

    public void OnKeyDown(KeyboardEventArgs e)
    {
        UserInputProcessor.ProcessKeyDown(e);
        KeyPressed?.Invoke(this, e.ToKey());
    }

    public void OnKeyUp(KeyboardEventArgs e)
    {
        UserInputProcessor.ProcessKeyUp(e);
        KeyReleased?.Invoke(this, e.ToKey());
    }
}
