using ScottPlot.Control.Interactions;

namespace ScottPlot.Control
{
    /// <summary>
    /// This class is intended to be instantiated in user controls
    /// and provides a convenient way to manage events and map events to actions.
    /// </summary>
    public class Backend
    {
        /// <summary>
        /// The <see cref="Plot"/> this backend is supporting
        /// </summary>
        private readonly Plot Plot;

        /// <summary>
        /// This object is used to pair mouse interactions with plot actions.
        /// The default behavior is left-click-drag pan and right-click drag-zoom,
        /// but advanced users can use their own interaction system instead.
        /// </summary>
        public IInteractions Interactions;

        /// <summary>
        /// Stores which keys are pressed
        /// </summary>
        private readonly KeyboardState Keyboard = new();

        /// <summary>
        /// Stores which mouse buttons are pressed
        /// </summary>
        private readonly MouseState Mouse = new();

        /// <summary>
        /// Latest position of the mouse (in pixel units)
        /// </summary>
        public Pixel MousePosition { get; private set; } = new(float.NaN, float.NaN);

        /// <summary>
        /// Latest position of the mouse (in coordinate units)
        /// </summary>
        public Coordinates GetMouseCoordinates()
        {
            return Plot.GetCoordinate(MousePosition);
        }

        /// <summary>
        /// Create a backend for a user control to manage interaction and event handling.
        /// </summary>
        /// <param name="plotControl">The control whose plot is being controlled by this backend</param>
        public Backend(IPlotControl plotControl)
        {
            Plot = plotControl.Plot;
            Interactions = new StandardInteractions(plotControl);
        }

        public void MouseDown(Pixel position, MouseButton button)
        {
            Mouse.Down(position, button, Plot.GetAxisLimits());
            Interactions.MouseDown(position, button);
        }

        public void MouseUp(Pixel position, MouseButton button)
        {
            bool drag = Mouse.IsDragging(position);

            // TODO: only end if the drag button was released
            if (drag)
                Interactions.MouseDragEnd(button, Keyboard.PressedKeys);

            Mouse.Up(button);
            Interactions.MouseUp(position, button, drag);
        }

        public void MouseMove(Pixel newPosition)
        {
            MousePosition = newPosition;

            Interactions.MouseMove(newPosition);

            if (Plot.ZoomRectangle.IsVisible)
            {
                Plot.MouseZoomRectangleClear(applyZoom: false);
            }

            if (Mouse.PressedButtons.Any() && Mouse.IsDragging(newPosition))
            {
                Interactions.MouseDrag(
                    from: Mouse.MouseDownPosition,
                    to: newPosition,
                    button: Mouse.PressedButtons.First(),
                    keys: Keyboard.PressedKeys,
                    start: Mouse.MouseDownAxisLimits);
            }
        }

        public void DoubleClick()
        {
            Interactions.DoubleClick();
        }

        public void MouseWheelVertical(Pixel position, float delta)
        {
            MouseWheelDirection direction = delta > 0 ? MouseWheelDirection.Up : MouseWheelDirection.Down;
            Interactions.MouseWheel(position, direction, Keyboard.PressedKeys);
        }

        public void KeyDown(Key key)
        {
            if (!Keyboard.IsPressed(key))
            {
                Keyboard.Down(key);
                Interactions.KeyDown(key);
            }
        }

        public void KeyUp(Key key)
        {
            if (Keyboard.IsPressed(key))
            {
                Keyboard.Up(key);
                Interactions.KeyUp(key);
            }
        }
    }
}
