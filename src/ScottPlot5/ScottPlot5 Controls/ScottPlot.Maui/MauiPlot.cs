using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ScottPlot.Maui;

#pragma warning disable CS0618 // disable obsolete warnings

public class MauiPlot : SKCanvasView, IPlotControl
{
    public Plot Plot { get; internal set; } = new();
    public SkiaSharp.GRContext? GRContext => null;

    [Obsolete("Deprecated. Use UserInputProcessor instead. See ScottPlot.NET demo and FAQ for usage details.")]
    public IPlotInteraction Interaction { get; set; }
    public IPlotMenu? Menu { get; set; }
    public Interactivity.UserInputProcessor UserInputProcessor { get; }
    public float DisplayScale { get; set; } = 1;
    internal Pixel LastPixel { get; set; }
    internal Pixel LastScalePixel { get; set; }
    public MauiPlot()
    {
        Plot = new Plot() { PlotControl = this };
        DisplayScale = DetectDisplayScale();
        Interaction = new Control.Interaction(this); // TODO: remove in an upcoming release
        UserInputProcessor = new(this) { IsEnabled = true };
        Menu = new MauiPlotMenu(this);

        IgnorePixelScaling = true;

        if (DeviceInfo.Idiom == DeviceIdiom.Desktop)
        {
            Touch += MauiPlot_Touch;
            EnableTouchEvents = true;
        }
        else
        {
            EnableTouchEvents = false;
            var panGestureRecognizer = new PanGestureRecognizer();
            var pinchGestureRecognizer = new PinchGestureRecognizer();

            panGestureRecognizer.PanUpdated += (s, e) => UserInputProcessor.ProcessPanUpdated(this, e);
            pinchGestureRecognizer.PinchUpdated += (s, e) => UserInputProcessor.ProcessPinchUpdated(this, e, (float)Width, (float)Height);

            GestureRecognizers.Add(pinchGestureRecognizer);
            GestureRecognizers.Add(panGestureRecognizer);
        }
    }

    private void MauiPlot_Touch(object? sender, SKTouchEventArgs e)
    {
        switch (e.ActionType)
        {
            case SKTouchAction.Pressed:
                UserInputProcessor.ProcessMouseDown(this, e);
                break;
            case SKTouchAction.Moved:
                UserInputProcessor.ProcessMouseMove(this, e);
                break;
            case SKTouchAction.Released:
                UserInputProcessor.ProcessMouseUp(this, e);
                break;
            case SKTouchAction.WheelChanged:
                UserInputProcessor.ProcessWheelChanged(this, e);
                break;
            default: break;
        }
    }

    public void Reset()
    {
        Reset(new Plot());
    }

    public void Reset(Plot plot)
    {
        Plot = plot;
    }

    public void Refresh()
    {
        InvalidateSurface();
    }

    public void ShowContextMenu(Pixel position)
    {
        Menu?.ShowContextMenu(position);
    }

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        base.OnPaintSurface(e);

        e.Surface.Canvas.Clear();
        Plot.Render(e.Surface.Canvas, e.Info.Width, e.Info.Height);
    }

    public float DetectDisplayScale()
    {
        if (Parent is VisualElement parent)
        {
            Plot.ScaleFactor = parent.Scale;
            DisplayScale = (float)parent.Scale;
        }

        return DisplayScale;
    }
}
