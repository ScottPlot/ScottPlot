using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using ScottPlot.Control;
using SkiaSharp;

namespace ScottPlot.WPF
{
    public abstract class WpfPlotBase : System.Windows.Controls.Control, IPlotControl
    {
        public abstract GRContext GRContext { get; }
        public abstract void Refresh();

        public Plot Plot { get; internal set; } = new();
        public IPlotInteraction Interaction { get; set; }
        public float DisplayScale { get; set; }
        public IPlotMenu Menu { get; set; }

        static WpfPlotBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                forType: typeof(WpfPlotBase),
                typeMetadata: new FrameworkPropertyMetadata(typeof(WpfPlotBase)));
        }

        public WpfPlotBase()
        {
            DisplayScale = DetectDisplayScale();
            Interaction = new Interaction(this);
            Menu = new WpfPlotMenu(this);
            Focusable = true;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            Interaction.KeyDown(e.Key());
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            Interaction.KeyUp(e.Key());
        }

        public void Reset()
        {
            Reset(new Plot());
        }

        public void Reset(Plot newPlot)
        {
            Plot oldPlot = Plot;
            Plot = newPlot;
            oldPlot?.Dispose();
        }

        public void ShowContextMenu(Pixel position)
        {
            Menu.ShowContextMenu(position);
        }

        internal void SKElement_MouseDown(object? sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Keyboard.Focus(this);

            Interaction.MouseDown(e.Pixel(this), e.ToButton());

            (sender as UIElement)?.CaptureMouse();

            if (e.ClickCount == 2)
            {
                Interaction.DoubleClick();
            }
        }

        internal void SKElement_MouseUp(object? sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Interaction.MouseUp(e.Pixel(this), e.ToButton());
            (sender as UIElement)?.ReleaseMouseCapture();
        }

        internal void SKElement_MouseMove(object? sender, MouseEventArgs e)
        {
            Interaction.OnMouseMove(e.Pixel(this));
        }

        internal void SKElement_MouseWheel(object? sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            Interaction.MouseWheelVertical(e.Pixel(this), e.Delta);
        }

        internal void SKElement_KeyDown(object? sender, KeyEventArgs e)
        {
            Interaction.KeyDown(e.Key());
        }

        internal void SKElement_KeyUp(object? sender, KeyEventArgs e)
        {
            Interaction.KeyUp(e.Key());
        }

        public float DetectDisplayScale()
        {
            return (float)VisualTreeHelper.GetDpi(this).DpiScaleX;
        }
    }
}
