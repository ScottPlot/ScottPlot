using Eto.Forms;
using SkiaSharp;
using Eto.Drawing;
using System.Runtime.InteropServices;
using System;

namespace ScottPlot.Eto;

#pragma warning disable CS0618 // disable obsolete warnings

public class EtoPlot : Drawable, IPlotControl
{
    public Plot Plot { get; internal set; }
    public GRContext? GRContext => null;

    [Obsolete("Deprecated. Use UserInputProcessor instead. See ScottPlot.NET demo and FAQ for usage details.")]
    public IPlotInteraction Interaction { get; set; }
    public Interactivity.UserInputProcessor UserInputProcessor { get; }
    public IPlotMenu? Menu { get; set; }
    public float DisplayScale { get; set; }

    public EtoPlot()
    {
        Plot = new() { PlotControl = this };
        DisplayScale = DetectDisplayScale();
        Interaction = new Control.Interaction(this); // TODO: remove in an upcoming release
        UserInputProcessor = new(this);
        Menu = new EtoPlotMenu(this);

        MouseDown += OnMouseDown;
        MouseUp += OnMouseUp;
        MouseMove += OnMouseMove;
        MouseWheel += OnMouseWheel;
        KeyDown += OnKeyDown;
        KeyUp += OnKeyUp;
        MouseDoubleClick += OnDoubleClick;
        SizeChanged += (s, e) => Refresh();
    }

    public void Reset()
    {
        Plot plot = new() { PlotControl = this };
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
        Menu?.ShowContextMenu(position);
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
        Interaction.MouseDown(e.Pixel(), e.OldToButton());
        UserInputProcessor.ProcessMouseDown(e);
    }

    private void OnMouseUp(object? sender, MouseEventArgs e)
    {
        Interaction.MouseUp(e.Pixel(), e.OldToButton());
        UserInputProcessor.ProcessMouseUp(e);
    }

    private void OnMouseMove(object? sender, MouseEventArgs e)
    {
        Interaction.OnMouseMove(e.Pixel());
        UserInputProcessor.ProcessMouseMove(e);
    }

    private void OnMouseWheel(object? sender, MouseEventArgs e)
    {
        Interaction.MouseWheelVertical(e.Pixel(), e.Delta.Height);
        UserInputProcessor.ProcessMouseWheel(e);
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        Interaction.KeyDown(e.OldToKey());
        UserInputProcessor.ProcessKeyDown(e);
    }

    private void OnKeyUp(object? sender, KeyEventArgs e)
    {
        Interaction.KeyUp(e.OldToKey());
        UserInputProcessor.ProcessKeyUp(e);
    }

    private void OnDoubleClick(object? sender, MouseEventArgs e)
    {
        Interaction.DoubleClick();
    }

    public float DetectDisplayScale()
    {
        // TODO: improve support for DPI scale detection
        // https://github.com/ScottPlot/ScottPlot/issues/2760
        return 1.0f;
    }
}
