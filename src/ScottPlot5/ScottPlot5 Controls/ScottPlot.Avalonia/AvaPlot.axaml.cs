using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using Avalonia.VisualTree;
using ScottPlot.Axis;
using ScottPlot.Control;
using SkiaSharp;

namespace ScottPlot.Avalonia;

public partial class AvaPlot : UserControl, IPlotControl
{
    public Plot Plot { get; } = new();

    public Interaction Interaction { get; private set; }

    public GRContext? GRContext => null;

    public float DisplayScale { get ; set; }

    private static readonly List<FilePickerFileType> filePickerFileTypes = new()
    {
        new("PNG Files") { Patterns = new List<string> { "*.png" } },
        new("JPEG Files") { Patterns = new List<string> { "*.jpg", "*.jpeg" } },
        new("BMP Files") { Patterns = new List<string> { "*.bmp" } },
        new("WebP Files") { Patterns = new List<string> { "*.webp" } },
        new("All Files") { Patterns = new List<string> { "*" } },
    };

    public AvaPlot()
    {
        DisplayScale = DetectDisplayScale();

        InitializeComponent();

        Interaction = new(this)
        {
            ContextMenuItems = GetDefaultContextMenuItems()
        };

        Refresh();
    }

    private ContextMenuItem[] GetDefaultContextMenuItems()
    {
        ContextMenuItem saveImage = new() { Label = "Save Image", OnInvoke = OpenSaveImageDialog };
        // TODO: Copying images to the clipboard is still difficult in Avalonia https://github.com/AvaloniaUI/Avalonia/issues/3588

        return new ContextMenuItem[] { saveImage };
    }

    private ContextMenu GetContextMenu()
    {
        List<MenuItem> items = new();

        foreach (var curr in Interaction.ContextMenuItems)
        {
            var menuItem = new MenuItem { Header = curr.Label };
            menuItem.Click += (s, e) => curr.OnInvoke();

            items.Add(menuItem);
        }

        return new()
        {
            ItemsSource = items
        };
    }

    private async void OpenSaveImageDialog()
    {
        var topLevel = TopLevel.GetTopLevel(this) ?? throw new NullReferenceException("Could not find a top level");
        var destinationFile = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {
            SuggestedFileName = Interaction.DefaultSaveImageFilename,
            FileTypeChoices = filePickerFileTypes
        });

        string? path = destinationFile?.TryGetLocalPath();
        if (path is not null && !string.IsNullOrWhiteSpace(path))
            Plot.Save(path, (int)Bounds.Width, (int)Bounds.Height, ImageFormatLookup.FromFilePath(path));
    }

    public void Replace(Interaction interaction)
    {
        Interaction = interaction;
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        SKImageInfo imageInfo = new((int)Bounds.Width, (int)Bounds.Height);

        using var surface = SKSurface.Create(imageInfo);
        if (surface is null)
            return;

        Plot.Render(surface);

        SKImage img = surface.Snapshot();
        SKPixmap pixels = img.ToRasterImage().PeekPixels();
        byte[] bytes = pixels.GetPixelSpan().ToArray();

        using WriteableBitmap bmp = new(
            size: new global::Avalonia.PixelSize((int)Bounds.Width, (int)Bounds.Height),
            dpi: new Vector(1, 1),
            format: PixelFormat.Bgra8888,
            alphaFormat: AlphaFormat.Unpremul);

        using ILockedFramebuffer buf = bmp.Lock();
        {
            Marshal.Copy(bytes, 0, buf.Address, pixels.BytesSize);
        }

        Rect rect = new(0, 0, Bounds.Width, Bounds.Height);

        context.DrawImage(bmp, rect, rect);
    }

    public void Refresh()
    {
        InvalidateVisual();
    }

    public void ShowContextMenu(Pixel position)
    {
        var manualContextMenu = GetContextMenu();

        // I am fully aware of how janky it is to place the menu in a 1x1 rect, unfortunately the Avalonia docs were down when I wrote this
        manualContextMenu.PlacementRect = new(position.X, position.Y, 1, 1);
        manualContextMenu.Open(this);
    }

    private void OnMouseDown(object sender, PointerPressedEventArgs e)
    {
        Interaction.MouseDown(
            position: e.ToPixel(this),
            button: e.GetCurrentPoint(this).Properties.PointerUpdateKind.ToButton());

        e.Pointer.Capture(this);

        if (e.ClickCount == 2)
        {
            Interaction.DoubleClick();
        }
    }

    private void OnMouseUp(object sender, PointerReleasedEventArgs e)
    {
        Interaction.MouseUp(
            position: e.ToPixel(this),
            button: e.GetCurrentPoint(this).Properties.PointerUpdateKind.ToButton());

        e.Pointer.Capture(null);
    }

    private void OnMouseMove(object sender, PointerEventArgs e)
    {
        Interaction.OnMouseMove(e.ToPixel(this));
    }

    private void OnMouseWheel(object sender, PointerWheelEventArgs e)
    {
        float delta = (float)e.Delta.Y; // This is now the correct behaviour even if shift is held, see https://github.com/AvaloniaUI/Avalonia/pull/8628

        if (delta != 0)
        {
            Interaction.MouseWheelVertical(e.ToPixel(this), delta);
        }
    }

    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        Interaction.KeyDown(e.ToKey());
    }

    private void OnKeyUp(object sender, KeyEventArgs e)
    {
        Interaction.KeyUp(e.ToKey());
    }

    public Coordinates GetCoordinates(Pixel px, IXAxis? xAxis = null, IYAxis? yAxis = null)
    {
        /* DISPLAY SCALING NOTE: 
         * If display scaling causes tracking issues, multiply X and Y by DisplayScale here.
         */
        return Plot.GetCoordinates(px, xAxis, yAxis);
    }

    public float DetectDisplayScale()
    {
        // TODO: improve support for DPI scale detection
        // https://github.com/ScottPlot/ScottPlot/issues/2760
        return 1.0f;
    }
}
