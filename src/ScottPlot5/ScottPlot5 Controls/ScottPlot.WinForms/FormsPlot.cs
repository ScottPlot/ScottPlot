using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace ScottPlot.WinForms;

[ToolboxItem(true)]
public class FormsPlot : FormsPlotBase
{
    public SKControl? SKControl { get; private set; }

    public override GRContext GRContext => null!;

    public FormsPlot()
    {

#if NETFRAMEWORK
        // do not attempt renders inside visual studio at design time
        if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            return;
#endif

        HandleCreated += (s, e) => SetupSKControl();
        HandleDestroyed += (s, e) => TeardownSKControl();
        SetupSKControl();
        Plot.FigureBackground.Color = Color.FromColor(SystemColors.Control);
        Plot.DataBackground.Color = Colors.White;
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
