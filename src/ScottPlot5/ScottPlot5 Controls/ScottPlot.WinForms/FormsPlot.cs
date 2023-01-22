using ScottPlot.Control;
using ScottPlot.Extensions;
using SkiaSharp.Views.Desktop;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ScottPlot.WinForms;

public class FormsPlot : UserControl, IPlotControl
{
    readonly SKGLControl SKElement;

    public Plot Plot { get; private set; }

    public Interaction Interaction { get; private set; }

    public FormsPlot()
    {
        Interaction = new(this)
        {
            ContextMenuItems = GetDefaultContextMenuItems()
        };

        SKElement = new() { Dock = DockStyle.Fill, VSync = true };
        SKElement.PaintSurface += SKControl_PaintSurface;
        SKElement.MouseDown += SKElement_MouseDown;
        SKElement.MouseUp += SKElement_MouseUp;
        SKElement.MouseMove += SKElement_MouseMove;
        SKElement.DoubleClick += SKElement_DoubleClick;
        SKElement.MouseWheel += SKElement_MouseWheel;
        SKElement.KeyDown += SKElement_KeyDown;
        SKElement.KeyUp += SKElement_KeyUp;

        Controls.Add(SKElement);

        HandleDestroyed += (s, e) => SKElement.Dispose();

        Plot = Reset();

        // TODO: replace this with an annotation instead of title
        bool isDesignMode = Process.GetCurrentProcess().ProcessName == "devenv";
        Plot.Title.Label.Text = isDesignMode
            ? $"ScottPlot {Version.VersionString}"
            : string.Empty;
    }

    private ContextMenuItem[] GetDefaultContextMenuItems()
    {
        ContextMenuItem saveImage = new() { Label = "Save Image", OnInvoke = OpenSaveImageDialog };
        ContextMenuItem copyImage = new() { Label = "Copy to Clipboard", OnInvoke = CopyImageToClipboard };

        return new ContextMenuItem[] { saveImage, copyImage };
    }

    // ContextMenu isn't available on net6-windows, ContextMenuStrip is the more modern replacement
    private ContextMenuStrip GetContextMenu()
    {
        ContextMenuStrip menu = new();
        foreach (var curr in Interaction.ContextMenuItems)
        {
            var menuItem = new ToolStripMenuItem(curr.Label);
            menuItem.Click += (s, e) => curr.OnInvoke();

            menu.Items.Add(menuItem);
        }

        return menu;
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

    public override void Refresh()
    {
        SKElement.Invalidate();
        base.Refresh();
    }

    public void ShowContextMenu(Pixel position)
    {
        var menu = GetContextMenu();
        menu.Show(this, new System.Drawing.Point((int)position.X, (int)position.Y));
    }

    private void SKControl_PaintSurface(object? sender, SKPaintGLSurfaceEventArgs e)
    {
        Plot.Render(e.Surface);
    }

    private void SKElement_MouseDown(object? sender, MouseEventArgs e)
    {
        Interaction.MouseDown(e.Pixel(), e.Button());
        base.OnMouseDown(e);
    }

    private void SKElement_MouseUp(object? sender, MouseEventArgs e)
    {
        Interaction.MouseUp(e.Pixel(), e.Button());
        base.OnMouseUp(e);
    }

    private void SKElement_MouseMove(object? sender, MouseEventArgs e)
    {
        Interaction.OnMouseMove(e.Pixel());
        base.OnMouseMove(e);
    }

    private void SKElement_DoubleClick(object? sender, EventArgs e)
    {
        Interaction.DoubleClick();
        base.OnDoubleClick(e);
    }

    private void SKElement_MouseWheel(object? sender, MouseEventArgs e)
    {
        Interaction.MouseWheelVertical(e.Pixel(), e.Delta);
        base.OnMouseWheel(e);
    }

    private void SKElement_KeyDown(object? sender, KeyEventArgs e)
    {
        Interaction.KeyDown(e.Key());
        base.OnKeyDown(e);
    }

    private void SKElement_KeyUp(object? sender, KeyEventArgs e)
    {
        Interaction.KeyUp(e.Key());
        base.OnKeyUp(e);
    }

    private void OpenSaveImageDialog()
    {
        SaveFileDialog dialog = new()
        {
            FileName = "ScottPlot.png",
            Filter = "PNG Files (*.png)|*.png" +
                     "|JPEG Files (*.jpg, *.jpeg)|*.jpg;*.jpeg" +
                     "|BMP Files (*.bmp)|*.bmp" +
                     "|WebP Files (*.webp)|*.webp" +
                     "|All files (*.*)|*.*"
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            if (string.IsNullOrEmpty(dialog.FileName))
                return;

            var format = ImageFormatHelpers.FromFilePath(dialog.FileName);
            if (!format.HasValue)
                return;

            try
            {
                Plot.Save(dialog.FileName, format: format.Value);
            }
            catch (Exception)
            {
                // TODO: Not sure if we can meaningfully do anything except perhaps show an error dialog?
            }
        }
    }

    private void CopyImageToClipboard()
    {
        using var bmp = new System.Drawing.Bitmap(new MemoryStream(Plot.GetImage().GetImageBytes()));
        Clipboard.SetImage(bmp);
    }
}
