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

        public Interaction Interaction { get; private set; }

        public WpfPlot()
        {
            InitializeComponent();
            Interaction = new(this);
        }

        public void Replace(Interaction interaction)
        {
            Interaction = interaction;
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

            Interaction.MouseDown(e.Pixel(this), e.ToButton());

            (sender as UIElement)?.CaptureMouse();

            if (e.ClickCount == 2)
            {
                Interaction.DoubleClick();
            }

            base.OnMouseDown(e);
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Interaction.MouseUp(e.Pixel(this), e.ToButton());
            (sender as UIElement)?.ReleaseMouseCapture();
            base.OnMouseUp(e);
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Interaction.OnMouseMove(e.Pixel(this));
            base.OnMouseMove(e);
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Interaction.MouseWheelVertical(e.Pixel(this), e.Delta);
            base.OnMouseWheel(e);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Interaction.KeyDown(e.Key());
            base.OnKeyDown(e);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Interaction.KeyUp(e.Key());
            base.OnKeyUp(e);
        }
    }
}
