using System;
using System.IO;
using System.Collections.Generic;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.ApplicationModel.DataTransfer;
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

    public Plot Plot { get; } = new();

    public SkiaSharp.GRContext? GRContext => null;

    public Interaction Interaction { get; private set; }

    public Window? AppWindow { get; set; } // https://stackoverflow.com/a/74286947

    public float DisplayScale { get; set; }

    public RenderQueue RenderQueue { get; } = new();

    public WinUIPlot()
    {
        DisplayScale = DetectDisplayScale();

        Interaction = new(this)
        {
            ContextMenuItems = GetDefaultContextMenuItems()
        };

        Background = new SolidColorBrush(Microsoft.UI.Colors.White);

        _canvas.PaintSurface += OnPaintSurface;

        _canvas.PointerWheelChanged += OnPointerWheelChanged;
        _canvas.PointerReleased += OnPointerReleased;
        _canvas.PointerPressed += OnPointerPressed;
        _canvas.PointerMoved += OnPointerMoved;
        _canvas.DoubleTapped += OnDoubleTapped;
        _canvas.KeyDown += OnKeyDown;
        _canvas.KeyUp += OnKeyUp;

        this.Content = _canvas;
    }

    private static SKXamlCanvas CreateRenderTarget()
    {
        return new SKXamlCanvas
        {
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent)
        };
    }

    private ContextMenuItem[] GetDefaultContextMenuItems()
    {
        ContextMenuItem saveImage = new() { Label = "Save Image", OnInvoke = OpenSaveImageDialog };
        ContextMenuItem copyImage = new() { Label = "Copy to Clipboard", OnInvoke = CopyImageToClipboard };

        return new ContextMenuItem[] { saveImage, copyImage };
    }

    private MenuFlyout GetContextMenu()
    {
        MenuFlyout menu = new();
        foreach (var curr in Interaction.ContextMenuItems)
        {
            var menuItem = new MenuFlyoutItem { Text = curr.Label };
            menuItem.Click += (s, e) => curr.OnInvoke();

            menu.Items.Add(menuItem);
        }

        return menu;
    }

    public void Replace(Interaction interaction)
    {
        Interaction = interaction;
    }

    public void Refresh()
    {
        _canvas.Invalidate();
        RenderQueue.RefreshAll();
    }

    public void ShowContextMenu(Pixel position)
    {
        var menu = GetContextMenu();

        menu.ShowAt(this, position.ToPoint());
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

    private async void OpenSaveImageDialog()
    {
        FileSavePicker dialog = new()
        {
            SuggestedFileName = Interaction.DefaultSaveImageFilename
        };
        dialog.FileTypeChoices.Add("PNG Files", new List<string>() { ".png" });
        dialog.FileTypeChoices.Add("JPEG Files", new List<string>() { ".jpg", ".jpeg" });
        dialog.FileTypeChoices.Add("BMP Files", new List<string>() { ".bmp" });
        dialog.FileTypeChoices.Add("WebP Files", new List<string>() { ".webp" });

#if NET6_0_WINDOWS10_0_18362 // https://github.com/microsoft/CsWinRT/blob/master/docs/interop.md#windows-sdk
        // TODO: launch a pop-up window or otherwise inform if AppWindow is not set before using save-dialog
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(AppWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(dialog, hwnd);
#endif

        var file = await dialog.PickSaveFileAsync();

        if (file != null)
        {
            // TODO: launch a pop-up window indicating if extension is invalid or save failed
            ImageFormat format = ImageFormatLookup.FromFilePath(file.Name);
            Plot.Save(file.Path, (int)ActualWidth, (int)ActualHeight, format);
        }
    }

    private void CopyImageToClipboard()
    {
        byte[] bytes = Plot.GetImage((int)ActualWidth, (int)ActualHeight).GetImageBytes();

        var stream = new InMemoryRandomAccessStream();
        stream.AsStreamForWrite().Write(bytes);

        var content = new DataPackage();
        content.SetBitmap(RandomAccessStreamReference.CreateFromStream(stream));

        Clipboard.SetContent(content);
    }

    public Coordinates GetCoordinates(Pixel px, IXAxis? xAxis = null, IYAxis? yAxis = null)
    {
        return Plot.GetCoordinates(px, xAxis, yAxis);
    }

    public float DetectDisplayScale()
    {
        // TODO: improve support for DPI scale detection
        // https://github.com/ScottPlot/ScottPlot/issues/2760
        return 1.0f;
    }
}
