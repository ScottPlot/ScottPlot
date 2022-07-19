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
        public Plot Plot { get; }

        public Backend<IPlotControl> Backend { get; private set; }

        public Coordinate MouseCoordinates => Backend.MouseCoordinates;

        public WpfPlot()
        {
            InitializeComponent();
            Plot = new();
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

        // TODO: should keyboard keypress state live in here?
        private InputState GetInputState(MouseEventArgs e)
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

            return new InputState(mousePosition, pressedButtons);
        }

        private Key GetScottPlotKey(KeyEventArgs e)
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

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.Focus(this);

            InputState state = GetInputState(e);

            Backend.TriggerMouseDown(state);

            (sender as UIElement)?.CaptureMouse();

            if (e.ClickCount == 2)
            {
                SendDoubleClick(state);
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            Backend.TriggerMouseUp(GetInputState(e));
            (sender as UIElement)?.ReleaseMouseCapture();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            Backend.TriggerMouseMove(GetInputState(e));
            base.OnMouseMove(e);
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            Backend.TriggerMouseWheel(GetInputState(e), 0, e.Delta);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Backend.TriggerKeyDown(GetScottPlotKey(e));
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Backend.TriggerKeyUp(GetScottPlotKey(e));
        }

        private void SendDoubleClick(InputState state)
        {
            Backend.TriggerDoubleClick(state);
        }
    }
}
