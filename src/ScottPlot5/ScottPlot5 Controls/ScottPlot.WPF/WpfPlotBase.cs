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

        public Plot Plot { get; internal set; }

        public IPlotInteraction Interaction { get; internal set; }

        public float DisplayScale { get; set; }

        static WpfPlotBase()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                forType: typeof(WpfPlotBase),
                typeMetadata: new FrameworkPropertyMetadata(typeof(WpfPlotBase)));
        }
        public WpfPlotBase()
        {
            DisplayScale = DetectDisplayScale();

            Interaction = new Interaction(this)
            {
                ContextMenuItems = Menu.GetDefaultContextMenuItems()
            };

            Plot = Reset();

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

        public Plot Reset()
        {
            return Reset(new Plot());
        }

        public Plot Reset(Plot newPlot)
        {
            Plot oldPlot = Plot;
            Plot = newPlot;
            oldPlot?.Dispose();
            return newPlot;
        }

        public void Replace(IPlotInteraction interaction)
        {
            Interaction = interaction;
        }

        public void ShowContextMenu(Pixel position)
        {
            var menu = Interaction.GetContextMenu();
            menu.PlacementTarget = this;
            menu.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
            menu.IsOpen = true;
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

        public Coordinates GetCoordinates(Pixel px, IXAxis? xAxis = null, IYAxis? yAxis = null)
        {
            float x = px.X * DisplayScale;
            float y = px.Y * DisplayScale;
            return Plot.GetCoordinates(x, y, xAxis, yAxis);
        }

        public float DetectDisplayScale()
        {
            return (float)VisualTreeHelper.GetDpi(this).DpiScaleX;
        }
    }
}
