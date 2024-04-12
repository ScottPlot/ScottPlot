using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.ComponentModel;
using System.Windows.Forms;

namespace ScottPlot.WinForms;

[ToolboxItem(true)]
#if NETFRAMEWORK
[DesignTimeVisible(false)]
#else
[DesignTimeVisible(true)]
#endif
public class FormsPlot : FormsPlotBase
{
    private SKControl? SKControl;

    public override GRContext GRContext => null!;

    public FormsPlot()
    {
        HandleCreated += (s, e) => SetupSKControl();
        HandleDestroyed += (s, e) => TeardownSKControl();
        SetupSKControl();
    }

    private void SetupSKControl()
    {
        TeardownSKControl();

        SKControl = new() { Dock = DockStyle.Fill };

        SKControl.PaintSurface += SKElement_PaintSurface;
        SKControl.MouseDown += SKElement_MouseDown;
        SKControl.MouseUp += SKElement_MouseUp;
        SKControl.MouseMove += SKElement_MouseMove;
        SKControl.DoubleClick += SKElement_DoubleClick;
        SKControl.MouseWheel += SKElement_MouseWheel;
        SKControl.KeyDown += SKElement_KeyDown;
        SKControl.KeyUp += SKElement_KeyUp;

        Controls.Add(SKControl);
    }

    private void TeardownSKControl()
    {
        if (SKControl is null)
            return;

        SKControl.PaintSurface -= SKElement_PaintSurface; ;
        SKControl.MouseDown -= SKElement_MouseDown;
        SKControl.MouseUp -= SKElement_MouseUp;
        SKControl.MouseMove -= SKElement_MouseMove;
        SKControl.DoubleClick -= SKElement_DoubleClick;
        SKControl.MouseWheel -= SKElement_MouseWheel;
        SKControl.KeyDown -= SKElement_KeyDown;
        SKControl.KeyUp -= SKElement_KeyUp;

        Controls.Remove(SKControl);

        if (!SKControl.IsDisposed)
            SKControl.Dispose();
    }

    private void SKElement_PaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        Plot.Render(e.Surface);
    }

    public override void Refresh()
    {
        SKControl?.Invalidate();
        base.Refresh();
    }
}
