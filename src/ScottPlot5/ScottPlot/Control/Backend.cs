using ScottPlot.Control.Interactions;

namespace ScottPlot.Control
{
    /// <summary>
    /// This class is intended to be instantiated in user controls
    /// and provides a convenient way to manage events and map events to actions.
    /// </summary>
    public class Backend
    {
        private readonly IPlotControl Control;

        public Pixel MousePosition { get; private set; } = new(float.NaN, float.NaN);
        public Coordinates MouseCoordinates => Control.Plot.GetCoordinate(MousePosition);

        private IInteractions Interactions;

        // TODO: add a flag so once the distance is exceeded it is ignored when you return to it, 
        // otherwise it feels laggy when you drag the cursor in small circles.

        /// <summary>
        /// A click-drag must exceed this number of pixels before it is considered a drag.
        /// </summary>
        public float MinimumDragDistance = 5;

        /// <summary>
        /// Create a backend for a user control to manage interaction and event handling.
        /// </summary>
        /// <param name="plotControl">The control whose plot is being controlled by this backend</param>
        public Backend(IPlotControl plotControl)
        {
            Control = plotControl;
            Interactions = new StandardInteractions(plotControl);
        }

        private Dictionary<MouseButton, bool> NEW_MouseButtonsPressed = new()
        {
            { MouseButton.Mouse1, false },
            { MouseButton.Mouse2, false },
            { MouseButton.Mouse3, false },
            { MouseButton.UNKNOWN, false },
        };

        // TODO: object to track button press state (including mouse down position)
        public void SetMouseButtonDown(MouseButton button) => NEW_MouseButtonsPressed[button] = true;
        public void SetMouseButtonUp(MouseButton button) => NEW_MouseButtonsPressed[button] = false;
        public bool IsMouseButtonPressed(MouseButton button) => NEW_MouseButtonsPressed[button];
        private bool IsDrag(Pixel from, Pixel to) => (from - to).Hypotenuse > MinimumDragDistance;

        private Pixel MouseDownPosition;

        private AxisLimits MouseDownAxisLimits;

        // TODO: object to track key press state

        private readonly HashSet<Key> CurrentlyPressedKeys = new();
        private IReadOnlyCollection<Key> PressedKeys => CurrentlyPressedKeys.ToArray();


        public void TriggerMouseDown(Pixel position, MouseButton button)
        {
            MouseDownPosition = position;
            MouseDownAxisLimits = Control.Plot.GetAxisLimits();
            SetMouseButtonDown(button);

            Interactions.MouseDown(position, button);
        }

        public void TriggerDoubleClick()
        {
            Interactions.DoubleClick();
        }

        public void TriggerMouseUp(Pixel position, MouseButton button)
        {
            bool endDrag = false;

            if (!float.IsNaN(MouseDownPosition.X) && IsDrag(MouseDownPosition, position))
            {
                Interactions.MouseDragEnd(button, PressedKeys);
                endDrag = true;
                MouseDownPosition = Pixel.NaN;
            }

            SetMouseButtonUp(button);
            ClearPressedButtons();
            Interactions.MouseUp(position, button, endDrag);
        }

        private MouseButton? GetPressedButton()
        {
            foreach (MouseButton button in NEW_MouseButtonsPressed.Keys.ToArray())
            {
                if (NEW_MouseButtonsPressed[button])
                {
                    return button;
                }
            }
            return null;
        }

        private void ClearPressedButtons()
        {
            foreach (MouseButton button in NEW_MouseButtonsPressed.Keys.ToArray())
            {
                NEW_MouseButtonsPressed[button] = false;
            }
        }

        public void TriggerMouseMove(Pixel newPosition)
        {
            MousePosition = newPosition;

            Interactions.MouseMove(newPosition);

            MouseButton? button = GetPressedButton();
            if (button is null)
                return;

            if (IsDrag(MouseDownPosition, newPosition))
            {
                Interactions.MouseDrag(MouseDownPosition, newPosition, button.Value, PressedKeys, MouseDownAxisLimits);
            }
            else if (Control.Plot.ZoomRectangle.IsVisible)
            {
                Control.Plot.MouseZoomRectangleClear(applyZoom: false);
            }
        }

        public void TriggerMouseWheel(Pixel position, float delta)
        {
            Interactions.MouseWheel(position, delta);
        }

        public void TriggerKeyDown(Key key)
        {
            if (key == Key.UNKNOWN)
                return;
            CurrentlyPressedKeys.Add(key);
            Interactions.KeyDown(key);
        }

        public void TriggerKeyUp(Key key)
        {
            if (key == Key.UNKNOWN)
                return;
            CurrentlyPressedKeys.Remove(key);
            Interactions.KeyUp(key);
        }
    }
}
