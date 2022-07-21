using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ScottPlot.Control;
using SkiaSharp.Views.Desktop;
using Key = ScottPlot.Control.Key;
using MouseButton = ScottPlot.Control.MouseButton;

namespace ScottPlot.WPF
{
    /// <summary>
    /// Interaction logic for WpfPlot.xaml
    /// </summary>
    public partial class WpfPlot : UserControl, IPlotControl
    {
        public Plot Plot { get; } = new();

        public Backend Backend { get; private set; }

        public Coordinates MouseCoordinates => Backend.MouseCoordinates;

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

        private Pixel GetMousePosition(MouseEventArgs e)
        {
            DpiScale dpiScale = VisualTreeHelper.GetDpi(this);
            return new Pixel(
                    x: (float)(e.GetPosition(this).X * dpiScale.DpiScaleX),
                    y: (float)(e.GetPosition(this).Y * dpiScale.DpiScaleY));
        }

        private MouseButton GetMouseButtonPressed(MouseEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
                return MouseButton.Mouse3;
            else if (e.LeftButton == MouseButtonState.Pressed)
                return MouseButton.Mouse1;
            else if (e.RightButton == MouseButtonState.Pressed)
                return MouseButton.Mouse2;
            else
                return MouseButton.UNKNOWN;
        }

        private MouseButton GetMouseButtonReleased(MouseEventArgs e)
        {
            if (e.MiddleButton != MouseButtonState.Pressed)
                return MouseButton.Mouse3;
            else if (e.LeftButton != MouseButtonState.Pressed)
                return MouseButton.Mouse1;
            else if (e.RightButton != MouseButtonState.Pressed)
                return MouseButton.Mouse2;
            else
                return MouseButton.UNKNOWN;
        }

        private Key GetKey(KeyEventArgs e)
        {
            // WPF likes to snatch Alt, in which case we have to grab the system key value
            var key = e.Key == System.Windows.Input.Key.System ? e.SystemKey : e.Key;

            return key switch
            {
                System.Windows.Input.Key.LeftCtrl => Key.Ctrl,
                System.Windows.Input.Key.RightCtrl => Key.Ctrl,
                System.Windows.Input.Key.LeftAlt => Key.Alt,
                System.Windows.Input.Key.RightAlt => Key.Alt,
                System.Windows.Input.Key.LeftShift => Key.Shift,
                System.Windows.Input.Key.RightShift => Key.Shift,
                _ => Key.UNKNOWN,
            };
        }

        // TODO: should every On event call its base event???
        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.Focus(this);

            Backend.TriggerMouseDown(GetMousePosition(e), GetMouseButtonPressed(e));

            (sender as UIElement)?.CaptureMouse();

            if (e.ClickCount == 2)
            {
                Backend.TriggerDoubleClick();
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Backend.TriggerMouseUp(GetMousePosition(e), GetMouseButtonReleased(e));
            (sender as UIElement)?.ReleaseMouseCapture();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Backend.TriggerMouseMove(GetMousePosition(e));
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Backend.TriggerMouseWheel(GetMousePosition(e), e.Delta);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Backend.TriggerKeyDown(GetKey(e));
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Backend.TriggerKeyUp(GetKey(e));
        }
    }
}
