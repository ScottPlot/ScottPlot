using System.Windows;
using SkiaSharp;

namespace ScottPlot.WPF;

[System.ComponentModel.ToolboxItem(false)]
[System.ComponentModel.DesignTimeVisible(true)]
[TemplatePart(Name = PART_SKElement, Type = typeof(SkiaSharp.Views.WPF.SKGLElement))]
public class WpfPlotGL : WpfPlotBase
{
    private const string PART_SKElement = "PART_SKElement";

    private SkiaSharp.Views.WPF.SKGLElement? SKElement;

    protected override FrameworkElement PlotFrameworkElement => SKElement!;
    public override GRContext GRContext => SKElement?.GRContext ?? GRContext.CreateGl();

    static WpfPlotGL()
    {
        DefaultStyleKeyProperty.OverrideMetadata(
            forType: typeof(WpfPlotGL),
            typeMetadata: new FrameworkPropertyMetadata(typeof(WpfPlotGL)));
    }


    public override void OnApplyTemplate()
    {
        SKElement = Template.FindName(PART_SKElement, this) as SkiaSharp.Views.WPF.SKGLElement;

        if (SKElement == null)
            return;

        SKElement.PaintSurface += (sender, e) =>
        {
            float width = (float)e.Surface.Canvas.LocalClipBounds.Width;
            float height = (float)e.Surface.Canvas.LocalClipBounds.Height;
            PixelRect rect = new(0, width, height, 0);
            Plot.Render(e.Surface.Canvas, rect);
        };

        SKElement.MouseDown += SKElement_MouseDown;
        SKElement.MouseUp += SKElement_MouseUp;
        SKElement.MouseMove += SKElement_MouseMove;
        SKElement.MouseWheel += SKElement_MouseWheel;
        SKElement.KeyDown += SKElement_KeyDown;
        SKElement.KeyUp += SKElement_KeyUp;
    }

    public override void Refresh()
    {
        if (!CheckAccess())
        {
            Dispatcher.BeginInvoke(Refresh);
            return;
        }

        SKElement?.InvalidateVisual();
    }

}
