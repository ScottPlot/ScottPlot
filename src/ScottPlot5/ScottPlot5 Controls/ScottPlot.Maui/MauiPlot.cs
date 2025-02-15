using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ScottPlot.Maui;

public class MauiPlot : SKCanvasView, IPlotControl
{
    public Plot Plot { get; internal set; } = new();
    public IMultiplot Multiplot { get; set; }
    public SkiaSharp.GRContext? GRContext => null;
    public IPlotMenu? Menu { get; set; }
    public Interactivity.UserInputProcessor UserInputProcessor { get; }
    public float DisplayScale { get; set; } = 1;
    internal Pixel LastPixel { get; set; }
    internal Pixel LastScalePixel { get; set; }
    public MauiPlot()
    {
        Plot = new Plot() { PlotControl = this };
        Multiplot = new Multiplot(Plot);
        DisplayScale = DetectDisplayScale();
        UserInputProcessor = new(this);
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
            var tapGestureRecognizer = new TapGestureRecognizer() { NumberOfTapsRequired = 2 };

            panGestureRecognizer.PanUpdated += (s, e) => UserInputProcessor.ProcessPanUpdated(this, e);
            pinchGestureRecognizer.PinchUpdated += (s, e) => UserInputProcessor.ProcessPinchUpdated(this, e, (float)Width, (float)Height);
            tapGestureRecognizer.Tapped += (s, e) => UserInputProcessor.ProcessZoomAll(this, e);

            GestureRecognizers.Add(pinchGestureRecognizer);
            GestureRecognizers.Add(panGestureRecognizer);
            GestureRecognizers.Add(tapGestureRecognizer);
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
        Plot.PlotControl = this;
        Multiplot.Reset(plot);
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
        Multiplot.Render(e.Surface);
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
