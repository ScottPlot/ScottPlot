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
    /// <typeparam name="T">The specific type of user control using this backend</typeparam>
    public class Backend<T> where T : IPlotControl
    {
        private readonly T Control;

        private readonly HashSet<Key> CurrentlyPressedKeys = new();

        public Pixel MousePosition { get; private set; } = new(float.NaN, float.NaN);

        private readonly Dictionary<MouseButton, MouseDownEventArgs?> MouseInteractions = new();

        private delegate void MouseDownHandler<U, V>(U sender, V eventArgs);
        private delegate void MouseUpHandler<U, V>(U sender, V eventArgs);
        private delegate void MouseMoveHandler<U, V>(U sender, V eventArgs);
        private delegate void MouseDragHandler<U, V>(U sender, V eventArgs);
        private delegate void MouseDragEndHandler<U, V>(U sender, V eventArgs);
        private delegate void DoubleClickHandler<U, V>(U sender, V eventArgs);
        private delegate void MouseWheelHandler<U, V>(U sender, V eventArgs);
        private delegate void KeyDownHandler<U, V>(U sender, V eventArgs);
        private delegate void KeyUpHandler<U, V>(U sender, V eventArgs);

        private event MouseDownHandler<T, MouseDownEventArgs> MouseDown = delegate { };
        private event MouseUpHandler<T, MouseUpEventArgs> MouseUp = delegate { };
        private event MouseMoveHandler<T, MouseMoveEventArgs> MouseMove = delegate { };
        private event MouseDragHandler<T, MouseDragEventArgs> MouseDrag = delegate { };
        private event MouseDragEndHandler<T, MouseDragEventArgs> MouseDragEnd = delegate { };
        private event DoubleClickHandler<T, MouseDownEventArgs> DoubleClick = delegate { };
        private event MouseWheelHandler<T, MouseWheelEventArgs> MouseWheel = delegate { };
        private event KeyDownHandler<T, KeyDownEventArgs> KeyDown = delegate { };
        private event KeyUpHandler<T, KeyUpEventArgs> KeyUp = delegate { };

        // TODO: add a flag so once the distance is exceeded it is ignored when you return to it, 
        // otherwise it feels laggy when you drag the cursor in small circles.

        /// <summary>
        /// A click-drag must exceed this number of pixels before it is considered a drag.
        /// </summary>
        public float MinimumDragDistance = 5;

        /// <summary>
        /// Create a backend for a user control to manage interaction and event handling.
        /// </summary>
        /// <param name="plotControl">The type of the control whose plot is being controlled by this backend</param>
        public Backend(T plotControl)
        {
            Control = plotControl;
            SetInteractions(new Default(plotControl));
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

            MouseDown += (T sender, MouseDownEventArgs e) => interactions.MouseDown(e);
            MouseUp += (T sender, MouseUpEventArgs e) => interactions.MouseUp(e);
            MouseMove += (T sender, MouseMoveEventArgs e) => interactions.MouseMove(e);
            MouseDrag += (T sender, MouseDragEventArgs e) => interactions.MouseDrag(e);
            MouseDragEnd += (T sender, MouseDragEventArgs e) => interactions.MouseDragEnd(e);
            DoubleClick += (T sender, MouseDownEventArgs e) => interactions.DoubleClick(e);
            MouseWheel += (T sender, MouseWheelEventArgs e) => interactions.MouseWheel(e);
            KeyDown += (T sender, KeyDownEventArgs e) => interactions.KeyDown(e);
            KeyUp += (T sender, KeyUpEventArgs e) => interactions.KeyUp(e);
        }

        private IReadOnlyCollection<Key> PressedKeys => CurrentlyPressedKeys.ToArray();

        private IEnumerable<MouseButton> PressedMouseButtons => MouseInteractions.Keys.Where(button => MouseInteractions[button] is not null);

        private IEnumerable<MouseButton> NewlyPressedButtons(IEnumerable<MouseButton> buttonsNow) => buttonsNow.Where(button => !PressedMouseButtons.Contains(button));

        private IEnumerable<MouseButton> NewlyReleasedButtons(IEnumerable<MouseButton> buttonsNow) => PressedMouseButtons.Where(b => !buttonsNow.Contains(b));

        private MouseDownEventArgs? GetMouseInteraction(MouseButton button) => MouseInteractions.ContainsKey(button) ? MouseInteractions[button] : null;

        private void SetMouseInteraction(MouseButton button, MouseDownEventArgs? value) => MouseInteractions[button] = value;

        public Coordinates MouseCoordinates => Control.Plot.GetCoordinate(MousePosition);

        private bool IsDrag(Pixel from, Pixel to) => (from - to).Hypotenuse > MinimumDragDistance;

        private void TriggerMouseDown(Pixel position, MouseButton button)
        {
            var interaction = new MouseDownEventArgs(position, button, Control.Plot.GetAxisLimits(), PressedKeys);
            SetMouseInteraction(button, interaction);
            MouseDown?.Invoke(Control, interaction);
        }

        public void TriggerMouseDown(MouseInputState state)
        {
            foreach (MouseButton mouseButton in NewlyPressedButtons(state.ButtonsPressed))
            {
                TriggerMouseDown(state.Position, mouseButton);
            }
        }

        private void TriggerDoubleClick(Pixel position, MouseButton button)
        {
            DoubleClick?.Invoke(Control, new(position, button, Control.Plot.GetAxisLimits(), PressedKeys));
        }

        public void TriggerDoubleClick(MouseInputState state)
        {
            foreach (MouseButton button in state.ButtonsPressed)
            {
                TriggerDoubleClick(state.Position, button);
            }
        }

        public void TriggerMouseUp(MouseInputState state)
        {
            foreach (MouseButton button in NewlyReleasedButtons(state.ButtonsPressed))
            {
                TriggerMouseUp(state.Position, button);
            }
        }

        private void TriggerMouseUp(Pixel position, MouseButton button)
        {
            bool cancelledDrag = false;
            var interaction = GetMouseInteraction(button);
            if (interaction is not null && IsDrag(interaction.Position, position))
            {
                TriggerMouseDragEnd(interaction, position, button);
                cancelledDrag = true;
            }

            SetMouseInteraction(button, null);
            MouseUp?.Invoke(Control, new(position, button, Control.Plot.GetAxisLimits(), cancelledDrag));
        }

        private void TriggerMouseUp(Pixel position, List<MouseButton> buttons)
        {
            foreach (MouseButton button in NewlyReleasedButtons(buttons))
            {
                TriggerMouseUp(position, button);
            }
        }

        public void TriggerMouseMove(MouseInputState state)
        {
            MousePosition = state.Position;
            MouseMove?.Invoke(Control, new(state.Position, PressedKeys));

            for (MouseButton button = MouseButton.Mouse1; button <= MouseButton.Mouse3; button++)
            {
                var interaction = GetMouseInteraction(button);
                if (interaction is not null)
                {
                    var lastMouseDown = interaction;
                    if (IsDrag(lastMouseDown.Position, state.Position))
                    {
                        TriggerMouseDrag(lastMouseDown, state.Position, button);
                    }
                    else if (Control.Plot.ZoomRectangle.IsVisible)
                    {
                        Control.Plot.MouseZoomRectangleClear(applyZoom: false);
                    }
                }
            }
        }

        public void TriggerMouseWheel(MouseInputState state, float deltaX, float deltaY)
        {
            MouseWheel?.Invoke(Control, new(state.Position, deltaX, deltaY));
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
