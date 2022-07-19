using ScottPlot.Control.EventArgs;
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
        private readonly HashSet<Key> CurrentlyPressedKeys = new();

        private Pixel? LastMousePosition = null;

        private readonly Dictionary<MouseButton, MouseDownEventArgs?> MouseInteractions = new();

        private readonly Plot Plot;
        private readonly object EventSender;
        private readonly Action RequestRender; // TODO: why is this here? So the backend itself can request a render?

        public delegate void MouseDownHandler(object sender, MouseDownEventArgs e);
        public delegate void MouseMoveHandler(object sender, MouseMoveEventArgs e);
        public delegate void MouseUpHandler(object sender, MouseUpEventArgs e);
        public delegate void MouseDragHandler(object sender, MouseDragEventArgs e);
        public delegate void MouseDragEndHandler(object sender, MouseDragEventArgs e);
        public delegate void DoubleClickHandler(object sender, MouseDownEventArgs e);
        public delegate void MouseWheelHandler(object sender, MouseWheelEventArgs e);
        public delegate void KeyDownHandler(object sender, KeyDownEventArgs e);
        public delegate void KeyUpHandler(object sender, KeyUpEventArgs e);

        public event MouseDownHandler MouseDown = delegate { };
        public event MouseMoveHandler MouseMove = delegate { };
        public event MouseUpHandler MouseUp = delegate { };
        public event MouseDragHandler MouseDrag = delegate { };
        public event DoubleClickHandler DoubleClick = delegate { };
        public event MouseWheelHandler MouseWheel = delegate { };
        public event MouseDragEndHandler MouseDragEnd = delegate { };
        public event KeyDownHandler KeyDown = delegate { };
        public event KeyUpHandler KeyUp = delegate { };

        public float MinimumDragDistance = 5;

        public Backend(object sender, Plot plot, Action requestRender)
        {
            EventSender = sender;
            Plot = plot;
            RequestRender = requestRender;

            MouseDown += (object sender, MouseDownEventArgs e) => DefaultEventHandlers.MouseDown(plot, e, requestRender);
            MouseUp += (object sender, MouseUpEventArgs e) => DefaultEventHandlers.MouseUp(plot, e, requestRender);
            MouseMove += (object sender, MouseMoveEventArgs e) => DefaultEventHandlers.MouseMove(plot, e, requestRender);
            MouseDrag += (object sender, MouseDragEventArgs e) => DefaultEventHandlers.MouseDrag(plot, e, requestRender);
            MouseDragEnd += (object sender, MouseDragEventArgs e) => DefaultEventHandlers.MouseDragEnd(plot, e, requestRender);
            DoubleClick += (object sender, MouseDownEventArgs e) => DefaultEventHandlers.DoubleClick(plot, e, requestRender);
            MouseWheel += (object sender, MouseWheelEventArgs e) => DefaultEventHandlers.MouseWheel(plot, e, requestRender);
            KeyDown += (object sender, KeyDownEventArgs e) => DefaultEventHandlers.KeyDown(plot, e, requestRender);
            KeyUp += (object sender, KeyUpEventArgs e) => DefaultEventHandlers.KeyUp(plot, e, requestRender);
        }

        public IReadOnlyCollection<Key> PressedKeys => CurrentlyPressedKeys.ToArray();

        public IEnumerable<MouseButton> PressedMouseButtons => MouseInteractions.Keys.Where(button => MouseInteractions[button] is not null);

        private MouseDownEventArgs? GetMouseInteraction(MouseButton button) => MouseInteractions.ContainsKey(button) ? MouseInteractions[button] : null;

        private void SetMouseInteraction(MouseButton button, MouseDownEventArgs? value) => MouseInteractions[button] = value;

        public Coordinate? MouseCoordinates => LastMousePosition.HasValue ? Plot.GetCoordinate(LastMousePosition.Value) : null;

        public bool IsDrag(Pixel from, Pixel to) => (from - to).Hypotenuse > MinimumDragDistance;

        public void TriggerMouseDown(Pixel position, MouseButton button)
        {
            var interaction = new MouseDownEventArgs(position, button, Plot.GetAxisLimits(), PressedKeys);
            SetMouseInteraction(button, interaction);
            MouseDown?.Invoke(EventSender, interaction);
        }

        public void TriggerDoubleClick(Pixel position, MouseButton button)
        {
            DoubleClick?.Invoke(EventSender, new(position, button, Plot.GetAxisLimits(), PressedKeys));
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
            MouseUp?.Invoke(EventSender, new(position, button, Plot.GetAxisLimits(), cancelledDrag));
        }

        public void TriggerMouseMove(Pixel position)
        {
            LastMousePosition = position;
            MouseMove?.Invoke(EventSender, new(position, PressedKeys));

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
                    else if (Plot.ZoomRectangle.IsVisible)
                    {
                        Plot.MouseZoomRectangleClear(applyZoom: false);
                    }
                }
            }
        }

        public void TriggerMouseWheel(Pixel position, float deltaX, float deltaY)
        {
            MouseWheel?.Invoke(EventSender, new(position, deltaX, deltaY));
        }

        public void TriggerKeyDown(Key key)
        {
            CurrentlyPressedKeys.Add(key);
            KeyDown?.Invoke(EventSender, new(key));
        }

        public void TriggerKeyUp(Key key)
        {
            CurrentlyPressedKeys.Remove(key);
            KeyUp?.Invoke(EventSender, new(key));
        }

        private void TriggerMouseDrag(MouseDownEventArgs MouseDown, Pixel to, MouseButton button)
        {
            MouseDrag?.Invoke(EventSender, new(MouseDown, to, button));
        }

        private void TriggerMouseDragEnd(MouseDownEventArgs MouseDown, Pixel to, MouseButton button)
        {
            MouseDragEnd?.Invoke(EventSender, new(MouseDown, to, button));
        }
    }
}
