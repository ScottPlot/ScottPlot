using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ScottPlot.Control;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using Key = ScottPlot.Control.Key;
using MouseButton = ScottPlot.Control.MouseButton;

namespace ScottPlot.WPF
{
    /// <summary>
    /// Interaction logic for WpfPlot.xaml
    /// </summary>
    public partial class WpfPlot : UserControl
    {
        public ScottPlot.Plot Plot { get; }
        private readonly Backend backend;

        public WpfPlot()
        {
            InitializeComponent();
            Plot = new();
            backend = new(this, Plot, Refresh);
        }

        public void Refresh()
        {
            SKElement.InvalidateVisual();
        }

        public Coordinate? MouseCoordinates => backend.MouseCoordinates;

        private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            Plot.Render(e.Surface);
        }

        private (Pixel position, List<MouseButton> buttons) GetInputState(MouseEventArgs e)
        {
            var dpiScale = VisualTreeHelper.GetDpi(this);

            Pixel position = new((float)(e.GetPosition(this).X * dpiScale.DpiScaleX), (float)(e.GetPosition(this).Y * dpiScale.DpiScaleY));
            List<MouseButton> buttons = new();

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                buttons.Add(Control.MouseButton.Mouse1);
            }

            if (e.RightButton == MouseButtonState.Pressed)
            {
                buttons.Add(Control.MouseButton.Mouse2);
            }

            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                buttons.Add(Control.MouseButton.Mouse3);
            }

            return (position, buttons);
        }

        private Key? GetScottPlotKey(KeyEventArgs e)
        {
            var key = (e.Key == System.Windows.Input.Key.System ? e.SystemKey : e.Key); // WPF likes to snatch Alt, so we have to look to see if it's a system key

            if (key == System.Windows.Input.Key.LeftCtrl || key == System.Windows.Input.Key.RightCtrl)
            {
                return Key.Ctrl;
            }
            else if (key == System.Windows.Input.Key.LeftAlt || key == System.Windows.Input.Key.RightAlt)
            {
                return Key.Alt;
            }
            else if (key == System.Windows.Input.Key.LeftShift || key == System.Windows.Input.Key.RightShift)
            {
                return Key.Shift;
            }

            return null;
        }

        private void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Keyboard.Focus(this);
            (var position, var buttons) = GetInputState(e);

            var pressedButtons = backend.GetPressedButtons();
            foreach (var button in buttons.Where(b => !pressedButtons.Contains(b)))
            {
                backend.TriggerMouseDown(position, button);
            }

            (sender as UIElement)?.CaptureMouse();

            if (e.ClickCount == 2)
            {
                SendDoubleClick(position, buttons);
            }
        }

        private void OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            (var position, var buttons) = GetInputState(e);

            var pressedButtons = backend.GetPressedButtons();
            foreach (var button in pressedButtons.Where(b => !buttons.Contains(b)))
            {
                backend.TriggerMouseUp(position, button);
            }

            (sender as UIElement)?.ReleaseMouseCapture();
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            backend.TriggerMouseMove(GetInputState(e).position);
            base.OnMouseMove(e);
        }

        private void OnMouseWheel(object sender, MouseWheelEventArgs e)
        {
            backend.TriggerDoubleClick(GetInputState(e).position, Control.MouseButton.Mouse1);
            backend.TriggerMouseWheel(GetInputState(e).position, 0, e.Delta);
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            Key? key = GetScottPlotKey(e);

            if (key.HasValue)
            {
                backend.TriggerKeyDown(key.Value);
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            Key? key = GetScottPlotKey(e);

            if (key.HasValue)
            {
                backend.TriggerKeyUp(key.Value);
            }
        }

        private void SendDoubleClick(Pixel position, List<MouseButton> buttons)
        {
            foreach (var button in buttons)
            {
                backend.TriggerDoubleClick(position, button);
            }
        }
    }
}
