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

        private Pixel? LastMousePosition = null;

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

        /// <summary>
        /// A click-drag must exceed this number of pixels before it is considered a drag.
        /// </summary>
        public float MinimumDragDistance = 5;

        /// <summary>
        /// Create a backend for a user control to manage interaction and event handling.
        /// </summary>
        /// <param name="sender">The type of the control whose plot is being controlled by this backend</param>
        public Backend(T sender)
        {
            Control = sender;

            IPlotInteractions interactions = new Default();
            MouseDown += (T sender, MouseDownEventArgs e) => interactions.MouseDown(sender, e);
            MouseUp += (T sender, MouseUpEventArgs e) => interactions.MouseUp(sender, e);
            MouseMove += (T sender, MouseMoveEventArgs e) => interactions.MouseMove(sender, e);
            MouseDrag += (T sender, MouseDragEventArgs e) => interactions.MouseDrag(sender, e);
            MouseDragEnd += (T sender, MouseDragEventArgs e) => interactions.MouseDragEnd(sender, e);
            DoubleClick += (T sender, MouseDownEventArgs e) => interactions.DoubleClick(sender, e);
            MouseWheel += (T sender, MouseWheelEventArgs e) => interactions.MouseWheel(sender, e);
            KeyDown += (T sender, KeyDownEventArgs e) => interactions.KeyDown(sender, e);
            KeyUp += (T sender, KeyUpEventArgs e) => interactions.KeyUp(sender, e);
        }

        /// <summary>
        /// Clear all existing interaction event handlers and replace them with a custom set
        /// </summary>
        public void ReplaceInteractions(IPlotInteractions interactions)
        {
            MouseDown = delegate { };
            MouseUp = delegate { };
            MouseMove = delegate { };
            MouseDrag = delegate { };
            MouseDragEnd = delegate { };
            DoubleClick = delegate { };
            MouseWheel = delegate { };
            KeyDown = delegate { };
            KeyUp = delegate { };

            MouseDown += (T sender, MouseDownEventArgs e) => interactions.MouseDown(sender, e);
            MouseUp += (T sender, MouseUpEventArgs e) => interactions.MouseUp(sender, e);
            MouseMove += (T sender, MouseMoveEventArgs e) => interactions.MouseMove(sender, e);
            MouseDrag += (T sender, MouseDragEventArgs e) => interactions.MouseDrag(sender, e);
            MouseDragEnd += (T sender, MouseDragEventArgs e) => interactions.MouseDragEnd(sender, e);
            DoubleClick += (T sender, MouseDownEventArgs e) => interactions.DoubleClick(sender, e);
            MouseWheel += (T sender, MouseWheelEventArgs e) => interactions.MouseWheel(sender, e);
            KeyDown += (T sender, KeyDownEventArgs e) => interactions.KeyDown(sender, e);
            KeyUp += (T sender, KeyUpEventArgs e) => interactions.KeyUp(sender, e);
        }

        public IReadOnlyCollection<Key> PressedKeys => CurrentlyPressedKeys.ToArray();

        public IEnumerable<MouseButton> PressedMouseButtons => MouseInteractions.Keys.Where(button => MouseInteractions[button] is not null);

        private MouseDownEventArgs? GetMouseInteraction(MouseButton button) => MouseInteractions.ContainsKey(button) ? MouseInteractions[button] : null;

        private void SetMouseInteraction(MouseButton button, MouseDownEventArgs? value) => MouseInteractions[button] = value;

        public Coordinate? MouseCoordinates => LastMousePosition.HasValue ? Control.Plot.GetCoordinate(LastMousePosition.Value) : null;

        public bool IsDrag(Pixel from, Pixel to) => (from - to).Hypotenuse > MinimumDragDistance;

        public void TriggerMouseDown(Pixel position, MouseButton button)
        {
            var interaction = new MouseDownEventArgs(position, button, Control.Plot.GetAxisLimits(), PressedKeys);
            SetMouseInteraction(button, interaction);
            MouseDown?.Invoke(Control, interaction);
        }

        public void TriggerDoubleClick(Pixel position, MouseButton button)
        {
            DoubleClick?.Invoke(Control, new(position, button, Control.Plot.GetAxisLimits(), PressedKeys));
        }

        public void TriggerMouseUp(Pixel position, MouseButton button)
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

        public void TriggerMouseMove(Pixel position)
        {
            LastMousePosition = position;
            MouseMove?.Invoke(Control, new(position, PressedKeys));

            for (MouseButton button = MouseButton.Mouse1; button <= MouseButton.Mouse3; button++)
            {
                var interaction = GetMouseInteraction(button);
                if (interaction is not null)
                {
                    var lastMouseDown = interaction;
                    if (IsDrag(lastMouseDown.Position, position))
                    {
                        TriggerMouseDrag(lastMouseDown, position, button);
                    }
                    else if (Control.Plot.ZoomRectangle.IsVisible)
                    {
                        Control.Plot.MouseZoomRectangleClear(applyZoom: false);
                    }
                }
            }
        }

        public void TriggerMouseWheel(Pixel position, float deltaX, float deltaY)
        {
            MouseWheel?.Invoke(Control, new(position, deltaX, deltaY));
        }

        public void TriggerKeyDown(Key key)
        {
            CurrentlyPressedKeys.Add(key);
            KeyDown?.Invoke(Control, new(key));
        }

        public void TriggerKeyUp(Key key)
        {
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
