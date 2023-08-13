using System.Windows.Controls;

namespace ScottPlot.WPF;

public partial class MultiWpfPlot : UserControl, IMultiplotControl
{
    public Multiplot Multiplot { get; } = new();

    public MultiWpfPlot()
    {
        InitializeComponent();
    }

    private void OnPaintSurface(object sender, SkiaSharp.Views.Desktop.SKPaintGLSurfaceEventArgs e)
    {
        float width = (float)e.Surface.Canvas.LocalClipBounds.Width;
        float height = (float)e.Surface.Canvas.LocalClipBounds.Height;
        PixelRect rect = new(0, width, height, 0);
        Multiplot.Render(e.Surface.Canvas, rect);
    }

    public void Refresh()
    {
        SKElement?.InvalidateVisual();
    }
}
