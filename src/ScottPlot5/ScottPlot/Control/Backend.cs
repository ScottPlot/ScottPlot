using ScottPlot.Control.EventArgs;
using ScottPlot.Control.Interactions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ScottPlot.Control
{
    /// <summary>
    /// This class is intended to be instantiated in user controls
    /// and provides a convenient way to manage events and map events to actions.
    /// </summary>
    public class Backend
    {
        private readonly IPlotControl Control;

        private readonly HashSet<Key> CurrentlyPressedKeys = new();

        public Pixel MousePosition { get; private set; } = new(float.NaN, float.NaN);

        private delegate void MouseDownHandler(IPlotControl sender, MouseDownEventArgs eventArgs);
        private delegate void MouseUpHandler(IPlotControl sender, MouseUpEventArgs eventArgs);
        private delegate void MouseMoveHandler(IPlotControl sender, MouseMoveEventArgs eventArgs);
        private delegate void MouseDragHandler(IPlotControl sender, MouseDragEventArgs eventArgs);
        private delegate void MouseDragEndHandler(IPlotControl sender, MouseDragEventArgs eventArgs);
        private delegate void DoubleClickHandler(IPlotControl sender, MouseDownEventArgs eventArgs);
        private delegate void MouseWheelHandler(IPlotControl sender, MouseWheelEventArgs eventArgs);
        private delegate void KeyDownHandler(IPlotControl sender, KeyDownEventArgs eventArgs);
        private delegate void KeyUpHandler(IPlotControl sender, KeyUpEventArgs eventArgs);

        private event MouseDownHandler MouseDown = delegate { };
        private event MouseUpHandler MouseUp = delegate { };
        private event MouseMoveHandler MouseMove = delegate { };
        private event MouseDragHandler MouseDrag = delegate { };
        private event MouseDragEndHandler MouseDragEnd = delegate { };
        private event DoubleClickHandler DoubleClick = delegate { };
        private event MouseWheelHandler MouseWheel = delegate { };
        private event KeyDownHandler KeyDown = delegate { };
        private event KeyUpHandler KeyUp = delegate { };

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
            IInteractions defaultInteractions = new StandardInteractions(plotControl);
            SetInteractions(defaultInteractions);
        }

        /// <summary>
        /// Clear all existing interaction event handlers and replace them with a custom set
        /// </summary>
        public void SetInteractions(IInteractions interactions)
        {
            // TODO: see if there's a more elegant way to do this

            MouseDown = delegate { };
            MouseUp = delegate { };
            MouseMove = delegate { };
            MouseDrag = delegate { };
            MouseDragEnd = delegate { };
            DoubleClick = delegate { };
            MouseWheel = delegate { };
            KeyDown = delegate { };
            KeyUp = delegate { };

            MouseDown += (IPlotControl sender, MouseDownEventArgs e) => interactions.MouseDown(e);
            MouseUp += (IPlotControl sender, MouseUpEventArgs e) => interactions.MouseUp(e);
            MouseMove += (IPlotControl sender, MouseMoveEventArgs e) => interactions.MouseMove(e);
            MouseDrag += (IPlotControl sender, MouseDragEventArgs e) => interactions.MouseDrag(e);
            MouseDragEnd += (IPlotControl sender, MouseDragEventArgs e) => interactions.MouseDragEnd(e);
            DoubleClick += (IPlotControl sender, MouseDownEventArgs e) => interactions.DoubleClick(e);
            MouseWheel += (IPlotControl sender, MouseWheelEventArgs e) => interactions.MouseWheel(e);
            KeyDown += (IPlotControl sender, KeyDownEventArgs e) => interactions.KeyDown(e);
            KeyUp += (IPlotControl sender, KeyUpEventArgs e) => interactions.KeyUp(e);
        }

        private Dictionary<MouseButton, bool> NEW_MouseButtonsPressed = new()
        {
            { MouseButton.Mouse1, false },
            { MouseButton.Mouse2, false },
            { MouseButton.Mouse3, false },
            { MouseButton.UNKNOWN, false },
        };

        public void NEW_SetButtonDown(MouseButton button) => NEW_MouseButtonsPressed[button] = true;

        public void NEW_SetButtonUp(MouseButton button) => NEW_MouseButtonsPressed[button] = false;

        public bool NEW_IsButtonPressed(MouseButton button) => NEW_MouseButtonsPressed[button];

        private IReadOnlyCollection<Key> PressedKeys => CurrentlyPressedKeys.ToArray();

        public Coordinates MouseCoordinates => Control.Plot.GetCoordinate(MousePosition);

        private bool IsDrag(Pixel from, Pixel to) => (from - to).Hypotenuse > MinimumDragDistance;

        public void TriggerMouseDown(Pixel position, MouseButton button)
        {
            MouseDownPosition = position;
            MouseDownAxisLimits = Control.Plot.GetAxisLimits();
            NEW_SetButtonDown(button);

            MouseDownEventArgs interaction = new(position, button, MouseDownAxisLimits, PressedKeys);
            MouseDown?.Invoke(Control, interaction);
        }

        public void TriggerDoubleClick()
        {
            // TODO: dont need all this
            MouseDownEventArgs args = new(Pixel.NaN, MouseButton.UNKNOWN, AxisLimits.NoLimits, Array.Empty<Key>());
            DoubleClick?.Invoke(Control, args);
        }

        private Pixel MouseDownPosition;
        private AxisLimits MouseDownAxisLimits;

        public void TriggerMouseUp(Pixel position, MouseButton button)
        {
            bool cancelledDrag = false;

            if (!float.IsNaN(MouseDownPosition.X) && IsDrag(MouseDownPosition, position))
            {
                MouseDownEventArgs down = new(MouseDownPosition, button, MouseDownAxisLimits, PressedKeys);
                TriggerMouseDragEnd(down, position, button);
                cancelledDrag = true;
                MouseDownPosition = Pixel.NaN;
            }

            NEW_SetButtonUp(button);
            ClearPressedButtons();
            AxisLimits axisLimitsNow = Control.Plot.GetAxisLimits();
            MouseUp?.Invoke(Control, new(position, button, axisLimitsNow, cancelledDrag));
        }

        private MouseButton? GetPressedButton()
        {
            foreach (MouseButton button in NEW_MouseButtonsPressed.Keys)
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
            foreach (MouseButton button in NEW_MouseButtonsPressed.Keys)
            {
                NEW_MouseButtonsPressed[button] = false;
            }
        }

        public void TriggerMouseMove(Pixel newPosition)
        {
            MousePosition = newPosition;
            MouseMove?.Invoke(Control, new(newPosition, PressedKeys));

            MouseButton? button = GetPressedButton();
            if (button is null)
                return;

            if (IsDrag(MouseDownPosition, newPosition))
            {
                MouseDownEventArgs mouseDown = new(MouseDownPosition, button.Value, MouseDownAxisLimits, PressedKeys);
                TriggerMouseDrag(mouseDown, newPosition, button.Value);
            }
            else if (Control.Plot.ZoomRectangle.IsVisible)
            {
                Control.Plot.MouseZoomRectangleClear(applyZoom: false);
            }
        }

        public void TriggerMouseWheel(Pixel position, float deltaX, float deltaY)
        {
            MouseWheel?.Invoke(Control, new(position, deltaX, deltaY));
        }

        public void TriggerKeyDown(Key key)
        {
            if (key == Key.UNKNOWN)
                return;
            CurrentlyPressedKeys.Add(key);
            KeyDown?.Invoke(Control, new(key));
        }

        public void TriggerKeyUp(Key key)
        {
            if (key == Key.UNKNOWN)
                return;
            CurrentlyPressedKeys.Remove(key);
            KeyUp?.Invoke(Control, new(key));
        }

        private void TriggerMouseDrag(MouseDownEventArgs MouseDown, Pixel to, MouseButton button)
        {
            MouseDrag?.Invoke(Control, new(MouseDown, to, button));
        }

        private void TriggerMouseDragEnd(MouseDownEventArgs MouseDown, Pixel to, MouseButton button)
        {
            MouseDragEnd?.Invoke(Control, new(MouseDown, to, button));
        }
    }
}
