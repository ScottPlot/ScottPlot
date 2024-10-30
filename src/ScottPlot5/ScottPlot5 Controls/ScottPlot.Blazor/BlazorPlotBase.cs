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

    [Obsolete("Deprecated. Use UserInputProcessor instead. See ScottPlot.NET demo and FAQ for usage details.")]
    public IPlotInteraction Interaction { get; set; }

    public IPlotMenu? Menu { get; set; }
    public Interactivity.UserInputProcessor UserInputProcessor { get; }

    public BlazorPlotBase()
    {
        Plot = new() { PlotControl = this };
        DisplayScale = DetectDisplayScale();

#pragma warning disable CS0618 
        Interaction = new Control.Interaction(this); // TODO: remove in an upcoming release
#pragma warning restore CS0618

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
    }

    public virtual void Refresh() { }

    public void ShowContextMenu(Pixel position) => Menu?.ShowContextMenu(position);

    public void OnPointerMoved(PointerEventArgs e) => UserInputProcessor.ProcessMouseMove(e);
    public void OnPointerPressed(PointerEventArgs e) => UserInputProcessor.ProcessMouseDown(e);
    public void OnPointerReleased(PointerEventArgs e) => UserInputProcessor.ProcessMouseUp(e);
    public void OnPointerWheelChanged(WheelEventArgs e) => UserInputProcessor.ProcessMouseWheel(e);
    public void OnKeyDown(KeyboardEventArgs e) => UserInputProcessor.ProcessKeyDown(e);
    public void OnKeyUp(KeyboardEventArgs e) => UserInputProcessor.ProcessKeyUp(e);
}
