using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SkiaSharp.Views.Windows;
using ScottPlot.Control;

namespace ScottPlot.WinUI;

public partial class WinUIPlot : UserControl, IPlotControl
{
    private readonly SKXamlCanvas _canvas = CreateRenderTarget();

    public Plot Plot { get; internal set; } = new();

    public SkiaSharp.GRContext? GRContext => null;

    public IPlotInteraction Interaction { get; set; }
    public IPlotMenu Menu { get; set; }

    public Window? AppWindow { get; set; } // https://stackoverflow.com/a/74286947

    public float DisplayScale { get; set; } = 1;

    public WinUIPlot()
    {
        Interaction = new Interaction(this);
        Menu = new WinUIPlotMenu(this);

        Background = new SolidColorBrush(Microsoft.UI.Colors.White);

        _canvas.PaintSurface += OnPaintSurface;

        _canvas.PointerWheelChanged += OnPointerWheelChanged;
        _canvas.PointerReleased += OnPointerReleased;
        _canvas.PointerPressed += OnPointerPressed;
        _canvas.PointerMoved += OnPointerMoved;
        _canvas.DoubleTapped += OnDoubleTapped;
        _canvas.KeyDown += OnKeyDown;
        _canvas.KeyUp += OnKeyUp;
        Loaded += WinUIPlot_Loaded;

        this.Content = _canvas;
    }

    private void WinUIPlot_Loaded(object sender, RoutedEventArgs e)
    {
        if (XamlRoot is null)
            return;

        XamlRoot.Changed += (s, e) => DetectDisplayScale();
        Plot.ScaleFactor = XamlRoot.RasterizationScale;
        DisplayScale = (float)XamlRoot.RasterizationScale;
    }

    private static SKXamlCanvas CreateRenderTarget()
    {
        return new SKXamlCanvas
        {
            VerticalAlignment = Microsoft.UI.Xaml.VerticalAlignment.Stretch,
            HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Stretch,
            Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent)
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
        _canvas.Invalidate();
    }

    public void ShowContextMenu(Pixel position)
    {
        Menu.ShowContextMenu(position);
    }

    private void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        Plot.Render(e.Surface.Canvas, (int)e.Surface.Canvas.LocalClipBounds.Width, (int)e.Surface.Canvas.LocalClipBounds.Height);
    }

    private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        Focus(FocusState.Pointer);

        Interaction.MouseDown(e.Pixel(this), e.ToButton(this));

        (sender as UIElement)?.CapturePointer(e.Pointer);

        base.OnPointerPressed(e);
    }

    private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
    {
        Interaction.MouseUp(e.Pixel(this), e.ToButton(this));

        (sender as UIElement)?.ReleasePointerCapture(e.Pointer);

        base.OnPointerReleased(e);
    }

    private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
    {
        Interaction.OnMouseMove(e.Pixel(this));
        base.OnPointerMoved(e);
    }

    private void OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
    {
        Interaction.DoubleClick();
        base.OnDoubleTapped(e);
    }

    private void OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
    {
        Interaction.MouseWheelVertical(e.Pixel(this), e.GetCurrentPoint(this).Properties.MouseWheelDelta);
        base.OnPointerWheelChanged(e);
    }

    private void OnKeyDown(object sender, KeyRoutedEventArgs e)
    {
        Interaction.KeyDown(e.Key());
        base.OnKeyDown(e);
    }

    private void OnKeyUp(object sender, KeyRoutedEventArgs e)
    {
        Interaction.KeyUp(e.Key());
        base.OnKeyUp(e);
    }

    public float DetectDisplayScale()
    {
        if (XamlRoot is not null)
        {
            Plot.ScaleFactor = XamlRoot.RasterizationScale;
            DisplayScale = (float)XamlRoot.RasterizationScale;
        }

        return DisplayScale;
    }
}
