using Microsoft.Win32;
using System.Windows.Media.Imaging;
using System.Windows;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.IO;

namespace ScottPlot.WPF;

public class WpfPlotMenu : IPlotMenu
{
    public string DefaultSaveImageFilename { get; set; } = "Plot.png";
    public List<ContextMenuItem> ContextMenuItems { get; set; } = new();
    readonly WpfPlotBase ThisControl;

    public WpfPlotMenu(WpfPlotBase control)
    {
        ThisControl = control;
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

        ContextMenuItem autoscale = new()
        {
            Label = "Autoscale",
            OnInvoke = Autoscale,
        };

        ContextMenuItem newWindow = new()
        {
            Label = "Open in New Window",
            OnInvoke = OpenInNewWindow,
        };

        return new ContextMenuItem[]
        {
            saveImage,
            copyImage,
            autoscale,
            newWindow,
        };
    }

    public ContextMenu GetContextMenu(Plot plot)
    {
        ContextMenu menu = new();

        foreach (ContextMenuItem curr in ContextMenuItems)
        {
            if (curr.IsSeparator)
            {
                menu.Items.Add(new Separator());
            }
            else
            {
                MenuItem menuItem = new() { Header = curr.Label };
                menuItem.Click += (s, e) => curr.OnInvoke(plot);
                menu.Items.Add(menuItem);
            }
        }

        return menu;
    }

    public void ShowContextMenu(Pixel pixel)
    {
        Plot? plot = ThisControl.GetPlotAtPixel(pixel);
        if (plot is null)
            return;
        var menu = GetContextMenu(plot);
        menu.PlacementTarget = ThisControl;
        menu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
        menu.IsOpen = true;
    }

    public void OpenSaveImageDialog(Plot plot)
    {
        SaveFileDialog dialog = new()
        {
            FileName = DefaultSaveImageFilename,
            Filter = "PNG Files (*.png)|*.png" +
                     "|JPEG Files (*.jpg, *.jpeg)|*.jpg;*.jpeg" +
                     "|BMP Files (*.bmp)|*.bmp" +
                     "|WebP Files (*.webp)|*.webp" +
                     "|SVG Files (*.svg)|*.svg" +
                     "|All files (*.*)|*.*"
        };

        if (dialog.ShowDialog() is true)
        {
            if (string.IsNullOrEmpty(dialog.FileName))
                return;

            ImageFormat format;

            try
            {
                format = ImageFormats.FromFilename(dialog.FileName);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Unsupported image file format", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                PixelSize lastRenderSize = plot.RenderManager.LastRender.FigureRect.Size;
                plot.Save(dialog.FileName, (int)lastRenderSize.Width, (int)lastRenderSize.Height, format);
            }
            catch (Exception)
            {
                MessageBox.Show("Image save failed", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }

    public static void CopyImageToClipboard(Plot plot)
    {
        PixelSize lastRenderSize = plot.RenderManager.LastRender.FigureRect.Size;
        Image bmp = plot.GetImage((int)lastRenderSize.Width, (int)lastRenderSize.Height);
        byte[] bmpBytes = bmp.GetImageBytes();

        using MemoryStream ms = new();
        ms.Write(bmpBytes, 0, bmpBytes.Length);
        BitmapImage bmpImage = new();
        bmpImage.BeginInit();
        bmpImage.StreamSource = ms;
        bmpImage.EndInit();
        Clipboard.SetImage(bmpImage);
    }

    public void Autoscale(Plot plot)
    {
        plot.Axes.AutoScale();
        ThisControl.Refresh();
    }

    public void OpenInNewWindow(Plot plot)
    {
        WpfPlotViewer.Launch(plot, "Interactive Plot");
        ThisControl.Refresh();
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
