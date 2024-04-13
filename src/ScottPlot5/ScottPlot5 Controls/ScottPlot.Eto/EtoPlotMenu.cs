using Eto.Drawing;
using Eto.Forms;
using ScottPlot.Control;
using System;
using System.Collections.Generic;
using System.IO;

namespace ScottPlot.Eto;

public class EtoPlotMenu : IPlotMenu
{
    public string DefaultSaveImageFilename { get; set; } = "Plot.png";
    public List<ContextMenuItem> ContextMenuItems { get; set; } = new();
    readonly EtoPlot ThisControl;

    public EtoPlotMenu(EtoPlot etoPlot)
    {
        ThisControl = etoPlot;
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
            OnInvoke = CopyImageToClipboard
        };

        return new ContextMenuItem[] { saveImage, copyImage };
    }

    public ContextMenu GetContextMenu()
    {
        ContextMenu menu = new();
        foreach (var curr in ContextMenuItems)
        {
            if (curr.IsSeparator)
            {
                menu.Items.AddSeparator();
            }
            else
            {
                var menuItem = new ButtonMenuItem() { Text = curr.Label };
                menuItem.Click += (s, e) => curr.OnInvoke(ThisControl);
                menu.Items.Add(menuItem);
            }
        }

        return menu;
    }

    public readonly List<FileFilter> FileDialogFilters = new()
    {
        new() { Name = "PNG Files", Extensions = new string[] { "png" } },
        new() { Name = "JPEG Files", Extensions = new string[] { "jpg", "jpeg" } },
        new() { Name = "BMP Files", Extensions = new string[] { "bmp" } },
        new() { Name = "WebP Files", Extensions = new string[] { "webp" } },
        new() { Name = "SVG Files", Extensions = new string[] { "svg" } },
        new() { Name = "All Files", Extensions = new string[] { "*" } },
    };

    public void OpenSaveImageDialog(IPlotControl plotControl)
    {
        SaveFileDialog dialog = new()
        {
            FileName = DefaultSaveImageFilename
        };

        foreach (var curr in FileDialogFilters)
        {
            dialog.Filters.Add(curr);
        }

        if (dialog.ShowDialog(ThisControl) == DialogResult.Ok)
        {
            var filename = dialog.FileName;

            if (string.IsNullOrEmpty(filename))
                return;

            // Eto doesn't add the extension for you when you select a filter :/
            if (!Path.HasExtension(filename))
                filename += $".{dialog.CurrentFilter.Extensions[0]}";

            // TODO: launch a pop-up window indicating if extension is invalid or save failed
            ImageFormat format = ImageFormatLookup.FromFilePath(filename);
            PixelSize lastRenderSize = plotControl.Plot.RenderManager.LastRender.FigureRect.Size;
            plotControl.Plot.Save(filename, (int)lastRenderSize.Width, (int)lastRenderSize.Height, format);
        }
    }

    public void CopyImageToClipboard(IPlotControl plotControl)
    {
        PixelSize lastRenderSize = plotControl.Plot.RenderManager.LastRender.FigureRect.Size;
        byte[] bytes = plotControl.Plot.GetImage((int)lastRenderSize.Width, (int)lastRenderSize.Height).GetImageBytes();
        MemoryStream ms = new(bytes);
        using Bitmap bmp = new(ms);
        Clipboard.Instance.Image = bmp;
    }

    public void ShowContextMenu(Pixel pixel)
    {
        var menu = GetContextMenu();
        menu.Show(ThisControl, new Point((int)pixel.X, (int)pixel.Y));
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

    public void Add(string Label, Action<IPlotControl> action)
    {
        ContextMenuItems.Add(new ContextMenuItem() { Label = Label, OnInvoke = action });
    }

    public void AddSeparator()
    {
        ContextMenuItems.Add(new ContextMenuItem() { IsSeparator = true });
    }
}
