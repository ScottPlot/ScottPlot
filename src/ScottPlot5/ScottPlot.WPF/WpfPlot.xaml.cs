using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ScottPlot.Control;
using SkiaSharp.Views.Desktop;

namespace ScottPlot.WPF
{
    /// <summary>
    /// Interaction logic for WpfPlot.xaml
    /// </summary>
    public partial class WpfPlot : UserControl, IPlotControl
    {
        public Plot Plot { get; } = new();

        // TODO: make internal?
        public Backend Backend { get; private set; }

        public InputMap ButtonMap
        {
            get => Backend.Interactions.InputMap;
            set => Backend.Interactions.InputMap = value;
        }

        public WpfPlot()
        {
            InitializeComponent();
            Backend = new(this);
        }

        public void Refresh()
        {
            SKElement.InvalidateVisual();
        }

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            Plot.Render(e.Surface);
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.Focus(this);

            Backend.MouseDown(e.Pixel(this), e.ToButton());

            (sender as UIElement)?.CaptureMouse();

            if (e.ClickCount == 2)
            {
                Backend.DoubleClick();
            }

            base.OnMouseDown(e);
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Backend.MouseUp(e.Pixel(this), e.ToButton());
            (sender as UIElement)?.ReleaseMouseCapture();
            base.OnMouseUp(e);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Backend.MouseMove(e.Pixel(this));
            base.OnMouseMove(e);
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Backend.MouseWheel(e.Pixel(this), e.Delta);
            base.OnMouseWheel(e);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Backend.KeyDown(e.Key());
            base.OnKeyDown(e);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Backend.KeyUp(e.Key());
            base.OnKeyUp(e);
        }
    }
}
