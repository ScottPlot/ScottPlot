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

        private MouseInputState GetMouseState(MouseEventArgs e)
        {
            var dpiScale = VisualTreeHelper.GetDpi(this);

            Pixel mousePosition = new(
                    x: (float)(e.GetPosition(this).X * dpiScale.DpiScaleX),
                    y: (float)(e.GetPosition(this).Y * dpiScale.DpiScaleY));

            List<MouseButton?> pressedButtons = new()
            {
                e.LeftButton == MouseButtonState.Pressed ? MouseButton.Mouse1 : null,
                e.RightButton == MouseButtonState.Pressed ? MouseButton.Mouse2 : null,
                e.MiddleButton == MouseButtonState.Pressed ? MouseButton.Mouse3 : null,
            };

            return new MouseInputState(mousePosition, pressedButtons);
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

            MouseInputState state = GetMouseState(e);

            Backend.TriggerMouseDown(state);

            (sender as UIElement)?.CaptureMouse();

            if (e.ClickCount == 2)
            {
                Backend.TriggerDoubleClick(MouseInputState.Empty);
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Backend.TriggerMouseUp(GetMouseState(e));
            (sender as UIElement)?.ReleaseMouseCapture();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Backend.TriggerMouseMove(GetMouseState(e));
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Backend.TriggerMouseWheel(GetMouseState(e), 0, e.Delta);
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
