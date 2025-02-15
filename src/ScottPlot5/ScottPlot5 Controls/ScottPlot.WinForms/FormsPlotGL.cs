using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.ComponentModel;
using System.Windows.Forms;

namespace ScottPlot.WinForms;

[ToolboxItem(false)]
public class FormsPlotGL : FormsPlotBase
{
    public SKGLControl SKElement { get; } = null!;

    public override GRContext GRContext => SKElement.GRContext;

    public FormsPlotGL() : base()
    {
        if (IsDesignerAlternative)
        {
            return;
        }

        SKElement = new() { Dock = DockStyle.Fill, VSync = true };
        SKElement.PaintSurface += SKControl_PaintSurface;
        SKElement.MouseDown += SKElement_MouseDown;
        SKElement.MouseUp += SKElement_MouseUp;
        SKElement.MouseMove += SKElement_MouseMove;
        SKElement.DoubleClick += SKElement_DoubleClick;
        SKElement.MouseWheel += SKElement_MouseWheel;
        SKElement.KeyDown += SKElement_KeyDown;
        SKElement.KeyUp += SKElement_KeyUp;
        SKElement.LostFocus += SKElement_LostFocus;

        Controls.Add(SKElement);

        HandleDestroyed += (s, e) => SKElement.Dispose();
    }

    private void SKControl_PaintSurface(object? sender, SKPaintGLSurfaceEventArgs e)
    {
        Multiplot.Render(e.Surface);
    }

    public override void Refresh()
    {
        SKElement.Invalidate();
        base.Refresh();
    }
}
