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

    public IPlotInteraction Interaction { get; set; }

    public IPlotMenu Menu { get; set; }
    public Interactivity.UserInputProcessor UserInputProcessor { get; }

    public BlazorPlotBase()
    {
        Plot = new() { PlotControl = this };
        DisplayScale = DetectDisplayScale();
        Interaction = new Control.Interaction(this);
        UserInputProcessor = new(Plot) { IsEnabled = true };
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
        UserInputProcessor.Plot = Plot;
    }

    public virtual void Refresh() { }

    public void ShowContextMenu(Pixel position) => Menu.ShowContextMenu(position);

    public void OnPointerMoved(PointerEventArgs e) => UserInputProcessor.ProcessMouseMove(e);
    public void OnPointerPressed(PointerEventArgs e) => UserInputProcessor.ProcessMouseDown(e);
    public void OnPointerReleased(PointerEventArgs e) => UserInputProcessor.ProcessMouseUp(e);
    public void OnPointerWheelChanged(WheelEventArgs e) => UserInputProcessor.ProcessMouseWheel(e);
    public void OnKeyDown(KeyboardEventArgs e) => UserInputProcessor.ProcessKeyDown(e);
    public void OnKeyUp(KeyboardEventArgs e) => UserInputProcessor.ProcessKeyUp(e);
}
