using ScottPlot.Interactivity;
using SkiaSharp;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ScottPlot.WinForms;

public abstract class FormsPlotBase : UserControl, IPlotControl
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public abstract GRContext GRContext { get; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public Plot Plot { get; internal set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public IMultiplot Multiplot { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public IPlotMenu? Menu { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public UserInputProcessor UserInputProcessor { get; }

    public float DisplayScale { get; set; }

    /// <summary>
    /// A design time alternative view is displayed for instances where the plot control
    /// is loaded inside Visual Studio and the SkiaSharp DLL cannot be properly loaded.
    /// </summary>
    protected bool IsDesignerAlternative { get; }

    public FormsPlotBase()
    {
        Plot = null!;
        Multiplot = null!;
        UserInputProcessor = null!;
        bool isDesignMode = Process.GetCurrentProcess().ProcessName == "devenv";

        try
        {
            Plot = new() { PlotControl = this };
            Multiplot = new Multiplot(Plot);
            DisplayScale = DetectDisplayScale();
            UserInputProcessor = new(this);
            Menu = new FormsPlotMenu(this);

            if (isDesignMode)
            {
                Plot.Title($"ScottPlot {Version.VersionString}");
            }
        }
        catch (Exception exception)
        {
            for (Exception? ex = exception; ex is not null; ex = ex.InnerException)
            {
                if (ex is DllNotFoundException dllNotFound && dllNotFound.Message.Contains("libSkiaSharp") && isDesignMode)
                {
                    IsDesignerAlternative = true;
                    FormsPlotDesignerAlternative altControl = new() { Dock = DockStyle.Fill };
                    Controls.Add(altControl);
                    return;
                }
            }

            throw;
        }
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
        Reset(plot, disposeOldPlot: true);
    }

    public void Reset(Plot plot, bool disposeOldPlot)
    {
        Plot oldPlot = Plot;
        Plot = plot;
        if (disposeOldPlot)
            oldPlot?.Dispose();
        Plot.PlotControl = this;
        UserInputProcessor.Reset();
        Multiplot.Reset(plot);
    }

    public void ShowContextMenu(Pixel position)
    {
        Menu?.ShowContextMenu(position);
    }

    internal void SKElement_MouseDown(object? sender, MouseEventArgs e)
    {
        UserInputProcessor.ProcessMouseDown(e);
        base.OnMouseDown(e);
    }

    internal void SKElement_MouseUp(object? sender, MouseEventArgs e)
    {
        UserInputProcessor.ProcessMouseUp(e);
        base.OnMouseUp(e);
    }

    internal void SKElement_MouseMove(object? sender, MouseEventArgs e)
    {
        UserInputProcessor.ProcessMouseMove(e);
        base.OnMouseMove(e);
    }

    internal void SKElement_DoubleClick(object? sender, EventArgs e)
    {
        base.OnDoubleClick(e);
    }

    internal void SKElement_MouseWheel(object? sender, MouseEventArgs e)
    {
        UserInputProcessor.ProcessMouseWheel(e);
        base.OnMouseWheel(e);
    }

    internal void SKElement_KeyDown(object? sender, KeyEventArgs e)
    {
        UserInputProcessor.ProcessKeyDown(e);
        base.OnKeyDown(e);
    }

    internal void SKControl_PreviewKeyDown(object? sender, PreviewKeyDownEventArgs e)
    {
        UserInputProcessor.ProcessKeyDown(e);
        base.OnPreviewKeyDown(e);
    }

    internal void SKElement_KeyUp(object? sender, KeyEventArgs e)
    {
        UserInputProcessor.ProcessKeyUp(e);
        base.OnKeyUp(e);
    }

    internal void SKElement_LostFocus(object? sender, System.EventArgs e)
    {
        FormsPlotExtensions.ProcessLostFocus(UserInputProcessor);
        base.OnLostFocus(e);
    }

    public float DetectDisplayScale()
    {
        using Graphics gfx = CreateGraphics();
        const int DEFAULT_DPI = 96;
        float ratio = gfx.DpiX / DEFAULT_DPI;
        return ratio;
    }
}
