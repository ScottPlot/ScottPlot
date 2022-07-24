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

        private readonly KeyStates KeyStates = new();

        private readonly MouseButtonStates MouseButtonStates = new();

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
            MouseButtonStates.Down(position, button, Plot.GetAxisLimits());
            Interactions.MouseDown(position, button, KeyStates.PressedKeys);
        }

        public void MouseUp(Pixel position, MouseButton button)
        {
            bool drag = MouseButtonStates.IsDragging(position);

            if (drag)
                Interactions.MouseDragEnd(button, KeyStates.PressedKeys);

            MouseButtonStates.Clear();
            Interactions.MouseUp(position, button, drag);
        }

        public void MouseMove(Pixel newPosition)
        {
            MousePosition = newPosition;

            Interactions.MouseMove(newPosition);

            MouseButton? button = MouseButtonStates.GetPressedButton();
            if (button is null)
                return;

            if (MouseButtonStates.IsDragging(newPosition))
            {
                Interactions.MouseDrag(
                    from: MouseButtonStates.MouseDownPosition,
                    to: newPosition,
                    button: button.Value,
                    keys: KeyStates.PressedKeys,
                    start: MouseButtonStates.MouseDownAxisLimits);
            }
            else if (Plot.ZoomRectangle.IsVisible)
            {
                Plot.MouseZoomRectangleClear(applyZoom: false);
            }
        }

        public void DoubleClick()
        {
            Interactions.DoubleClick();
        }

        public void MouseWheel(Pixel position, float delta)
        {
            MouseButton button = delta > 0 ? MouseButton.WheelUp : MouseButton.WheelDown;
            Interactions.MouseDown(position, button, KeyStates.PressedKeys);
        }

        public void KeyDown(Key key)
        {
            KeyStates.Down(key);
            Interactions.KeyDown(key);
        }

        public void KeyUp(Key key)
        {
            KeyStates.Up(key);
            Interactions.KeyUp(key);
        }
    }
}
