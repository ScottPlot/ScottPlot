using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Skia;
using Avalonia.Input;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using Avalonia.Rendering.SceneGraph;
using ScottPlot.Control;
using SkiaSharp;

using Controls = Avalonia.Controls;

namespace ScottPlot.Avalonia;

public class AvaPlot : Controls.Control, IPlotControl
{
    public Plot Plot { get; } = new();

    public Interaction Interaction { get; private set; }

    public GRContext? GRContext => null;

    public float DisplayScale { get; set; }

    public RenderQueue RenderQueue { get; } = new();


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

    private class CustomDrawOp : ICustomDrawOperation
    {
        private readonly Plot _plot;

        public Rect Bounds { get; }
        public bool HitTest(Point p) => true;
        public bool Equals(ICustomDrawOperation? other) => false;

        public CustomDrawOp(Rect bounds, Plot plot)
        {
            _plot = plot;
            Bounds = bounds;
        }

        public void Dispose()
        {
            // No-op
        }

        public void Render(ImmediateDrawingContext context)
        {
            var leaseFeature = context.TryGetFeature<ISkiaSharpApiLeaseFeature>();
            if (leaseFeature is null) return;

            using var lease = leaseFeature.Lease();

            var surface = lease.SkSurface;
            if (surface is null) return;

            _plot.Render(surface);
        }
    }

    public override void Render(DrawingContext context)
    {
        context.Custom(new CustomDrawOp(Bounds, Plot));
    }

    public void Refresh()
    {
        InvalidateVisual();
        RenderQueue.RefreshAll();
    }

    public void ShowContextMenu(Pixel position)
    {
        var manualContextMenu = GetContextMenu();

        // I am fully aware of how janky it is to place the menu in a 1x1 rect, unfortunately the Avalonia docs were down when I wrote this
        manualContextMenu.PlacementRect = new(position.X, position.Y, 1, 1);
        manualContextMenu.Open(this);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
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

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        Interaction.MouseUp(
            position: e.ToPixel(this),
            button: e.GetCurrentPoint(this).Properties.PointerUpdateKind.ToButton());

        e.Pointer.Capture(null);
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        Interaction.OnMouseMove(e.ToPixel(this));
    }

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        float delta = (float)e.Delta.Y; // This is now the correct behavior even if shift is held, see https://github.com/AvaloniaUI/Avalonia/pull/8628

        if (delta != 0)
        {
            Interaction.MouseWheelVertical(e.ToPixel(this), delta);
        }
    }

    protected override void OnKeyDown(KeyEventArgs e)
    {
        Interaction.KeyDown(e.ToKey());
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        Interaction.KeyUp(e.ToKey());
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
