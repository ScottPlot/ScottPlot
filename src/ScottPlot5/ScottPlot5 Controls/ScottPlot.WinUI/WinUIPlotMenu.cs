using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;

namespace ScottPlot.WinUI;

public class WinUIPlotMenu : IPlotMenu
{
    public string DefaultSaveImageFilename { get; set; } = "Plot.png";
    public List<ContextMenuItem> ContextMenuItems { get; set; } = new();
    readonly WinUIPlot ThisControl;

    public WinUIPlotMenu(WinUIPlot thisControl)
    {
        ThisControl = thisControl;
        Reset();
    }

    public ContextMenuItem[] GetDefaultContextMenuItems()
    {
        ContextMenuItem saveImage = new()
        {
            Label = "Save Image",
            OnInvoke = OpenSaveImageDialog,
        };

        ContextMenuItem copyImage = new()
        {
            Label = "Copy to Clipboard",
            OnInvoke = CopyImageToClipboard,
        };

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

    public MenuFlyout GetContextMenu(Plot plot)
    {
        MenuFlyout menu = new();

        foreach (var curr in ContextMenuItems)
        {
            if (curr.IsSeparator)
            {
                menu.Items.Add(new MenuFlyoutSeparator());
            }
            else
            {
                var menuItem = new MenuFlyoutItem { Text = curr.Label };
                menuItem.Click += (s, e) => curr.OnInvoke(plot);
                menu.Items.Add(menuItem);
            }
        }

        return menu;
    }

    public async void OpenSaveImageDialog(Plot plot)
    {
        FileSavePicker dialog = new()
        {
            SuggestedFileName = DefaultSaveImageFilename
        };
        dialog.FileTypeChoices.Add("PNG Files", new List<string>() { ".png" });
        dialog.FileTypeChoices.Add("JPEG Files", new List<string>() { ".jpg", ".jpeg" });
        dialog.FileTypeChoices.Add("BMP Files", new List<string>() { ".bmp" });
        dialog.FileTypeChoices.Add("WebP Files", new List<string>() { ".webp" });
        dialog.FileTypeChoices.Add("SVG Files", new List<string>() { ".svg" });

        // https://github.com/microsoft/CsWinRT/blob/master/docs/interop.md#windows-sdk
        // TODO: launch a pop-up window or otherwise inform if AppWindow is not set before using save-dialog
        var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(ThisControl.AppWindow);
        WinRT.Interop.InitializeWithWindow.Initialize(dialog, hwnd);

        var file = await dialog.PickSaveFileAsync();

        if (file != null)
        {
            // TODO: launch a pop-up window indicating if extension is invalid or save failed
            ImageFormat format = ImageFormats.FromFilename(file.Name);
            PixelSize lastRenderSize = plot.RenderManager.LastRender.FigureRect.Size;
            plot.Save(file.Path, (int)lastRenderSize.Width, (int)lastRenderSize.Height, format);
        }
    }

    public void CopyImageToClipboard(Plot plot)
    {
        PixelSize lastRenderSize = plot.RenderManager.LastRender.FigureRect.Size;
        byte[] bytes = plot.GetImage((int)lastRenderSize.Width, (int)lastRenderSize.Height).GetImageBytes();

        var stream = new InMemoryRandomAccessStream();
        stream.AsStreamForWrite().Write(bytes);

        var content = new DataPackage();
        content.SetBitmap(RandomAccessStreamReference.CreateFromStream(stream));

        Clipboard.SetContent(content);
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
        MenuFlyout flyout = GetContextMenu(plot);
        Windows.Foundation.Point pt = new(pixel.X, pixel.Y);
        flyout.ShowAt(ThisControl, pt);
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
