using ScottPlot.Control;
using System.Windows.Forms;
using System;
using System.Drawing;

namespace ScottPlot.WinForms;

public class Menu
{
    readonly UserControl ThisControl;

    public Menu(UserControl control)
    {
        ThisControl = control;
    }

    public ContextMenuItem[] StandardContextMenuItems()
    {
        ContextMenuItem saveImage = new()
        {
            Label = "Save Image",
            OnInvoke = OpenSaveImageDialog,
        };

        ContextMenuItem copyImage = new()
        {
            Label = "Copy to Clipboard",
            OnInvoke = CopyImageToClipboard
        };

        return new ContextMenuItem[] { saveImage, copyImage };
    }

    public void CopyImageToClipboard(IPlotControl plotControl)
    {
        PixelSize lastRenderSize = plotControl.Plot.RenderManager.LastRender.FigureRect.Size;
        Bitmap bmp = plotControl.Plot.GetBitmap((int)lastRenderSize.Width, (int)lastRenderSize.Height);
        Clipboard.SetImage(bmp);
    }

    public void OpenSaveImageDialog(IPlotControl plotControl)
    {
        SaveFileDialog dialog = new()
        {
            FileName = plotControl.Interaction.DefaultSaveImageFilename,
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
                format = ImageFormatLookup.FromFilePath(dialog.FileName);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Unsupported image file format", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                PixelSize lastRenderSize = plotControl.Plot.RenderManager.LastRender.FigureRect.Size;
                plotControl.Plot.Save(dialog.FileName, (int)lastRenderSize.Width, (int)lastRenderSize.Height, format);
            }
            catch (Exception)
            {
                MessageBox.Show("Image save failed", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }
}
