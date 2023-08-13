using SkiaSharp.Views.Desktop;
using System.Windows.Forms;

namespace ScottPlot.WinForms;

public class MultiFormsPlot : UserControl
{
    private readonly SKGLControl SKElement;
    public readonly Control.SubPlotCollection SubPlots = new();

    public MultiFormsPlot()
    {
        SKElement = new() { Dock = DockStyle.Fill, VSync = true };
        SKElement.PaintSurface += SKElement_PaintSurface;
        Controls.Add(SKElement);
    }

    public override void Refresh()
    {
        SKElement.Invalidate();
        base.Refresh();
    }

    private void SKElement_PaintSurface(object? sender, SKPaintGLSurfaceEventArgs e)
    {
        PixelRect rect = new(left: 0, right: SKElement.Width, bottom: SKElement.Height, top: 0);
        SubPlots.Render(e.Surface.Canvas, rect);
    }
}
