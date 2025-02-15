using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SkiaSharp.Views.Windows;

namespace ScottPlot.WinUI;

public partial class WinUIPlot : UserControl, IPlotControl
{
    public Plot Plot { get; internal set; }
    public IMultiplot Multiplot { get; set; }
    public SkiaSharp.GRContext? GRContext => null;

    public IPlotMenu? Menu { get; set; }
    public Interactivity.UserInputProcessor UserInputProcessor { get; }
    public Window? AppWindow { get; set; } // https://stackoverflow.com/a/74286947
    public float DisplayScale { get; set; } = 1;

    private readonly SKXamlCanvas _canvas = CreateRenderTarget();

    public WinUIPlot()
    {
        Plot = new() { PlotControl = this };
        Multiplot = new Multiplot(Plot);
        UserInputProcessor = new(this);
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
        Plot.PlotControl = this;
        Multiplot.Reset(plot);
    }

    public void Refresh()
    {
        _canvas.Invalidate();
    }

    public void ShowContextMenu(Pixel position)
    {
        Menu?.ShowContextMenu(position);
    }

    private void OnPaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        Multiplot.Render(e.Surface);
    }

    private void OnPointerPressed(object sender, PointerRoutedEventArgs e)
    {
        Focus(FocusState.Pointer);
        UserInputProcessor.ProcessMouseDown(this, e);
        (sender as UIElement)?.CapturePointer(e.Pointer);
        base.OnPointerPressed(e);
    }

    private void OnPointerReleased(object sender, PointerRoutedEventArgs e)
    {
        UserInputProcessor.ProcessMouseUp(this, e);
        (sender as UIElement)?.ReleasePointerCapture(e.Pointer);
        base.OnPointerReleased(e);
    }

    private void OnPointerMoved(object sender, PointerRoutedEventArgs e)
    {
        UserInputProcessor.ProcessMouseMove(this, e);
        base.OnPointerMoved(e);
    }

    private void OnDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
    {
        base.OnDoubleTapped(e);
    }

    private void OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
    {
        UserInputProcessor.ProcessMouseWheel(this, e);
        base.OnPointerWheelChanged(e);
    }

    private void OnKeyDown(object sender, KeyRoutedEventArgs e)
    {
        UserInputProcessor.ProcessKeyDown(this, e);
        base.OnKeyDown(e);
    }

    private void OnKeyUp(object sender, KeyRoutedEventArgs e)
    {
        System.Diagnostics.Debug.WriteLine($"KEY UP {e.Key}");
        UserInputProcessor.ProcessKeyUp(this, e);
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
