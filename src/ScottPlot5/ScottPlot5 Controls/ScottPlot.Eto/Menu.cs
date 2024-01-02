using Eto.Drawing;
using Eto.Forms;
using ScottPlot.Control;
using System.Collections.Generic;
using System.IO;

namespace ScottPlot.Eto;

public class Menu
{
    readonly EtoPlot ThisControl;

    public Menu(EtoPlot etoPlot)
    {
        ThisControl = etoPlot;
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
            FileName = plotControl.Interaction.DefaultSaveImageFilename
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

}
