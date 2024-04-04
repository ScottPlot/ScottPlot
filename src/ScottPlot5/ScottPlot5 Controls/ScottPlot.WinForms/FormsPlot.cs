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
    readonly SKControl SKElement;

    public override GRContext GRContext => null!;

    public FormsPlot()
    {
        SKElement = new() { Dock = DockStyle.Fill };
        SKElement.PaintSurface += SKElement_PaintSurface; ;
        SKElement.MouseDown += SKElement_MouseDown;
        SKElement.MouseUp += SKElement_MouseUp;
        SKElement.MouseMove += SKElement_MouseMove;
        SKElement.DoubleClick += SKElement_DoubleClick;
        SKElement.MouseWheel += SKElement_MouseWheel;
        SKElement.KeyDown += SKElement_KeyDown;
        SKElement.KeyUp += SKElement_KeyUp;

        Controls.Add(SKElement);

        HandleDestroyed += (s, e) => {
            SKElement.Dispose();
            Plot.Dispose();
        };
    }

    private void SKElement_PaintSurface(object? sender, SKPaintSurfaceEventArgs e)
    {
        Plot.Render(e.Surface.Canvas, (int)e.Surface.Canvas.LocalClipBounds.Width, (int)e.Surface.Canvas.LocalClipBounds.Height);
    }

    public override void Refresh()
    {
        SKElement.Invalidate();
        base.Refresh();
    }
}
