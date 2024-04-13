using System.Windows;
using SkiaSharp;

namespace ScottPlot.WPF
{
    [System.ComponentModel.ToolboxItem(true)]
    [System.ComponentModel.DesignTimeVisible(true)]
    [TemplatePart(Name = PART_SKElement, Type = typeof(SkiaSharp.Views.WPF.SKElement))]
    public class WpfPlot : WpfPlotBase
    {
        private const string PART_SKElement = "PART_SKElement";

        private SkiaSharp.Views.WPF.SKElement? SKElement;
        protected override FrameworkElement PlotFrameworkElement => SKElement!;

        public override GRContext GRContext => null!;

        static WpfPlot()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                forType: typeof(WpfPlot),
                typeMetadata: new FrameworkPropertyMetadata(typeof(WpfPlot)));
        }

        public override void OnApplyTemplate()
        {
            SKElement = Template.FindName(PART_SKElement, this) as SkiaSharp.Views.WPF.SKElement;

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
}
