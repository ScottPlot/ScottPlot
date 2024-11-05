using ScottPlot.Interactivity;
using SkiaSharp;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Windows.Forms;

namespace ScottPlot.WinForms;

#pragma warning disable CS0618 

public abstract class FormsPlotBase : UserControl, IPlotControl
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public abstract GRContext GRContext { get; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public Plot Plot { get; internal set; }

    [Obsolete("Deprecated. Use UserInputProcessor instead. See ScottPlot.NET demo and FAQ for usage details.")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    [Browsable(false)]
    public IPlotInteraction Interaction { get; set; }

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
        Interaction = null!;
        UserInputProcessor = null!;
        bool isDesignMode = Process.GetCurrentProcess().ProcessName == "devenv";

        try
        {
            Plot = new() { PlotControl = this };
            DisplayScale = DetectDisplayScale();
            Interaction = new Control.Interaction(this); // TODO: remove in an upcoming release
            UserInputProcessor = new(this);
            Menu = new FormsPlotMenu(this);
            Plot.Title(isDesignMode ? $"ScottPlot {Version.VersionString}" : string.Empty);
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
