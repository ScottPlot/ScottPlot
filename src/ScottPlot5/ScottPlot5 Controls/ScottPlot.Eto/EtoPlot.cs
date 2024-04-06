using Eto.Forms;
using System;
using System.Collections.Generic;
using ScottPlot.Control;
using SkiaSharp;
using Eto.Drawing;
using System.Runtime.InteropServices;
using System.IO;

namespace ScottPlot.Eto;

public class EtoPlot : Drawable, IPlotControl
{
    public Plot Plot { get; internal set; } = new();

    public GRContext? GRContext => null;

    public IPlotInteraction Interaction { get; set; }

    public IPlotMenu Menu { get; set; }

    public float DisplayScale { get; set; }

    public EtoPlot()
    {
        DisplayScale = DetectDisplayScale();
        Interaction = new Interaction(this);
        Menu = new EtoPlotMenu(this);

        this.MouseDown += OnMouseDown;
        this.MouseUp += OnMouseUp;
        this.MouseMove += OnMouseMove;
        this.MouseWheel += OnMouseWheel;
        this.KeyDown += OnKeyDown;
        this.KeyUp += OnKeyUp;
        this.MouseDoubleClick += OnDoubleClick;
        this.SizeChanged += OnSizeChanged;
    }

    public void Reset()
    {
        Plot plot = new();
        Reset(plot);
    }

    public void Reset(Plot plot)
    {
        Plot oldPlot = Plot;
        Plot = plot;
        oldPlot?.Dispose();
    }

    public void Refresh()
    {
        Invalidate();
    }

    public void ShowContextMenu(Pixel position)
    {
        Menu.ShowContextMenu(position);
    }

    protected override void OnPaint(PaintEventArgs args)
    {
        base.OnPaint(args);

        SKImageInfo imageInfo = new((int)Bounds.Width, (int)Bounds.Height);

        using var surface = SKSurface.Create(imageInfo);
        if (surface is null)
            return;

        Plot.Render(surface.Canvas, (int)surface.Canvas.LocalClipBounds.Width, (int)surface.Canvas.LocalClipBounds.Height);

        SKImage img = surface.Snapshot();
        SKPixmap pixels = img.ToRasterImage().PeekPixels();
        byte[] bytes = pixels.GetPixelSpan().ToArray();

        var bmp = new Bitmap((int)Bounds.Width, (int)Bounds.Height, PixelFormat.Format32bppRgba);

        using (var data = bmp.Lock())
        {
            Marshal.Copy(bytes, 0, data.Data, bytes.Length);
        }

        args.Graphics.DrawImage(bmp, 0, 0);
    }

    private void OnMouseDown(object? sender, MouseEventArgs e)
    {
        Focus();

        Interaction.MouseDown(e.Pixel(), e.ToButton());
    }

    private void OnMouseUp(object? sender, MouseEventArgs e)
    {
        Interaction.MouseUp(e.Pixel(), e.ToButton());
    }

    private void OnMouseMove(object? sender, MouseEventArgs e)
    {
        Interaction.OnMouseMove(e.Pixel());
    }

    private void OnMouseWheel(object? sender, MouseEventArgs e)
    {
        Interaction.MouseWheelVertical(e.Pixel(), e.Delta.Height);
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        Interaction.KeyDown(e.Key());
    }

    private void OnKeyUp(object? sender, KeyEventArgs e)
    {
        Interaction.KeyUp(e.Key());
    }

    private void OnDoubleClick(object? sender, MouseEventArgs e)
    {
        Interaction.DoubleClick();
    }

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        Refresh();
    }

    public float DetectDisplayScale()
    {
        // TODO: improve support for DPI scale detection
        // https://github.com/ScottPlot/ScottPlot/issues/2760
        return 1.0f;
    }
}
