using ScottPlot.Interactivity;
using SkiaSharp;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ScottPlot.WinForms;

#pragma warning disable CS0618 

public abstract class FormsPlotBase : UserControl, IPlotControl
{
    public abstract GRContext GRContext { get; }

    public Plot Plot { get; internal set; }

    [Obsolete("Deprecated. Use UserInputProcessor instead. See ScottPlot.NET demo and FAQ for usage details.")]
    public IPlotInteraction Interaction { get; set; }
    public IPlotMenu? Menu { get; set; }
    public UserInputProcessor UserInputProcessor { get; }

    public float DisplayScale { get; set; }

    public FormsPlotBase()
    {
        Plot = new() { PlotControl = this };
        DisplayScale = DetectDisplayScale();
        Interaction = new Control.Interaction(this); // TODO: remove in an upcoming release
        UserInputProcessor = new(this);
        Menu = new FormsPlotMenu(this);

        // TODO: replace this with an annotation instead of title
        bool isDesignMode = Process.GetCurrentProcess().ProcessName == "devenv";
        Plot.Title(isDesignMode ? $"ScottPlot {Version.VersionString}" : string.Empty);
    }

    // make it so changing the background color of the control changes background color of the plot too
    public override System.Drawing.Color BackColor
    {
        get => base.BackColor;
        set
        {
            base.BackColor = value;
            if (Plot is not null)
                Plot.FigureBackground.Color = Color.FromColor(value);
        }
    }

    public void Reset()
    {
        Plot plot = new();
        plot.FigureBackground.Color = Color.FromColor(BackColor);
        plot.DataBackground.Color = Colors.White;
        Reset(plot);
    }

    public void Reset(Plot plot)
    {
        Plot oldPlot = Plot;
        Plot = plot;
        oldPlot?.Dispose();
        Plot.PlotControl = this;
    }

    public void ShowContextMenu(Pixel position)
    {
        Menu?.ShowContextMenu(position);
    }

    internal void SKElement_MouseDown(object? sender, MouseEventArgs e)
    {
        Interaction.MouseDown(e.Pixel(), e.Button());
        UserInputProcessor.ProcessMouseDown(e);
        base.OnMouseDown(e);
    }

    internal void SKElement_MouseUp(object? sender, MouseEventArgs e)
    {
        Interaction.MouseUp(e.Pixel(), e.Button());
        UserInputProcessor.ProcessMouseUp(e);
        base.OnMouseUp(e);
    }

    internal void SKElement_MouseMove(object? sender, MouseEventArgs e)
    {
        Interaction.OnMouseMove(e.Pixel());
        UserInputProcessor.ProcessMouseMove(e);
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
        UserInputProcessor.ProcessMouseWheel(e);
        base.OnMouseWheel(e);
    }

    internal void SKElement_KeyDown(object? sender, KeyEventArgs e)
    {
        Interaction.KeyDown(e.Key());
        UserInputProcessor.ProcessKeyDown(e);
        base.OnKeyDown(e);
    }

    internal void SKElement_KeyUp(object? sender, KeyEventArgs e)
    {
        Interaction.KeyUp(e.Key());
        UserInputProcessor.ProcessKeyUp(e);
        base.OnKeyUp(e);
    }

    public float DetectDisplayScale()
    {
        using Graphics gfx = CreateGraphics();
        const int DEFAULT_DPI = 96;
        float ratio = gfx.DpiX / DEFAULT_DPI;
        return ratio;
    }
}
