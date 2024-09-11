using ScottPlot.Control;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;

namespace ScottPlot.Maui;

public partial class MauiPlot : ContentPage, IPlotControl
{
    private readonly SKCanvasView _canvas = CreateRenderTarget();

    public Plot Plot { get; internal set; } = new();

    private ContentPage? XamlRoot = null;

    public SkiaSharp.GRContext? GRContext => null;

    public IPlotInteraction Interaction { get; set; }
    public IPlotMenu? Menu { get; set; }
    public Interactivity.UserInputProcessor UserInputProcessor { get; }

    public float DisplayScale { get; set; } = 1;

    SKPaint skPaint = new SKPaint()
    {
        Style = SKPaintStyle.Stroke,
        Color = SKColors.DeepPink,
        StrokeWidth = 10,
        IsAntialias = true,
    };

    public MauiPlot()
    {
        Interaction = new Interaction(this);
        UserInputProcessor = new(Plot);
        Menu = new MauiPlotMenu(this);
        PointerGestureRecognizer pointerGestureRecognizer = new PointerGestureRecognizer();
        TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();

        Background = new SolidColorBrush(Microsoft.Maui.Graphics.Colors.Gray);

        _canvas.PaintSurface += (s, e) =>
        {
            var surface = e.Surface;
            var canvas = surface.Canvas;

            SKRect skRectangle = new SKRect();
            skRectangle.Size = new SKSize(100, 100);
            skRectangle.Location = new SKPoint(-100f / 2, -100f / 2);

            canvas.DrawRect(skRectangle, skPaint);
        };

        pointerGestureRecognizer.PointerMoved += (s, e) =>
        {
            Pixel ePixel = GetMousePos(e);
            Interaction.OnMouseMove(ePixel);
        };

        pointerGestureRecognizer.PointerPressed += (s, e) =>
        {
            Pixel ePixel = GetMousePos(e);
            Interaction.MouseDown(ePixel, MouseButton.Left);
        };

        pointerGestureRecognizer.PointerReleased += (s, e) =>
        {
            Pixel ePixel = GetMousePos(e);
            Interaction.MouseUp(ePixel, MouseButton.Left);
        };

        tapGestureRecognizer.Tapped += (s, e) =>
        {
            if (e.Buttons == ButtonsMask.Secondary)
            {
                // Do something
                Pixel ePixel = GetMousePos(e);
            }
        };

        _canvas.GestureRecognizers.Add(pointerGestureRecognizer);
        _canvas.GestureRecognizers.Add(tapGestureRecognizer);

        /*this.Content = _canvas;*/
    }

    private Pixel GetMousePos(TappedEventArgs e)
    {
        Point? position = e.GetPosition(null);
        if (position is null)
            return Pixel.NaN;
        Point tmpPos = new Point(position.Value.X, position.Value.X);
        return tmpPos.ToPixel();
    }
    private Pixel GetMousePos(PointerEventArgs e)
    {
        Point? position = e.GetPosition(null);
        if (position is null)
            return Pixel.NaN;
        Point tmpPos = new Point(position.Value.X, position.Value.X);
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
        Menu?.ShowContextMenu(position);
    }

    private void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        Plot.Render(e.Surface.Canvas, (int)e.Surface.Canvas.LocalClipBounds.Width, (int)e.Surface.Canvas.LocalClipBounds.Height);
    }

    public float DetectDisplayScale()
    {
        if (XamlRoot is not null)
        {
            Plot.ScaleFactor = XamlRoot.Scale;
            DisplayScale = (float)XamlRoot.Scale;
        }

        return DisplayScale;
    }
}
