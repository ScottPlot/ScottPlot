using System.Windows.Forms;
using System;
using System.Drawing;
using System.Collections.Generic;

namespace ScottPlot.WinForms;

public class FormsPlotMenu : IPlotMenu
{
    public string DefaultSaveImageFilename { get; set; } = "Plot.png";
    public List<ContextMenuItem> ContextMenuItems { get; set; } = new();
    readonly FormsPlotBase ThisControl;

    public FormsPlotMenu(FormsPlotBase control)
    {
        ThisControl = control;
        Reset();
    }

    public List<ContextMenuItem> StandardContextMenuItems()
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

        ContextMenuItem newWindow = new()
        {
            Label = "Open in New Window",
            OnInvoke = OpenInNewWindow,
        };

        return new List<ContextMenuItem>()
        {
            saveImage,
            copyImage,
            autoscale,
            newWindow,
        };
    }

    public void CopyImageToClipboard(Plot plot)
    {
        PixelSize lastRenderSize = plot.RenderManager.LastRender.FigureRect.Size;
        Bitmap bmp = plot.GetBitmap((int)lastRenderSize.Width, (int)lastRenderSize.Height);
        Clipboard.SetImage(bmp);
    }

    public void OpenInNewWindow(Plot plot)
    {
        FormsPlotViewer.Launch(plot, "Interactive Plot");
        ThisControl.Refresh();
    }

    public void Autoscale(Plot plot)
    {
        plot.Axes.AutoScale();
        ThisControl.Refresh();
    }

    public ContextMenuStrip GetContextMenu(Plot plot)
    {
        ContextMenuStrip menu = new();

        foreach (ContextMenuItem item in ContextMenuItems)
        {
            if (item.IsSeparator)
            {
                menu.Items.Add(new ToolStripSeparator());
            }
            else
            {
                ToolStripMenuItem menuItem = new(item.Label);
                menuItem.Click += (s, e) =>
                {
                    item.OnInvoke(plot);
                };

                menu.Items.Add(menuItem);
            }
        }

        return menu;
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

        if (dialog.ShowDialog() == DialogResult.OK)
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
                MessageBox.Show("Unsupported image file format", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                PixelSize lastRenderSize = plot.RenderManager.LastRender.FigureRect.Size;
                plot.Save(dialog.FileName, (int)lastRenderSize.Width, (int)lastRenderSize.Height, format);
            }
            catch (Exception)
            {
                MessageBox.Show("Image save failed", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }

    public void ShowContextMenu(Pixel pixel)
    {
        Plot? plot = ThisControl.GetPlotAtPixel(pixel);
        if (plot is null)
            return;
        ContextMenuStrip menu = GetContextMenu(plot);
        menu.Show(ThisControl, new Point((int)pixel.X, (int)pixel.Y));
    }

    public void Reset()
    {
        Clear();
        ContextMenuItems.AddRange(StandardContextMenuItems());
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
