using ScottPlot.Control;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ScottPlot.Maui;

public partial class MauiPlot : ContentView, IPlotControl
{
    public readonly SKCanvasView _canvas = CreateRenderTarget();

    public Plot Plot { get; internal set; } = new();

    public GRContext? GRContext => null;

    public IPlotInteraction Interaction { get; set; }
    public IPlotMenu Menu { get; set; }

    public float DisplayScale { get; set; } = 1;

    public MauiPlot()
    {
        PointerGestureRecognizer point = new PointerGestureRecognizer();
        TapGestureRecognizer tap = new TapGestureRecognizer();

        DisplayScale = DetectDisplayScale();
        Interaction = new Interaction(this);
        Menu = new MauiPlotMenu(this);
        Content = _canvas;
        _canvas.PaintSurface += OnPaintSurface;
        Background = SolidColorBrush.Aqua;

        point.PointerMoved += OnPointerMoved;
        point.PointerPressed += OnPointerPressed;
        point.PointerReleased += OnPointerReleased;

        tap.Tapped += OnTapped;

        _canvas.GestureRecognizers.Add(point);
        _canvas.GestureRecognizers.Add(tap);
        Refresh();
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
#if WINDOWS
        var view = _canvas.Handler.PlatformView as SkiaSharp.Views.Windows.SKXamlCanvas;
        view.PointerWheelChanged += (s, e) =>
        {
            var point = e.GetCurrentPoint(s as Microsoft.Maui.Platform.ContentPanel);
            var delta = point.Properties.MouseWheelDelta;
            Pixel ePixel = new Pixel(point.Position.X, point.Position.Y);
            Interaction.MouseWheelVertical(ePixel, delta);
            Debug.WriteLine($"ScrollPos: {point.Position.X} - {point.Position.Y}");
        };
#endif
    }

    private void OnPointerMoved(object? s, PointerEventArgs e)
    {
        Pixel ePixel = GetMousePos(e);
        Interaction.OnMouseMove(ePixel);
        Debug.WriteLine($"MousePos: {ePixel.X} - {ePixel.Y}");
    }
    private void OnPointerPressed(object? s, PointerEventArgs e)
    {
        Pixel ePixel = GetMousePos(e);
        Interaction.MouseDown(ePixel, MouseButton.Left);
    }
    private void OnPointerReleased(object? s, PointerEventArgs e)
    {
        Pixel ePixel = GetMousePos(e);
        Interaction.MouseUp(ePixel, MouseButton.Left);
    }

    private void OnTapped(object? s, TappedEventArgs e)
    {
        if (e.Buttons == ButtonsMask.Secondary)
        {
            Pixel ePixel = GetMousePos(e);
        }
    }

private Pixel GetMousePos(DragStartingEventArgs e)
    {
        Point? position = e.GetPosition(null);
        if (position is null)
            return Pixel.NaN;
        Point tmpPos = new Point(position.Value.X, position.Value.Y);
        return tmpPos.ToPixel();
    }
    private Pixel GetMousePos(TappedEventArgs e)
    {
        Point? position = e.GetPosition(null);
        if (position is null)
            return Pixel.NaN;
        Point tmpPos = new Point(position.Value.X, position.Value.Y);
        return tmpPos.ToPixel();
    }
    private Pixel GetMousePos(PointerEventArgs e)
    {
        Point? position = e.GetPosition(null);
        if (position is null)
            return Pixel.NaN;
        Point tmpPos = new Point(position.Value.X, position.Value.Y);
        return tmpPos.ToPixel();
    }

    private static SKCanvasView CreateRenderTarget()
    {
        return new SKCanvasView
        {
            Background = new SolidColorBrush(Microsoft.Maui.Graphics.Colors.Transparent)
        };
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
        _canvas.InvalidateSurface();
    }

    public void ShowContextMenu(Pixel position)
    {
        Menu.ShowContextMenu(position);
    }

    private void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        Plot.Render(e.Surface.Canvas, (int)e.Surface.Canvas.LocalClipBounds.Width, (int)e.Surface.Canvas.LocalClipBounds.Height);
    }

    public float DetectDisplayScale()
    {
        return 1.0f;
    }
}