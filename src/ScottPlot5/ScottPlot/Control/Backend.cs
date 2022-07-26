namespace ScottPlot.Control
{
    /// <summary>
    /// This class is intended to be instantiated in user controls
    /// and provides a convenient way to manage events and map events to actions.
    /// </summary>
    public class Backend
    {
        /// <summary>
        /// This configuration object defines which buttons and keys perform specific actions
        /// </summary>
        public InputBindings InputBindings { get; set; } = InputBindings.Standard();

        private Plot Plot => Control.Plot;

        private readonly IPlotControl Control;

        private readonly KeyboardState Keyboard = new();

        private readonly MouseState Mouse = new();

        private Pixel MousePosition = new(float.NaN, float.NaN);

        private double ZoomInFraction { get; set; } = 1.15;

        private double ZoomOutFraction { get; set; } = 0.85;

        private double PanFraction { get; set; } = 0.1;

        /// <summary>
        /// Create a backend for a user control to manage interaction and event handling.
        /// </summary>
        /// <param name="plotControl">The control whose plot is being controlled by this backend</param>
        public Backend(IPlotControl control)
        {
            Control = control;
        }

        /// <summary>
        /// Latest position of the mouse (in coordinate units)
        /// </summary>
        public virtual Coordinates GetMouseCoordinates()
        {
            return Plot.GetCoordinate(MousePosition);
        }

        public virtual void MouseDown(Pixel position, MouseButton button)
        {
            Mouse.Down(position, button, Plot.GetAxisLimits());
        }

        public virtual void MouseUp(Pixel position, MouseButton button)
        {
            bool drag = Mouse.IsDragging(position);

            // TODO: when should the rectangle be cleared?

            if (drag)
            {
                if (InputBindings.ShouldZoomRectangle(button, Keyboard.PressedKeys) && Plot.ZoomRectangle.IsVisible)
                {
                    Plot.MouseZoomRectangleClear(applyZoom: true);
                    Control.Refresh();
                }
            }

            Mouse.Up(button);

            if (button == InputBindings.DragZoomRectangleButton)
            {
                if (!drag)
                {
                    Plot.MouseZoomRectangleClear(applyZoom: false);
                    Plot.AutoScale();
                }
                Control.Refresh();
            }
        }

        public virtual void MouseMove(Pixel newPosition)
        {
            MousePosition = newPosition;

            if (Plot.ZoomRectangle.IsVisible)
            {
                Plot.MouseZoomRectangleClear(applyZoom: false);
            }

            if (Mouse.PressedButtons.Any() && Mouse.IsDragging(newPosition))
            {
                MouseDrag(
                    from: Mouse.MouseDownPosition,
                    to: newPosition,
                    button: Mouse.PressedButtons.First(),
                    keys: Keyboard.PressedKeys,
                    start: Mouse.MouseDownAxisLimits);
            }
        }

        public virtual void MouseDrag(Pixel from, Pixel to, MouseButton button, IEnumerable<Key> keys, AxisLimits start)
        {
            bool lockedY = InputBindings.ShouldLockY(keys);
            bool lockedX = InputBindings.ShouldLockX(keys);

            Pixel to2 = new(
                x: lockedX ? from.X : to.X,
                y: lockedY ? from.Y : to.Y);

            if (InputBindings.ShouldZoomRectangle(button, keys))
            {
                Plot.MouseZoomRectangle(from, to, vSpan: lockedY, hSpan: lockedX);
            }
            else if (button == InputBindings.DragPanButton)
            {
                Plot.MousePan(start, from, to2);
            }
            else if (button == InputBindings.DragZoomButton)
            {
                Plot.MouseZoom(start, from, to2);
            }

            Control.Refresh();
        }

        public virtual void DoubleClick()
        {
            Plot.Benchmark.IsVisible = !Plot.Benchmark.IsVisible;
            Control.Refresh();
        }

        public virtual void MouseWheelVertical(Pixel pixel, float delta)
        {
            MouseWheelDirection direction = delta > 0 ? MouseWheelDirection.Up : MouseWheelDirection.Down;

            if (InputBindings.ZoomInWheelDirection.HasValue && InputBindings.ZoomInWheelDirection == direction)
            {
                ZoomIn(pixel, Keyboard.PressedKeys);
            }
            else if (InputBindings.ZoomOutWheelDirection.HasValue && InputBindings.ZoomOutWheelDirection == direction)
            {
                ZoomOut(pixel, Keyboard.PressedKeys);
            }
            else if (InputBindings.PanUpWheelDirection.HasValue && InputBindings.PanUpWheelDirection == direction)
            {
                PanVertically(true);
            }
            else if (InputBindings.PanDownWheelDirection.HasValue && InputBindings.PanDownWheelDirection == direction)
            {
                PanVertically(false);
            }
            else if (InputBindings.PanRightWheelDirection.HasValue && InputBindings.PanRightWheelDirection == direction)
            {
                PanHorizontally(true);
            }
            else if (InputBindings.PanLeftWheelDirection.HasValue && InputBindings.PanLeftWheelDirection == direction)
            {
                PanHorizontally(false);
            }
            else
            {
                return;
            }
        }

        public virtual void KeyDown(Key key)
        {
            if (!Keyboard.IsPressed(key))
            {
                Keyboard.Down(key);
                Control.Refresh();
            }
        }

        public virtual void KeyUp(Key key)
        {
            if (Keyboard.IsPressed(key))
            {
                Keyboard.Up(key);
                Control.Refresh();
            }
        }

        private void ZoomIn(Pixel pixel, IEnumerable<Key> keys)
        {
            double xFrac = InputBindings.ShouldLockX(keys) ? 1 : ZoomInFraction;
            double yFrac = InputBindings.ShouldLockY(keys) ? 1 : ZoomInFraction;

            Plot.MouseZoom(xFrac, yFrac, pixel);
            Control.Refresh();
        }

        private void ZoomOut(Pixel pixel, IEnumerable<Key> keys)
        {
            double xFrac = InputBindings.ShouldLockX(keys) ? 1 : ZoomOutFraction;
            double yFrac = InputBindings.ShouldLockY(keys) ? 1 : ZoomOutFraction;

            Plot.MouseZoom(xFrac, yFrac, pixel);
            Control.Refresh();
        }

        private void PanVertically(bool up)
        {
            AxisLimits limits = Plot.GetAxisLimits();
            double deltaY = limits.Rect.Height * PanFraction;
            Plot.SetAxisLimits(limits.WithPan(0, up ? deltaY : -deltaY));
            Control.Refresh();
        }

        private void PanHorizontally(bool right)
        {
            AxisLimits limits = Plot.GetAxisLimits();
            double deltaX = limits.Rect.Width * PanFraction;
            Plot.SetAxisLimits(limits.WithPan(right ? deltaX : -deltaX, 0));
            Control.Refresh();
        }
    }
}
