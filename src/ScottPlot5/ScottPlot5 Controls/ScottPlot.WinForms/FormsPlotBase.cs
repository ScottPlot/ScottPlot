using ScottPlot.Control;
using ScottPlot.Extensions;
using SkiaSharp;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ScottPlot.WinForms;

public abstract class FormsPlotBase : UserControl, IPlotControl
{
    public abstract GRContext GRContext { get; }

    public Plot Plot { get; internal set; }

    public Interaction Interaction { get; internal set; }

    public float DisplayScale { get; set; }

    public FormsPlotBase()
    {
        DisplayScale = DetectDisplayScale();

        Interaction = new(this)
        {
            ContextMenuItems = GetDefaultContextMenuItems()
        };

        Plot = Reset();

        // TODO: replace this with an annotation instead of title
        bool isDesignMode = Process.GetCurrentProcess().ProcessName == "devenv";
        Plot.Title(isDesignMode ? $"ScottPlot {Version.VersionString}" : string.Empty);
    }

    internal ContextMenuItem[] GetDefaultContextMenuItems()
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

    // make it so changing the background color of the control changes background color of the plot too
    public override System.Drawing.Color BackColor
    {
        get => base.BackColor;
        set
        {
            base.BackColor = value;
            if (Plot is not null)
                Plot.FigureBackground = value.ToColor();
        }
    }

    public Plot Reset()
    {
        Plot newPlot = new()
        {
            FigureBackground = this.BackColor.ToColor(),
        };

        return Reset(newPlot);
    }

    public Plot Reset(Plot newPlot)
    {
        Plot oldPlot = Plot;
        Plot = newPlot;
        oldPlot?.Dispose();
        return newPlot;
    }

    public void Replace(Interaction interaction)
    {
        Interaction = interaction;
    }

    public void ShowContextMenu(Pixel position)
    {
        ContextMenuStrip menu = Interaction.GetContextMenu();
        menu.Show(this, new System.Drawing.Point((int)position.X, (int)position.Y));
    }

    internal void SKElement_MouseDown(object? sender, MouseEventArgs e)
    {
        Interaction.MouseDown(e.Pixel(), e.Button());
        base.OnMouseDown(e);
    }

    internal void SKElement_MouseUp(object? sender, MouseEventArgs e)
    {
        Interaction.MouseUp(e.Pixel(), e.Button());
        base.OnMouseUp(e);
    }

    internal void SKElement_MouseMove(object? sender, MouseEventArgs e)
    {
        Interaction.OnMouseMove(e.Pixel());
        base.OnMouseMove(e);
    }

    internal void SKElement_DoubleClick(object? sender, EventArgs e)
    {
        Interaction.DoubleClick();
        base.OnDoubleClick(e);
    }

    internal void SKElement_MouseWheel(object? sender, MouseEventArgs e)
    {
        Interaction.MouseWheelVertical(e.Pixel(), e.Delta);
        base.OnMouseWheel(e);
    }

    internal void SKElement_KeyDown(object? sender, KeyEventArgs e)
    {
        Interaction.KeyDown(e.Key());
        base.OnKeyDown(e);
    }

    internal void SKElement_KeyUp(object? sender, KeyEventArgs e)
    {
        Interaction.KeyUp(e.Key());
        base.OnKeyUp(e);
    }

    internal void OpenSaveImageDialog()
    {
        SaveFileDialog dialog = new()
        {
            FileName = Interaction.DefaultSaveImageFilename,
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
                Plot.Save(dialog.FileName, Width, Height, format);
            }
            catch (Exception)
            {
                MessageBox.Show("Image save failed", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }
    }

    internal void CopyImageToClipboard()
    {
        Bitmap bmp = Plot.GetBitmap(Width, Height);
        Clipboard.SetImage(bmp);
    }

    public Coordinates GetCoordinates(Pixel px, IXAxis? xAxis = null, IYAxis? yAxis = null)
    {
        return Plot.GetCoordinates(px.X, px.Y, xAxis, yAxis);
    }

    public float DetectDisplayScale()
    {
        using Graphics gfx = CreateGraphics();
        const int DEFAULT_DPI = 96;
        float ratio = gfx.DpiX / DEFAULT_DPI;
        return ratio;
    }
}
