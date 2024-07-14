using ScottPlot.Control;
using ScottPlot.Interactivity;
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

    public IPlotInteraction Interaction { get; set; }
    public IPlotMenu Menu { get; set; }
    public Interactivity.UserInputProcessor UserInputProcessor { get; }

    public float DisplayScale { get; set; }

    public FormsPlotBase()
    {
        Plot = new() { PlotControl = this };
        DisplayScale = DetectDisplayScale();
        Interaction = new Interaction(this);
        UserInputProcessor = new(Plot);
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
        Menu.ShowContextMenu(position);
    }

    internal void SKElement_MouseDown(object? sender, MouseEventArgs e)
    {
        // OLD
        Interaction.MouseDown(e.Pixel(), e.Button());

        // NEW
        UserInputProcessor.Process(e.ButtonDownAction());

        base.OnMouseDown(e);
    }

    internal void SKElement_MouseUp(object? sender, MouseEventArgs e)
    {
        // OLD
        Interaction.MouseUp(e.Pixel(), e.Button());

        // NEW
        UserInputProcessor.Process(e.ButtonUpAction());

        base.OnMouseUp(e);
    }

    internal void SKElement_MouseMove(object? sender, MouseEventArgs e)
    {
        // OLD
        Interaction.OnMouseMove(e.Pixel());

        // NEW
        UserInputProcessor.Process(e.MouseMoveAction());

        base.OnMouseMove(e);
    }

    internal void SKElement_DoubleClick(object? sender, EventArgs e)
    {
        // OLD
        Interaction.DoubleClick();

        // NEW - not needed because double clicks are inferred from MouseDown events

        base.OnDoubleClick(e);
    }

    internal void SKElement_MouseWheel(object? sender, MouseEventArgs e)
    {
        // OLD
        Interaction.MouseWheelVertical(e.Pixel(), e.Delta);

        // NEW
        UserInputProcessor.Process(e.MouseWheelAction());

        base.OnMouseWheel(e);
    }

    internal void SKElement_KeyDown(object? sender, KeyEventArgs e)
    {
        // OLD
        Interaction.KeyDown(e.Key());

        // NEW
        UserInputProcessor.Process(e.KeyDownAction());

        base.OnKeyDown(e);
    }

    internal void SKElement_KeyUp(object? sender, KeyEventArgs e)
    {
        // OLD
        Interaction.KeyUp(e.Key());

        // NEW
        UserInputProcessor.Process(e.KeyUpAction());

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
