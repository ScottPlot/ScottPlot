using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace ScottPlot.Avalonia;

public class AvaPlotMenu : IPlotMenu
{
    public string DefaultSaveImageFilename { get; set; } = "Plot.png";
    public List<ContextMenuItem> ContextMenuItems { get; set; } = new();
    private readonly AvaPlot ThisControl;

    public AvaPlotMenu(AvaPlot avaPlot)
    {
        ThisControl = avaPlot;
        Reset();
    }

    public ContextMenuItem[] GetDefaultContextMenuItems()
    {
        ContextMenuItem saveImage = new()
        {
            Label = "Save Image",
            OnInvoke = OpenSaveImageDialog
        };

        ContextMenuItem copyImage = new()
        {
            Label = "Copy to Clipboard",
            OnInvoke = CopyToClipboard
        };

        // TODO: Copying images to the clipboard is still difficult in Avalonia
        // https://github.com/AvaloniaUI/Avalonia/issues/3588

        ContextMenuItem autoscale = new()
        {
            Label = "Autoscale",
            OnInvoke = Autoscale,
        };

        return new ContextMenuItem[] {
            saveImage,
            copyImage,
            autoscale,
        };
    }

    public ContextMenu GetContextMenu(Plot plot)
    {
        List<MenuItem> items = new();

        foreach (var contextMenuItem in ContextMenuItems)
        {
            if (contextMenuItem.IsSeparator)
            {
                items.Add(new MenuItem { Header = "-" });
            }
            else
            {
                var menuItem = new MenuItem { Header = contextMenuItem.Label };
                menuItem.Click += (s, e) => contextMenuItem.OnInvoke(plot);
                items.Add(menuItem);
            }
        }

        return new()
        {
            ItemsSource = items
        };
    }

    public async void OpenSaveImageDialog(Plot plot)
    {
        var topLevel = TopLevel.GetTopLevel(ThisControl) ?? throw new NullReferenceException("Could not find a top level");
        var destinationFile = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {
            SuggestedFileName = DefaultSaveImageFilename,
            FileTypeChoices = filePickerFileTypes
        });

        string? path = destinationFile?.TryGetLocalPath();
        if (path is not null && !string.IsNullOrWhiteSpace(path))
        {
            PixelSize lastRenderSize = plot.RenderManager.LastRender.FigureRect.Size;
            plot.Save(path, (int)lastRenderSize.Width, (int)lastRenderSize.Height, ImageFormats.FromFilename(path));
        }
    }

    public readonly List<FilePickerFileType> filePickerFileTypes = new()
    {
        new("PNG Files") { Patterns = new List<string> { "*.png" } },
        new("JPEG Files") { Patterns = new List<string> { "*.jpg", "*.jpeg" } },
        new("BMP Files") { Patterns = new List<string> { "*.bmp" } },
        new("WebP Files") { Patterns = new List<string> { "*.webp" } },
        new("SVG Files") { Patterns = new List<string> { "*.svg" } },
        new("All Files") { Patterns = new List<string> { "*" } },
    };

    public async void CopyToClipboard(Plot plot)
    {
        if (TopLevel.GetTopLevel(ThisControl)?.Clipboard is { } clipboard)
        {
            PixelSize lastRenderSize = plot.RenderManager.LastRender.FigureRect.Size;
            var img = plot.GetImage((int)lastRenderSize.Width, (int)lastRenderSize.Height);
            var bytes = img.GetImageBytes(ImageFormat.Bmp);
            
            IntPtr p = IntPtr.Zero;

            // Regrettably the Avalonia API takes only an IntPtr. So we just copy the bytes over
            // There is no safe API that allows you to get an IntPtr to a byte[] w/o copying
            // Not that Marshal.Copy is a safe API, but it doesn't require the unsafe keyword
            try
            {
                p = Marshal.AllocHGlobal(bytes.Length);
                Marshal.Copy(bytes, 0, p, bytes.Length);

                Bitmap bitmap = new Bitmap(
                    PixelFormat.Bgra8888, // This may cause problems on other platforms where SKImage isn't BGRA/ARGB
                    AlphaFormat.Unpremul, // Ditto
                    p,
                    new global::Avalonia.PixelSize((int)lastRenderSize.Width, (int)lastRenderSize.Height),
                    global::Avalonia.Vector.Zero, // (0, 0) for DPI means undefined
                    img.Width * 4 // We're already assuming a byte-order that's 32bpp
                );
                await clipboard.SetValueAsync(DataFormat.Bitmap, bitmap);
            }
            finally
            {
                Marshal.FreeHGlobal(p);
            }
        }
    }

    public void Autoscale(Plot plot)
    {
        plot.Axes.AutoScale();
        ThisControl.Refresh();
    }

    public void ShowContextMenu(Pixel pixel)
    {
        Plot? plot = ThisControl.GetPlotAtPixel(pixel);
        if (plot is null)
            return;
        var manualContextMenu = GetContextMenu(plot);

        // I am fully aware of how janky it is to place the menu in a 1x1 rect,
        // unfortunately the Avalonia docs were down when I wrote this
        manualContextMenu.PlacementRect = new(pixel.X, pixel.Y, 1, 1);

        manualContextMenu.Open(ThisControl);
    }

    public void Reset()
    {
        Clear();
        ContextMenuItems.AddRange(GetDefaultContextMenuItems());
    }

    public void Clear()
    {
        ContextMenuItems.Clear();
    }

    public void Add(string Label, Action<Plot> action)
    {
        ContextMenuItems.Add(new ContextMenuItem() { Label = Label, OnInvoke = action });
    }

    public void AddSeparator()
    {
        ContextMenuItems.Add(new ContextMenuItem() { IsSeparator = true });
    }
}
