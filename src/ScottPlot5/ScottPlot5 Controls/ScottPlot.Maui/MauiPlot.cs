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
    public MauiPlot()
    {
        Plot = new Plot() { PlotControl = this };
        DisplayScale = DetectDisplayScale();
        Interaction = new Control.Interaction(this); // TODO: remove in an upcoming release
        UserInputProcessor = new(Plot) { IsEnabled = true };
        Menu = new MauiPlotMenu(this);

        var panGestureRecognizer = new PanGestureRecognizer();
        var pointerGestureRecognizer = new PointerGestureRecognizer();
        var tapGestureRecognizer = new TapGestureRecognizer() { Buttons = ButtonsMask.Secondary };
        var pinchGestureRecognizer = new PinchGestureRecognizer();

        panGestureRecognizer.PanUpdated += (s, e) =>
        {
            UserInputProcessor.ProcessPanUpdated(this, e);
        };

        pinchGestureRecognizer.PinchUpdated += (s, e) =>
        {
            UserInputProcessor.ProcessPinchUpdated(this, e);
        };

        pointerGestureRecognizer.PointerMoved += (s, e) =>
        {
            UserInputProcessor.ProcessPointerMoved(this, e);
        };

        pointerGestureRecognizer.PointerPressed += (s, e) =>
        {
            UserInputProcessor.ProcessPointerPressed(this, e);
        };

        pointerGestureRecognizer.PointerReleased += (s, e) =>
        {
            UserInputProcessor.ProcessPointerReleased(this, e);
        };

        tapGestureRecognizer.Tapped += (s, e) =>
        {
            UserInputProcessor.ProcessContext(this, e);
        };

        GestureRecognizers.Add(tapGestureRecognizer);
        GestureRecognizers.Add(pointerGestureRecognizer);
        GestureRecognizers.Add(pinchGestureRecognizer);
        GestureRecognizers.Add(panGestureRecognizer);
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

    internal static Page? GetFirstPageParent(Element element)
    {
        if (element.Parent is null) return null;
        else if (element.Parent is Page parent) return parent;
        else if (element.Parent is Element e) return GetFirstPageParent(e);
        else return null;
    }
}
