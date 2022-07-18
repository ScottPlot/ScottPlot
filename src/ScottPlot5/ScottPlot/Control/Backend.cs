using ScottPlot.Control.EventArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace ScottPlot.Control
{
    public class Backend
    {
        private MouseDownEventArgs? CurrentMouse1Down = null;
        private MouseDownEventArgs? CurrentMouse2Down = null;
        private MouseDownEventArgs? CurrentMouse3Down = null;

        private readonly HashSet<Key> CurrentlyPressedKeys = new();
        private Pixel? LastMousePosition = null;

        private readonly Plot plot;
        private readonly object EventSender;
        private readonly Action requestRender;

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

        public Backend(object sender, Plot plot, Action requestRender)
        {
            EventSender = sender;
            this.plot = plot;
            this.requestRender = requestRender;

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

        public IEnumerable<MouseButton> GetPressedButtons()
        {
            if (CurrentMouse1Down is not null)
                yield return MouseButton.Mouse1;
            if (CurrentMouse2Down is not null)
                yield return MouseButton.Mouse2;
            if (CurrentMouse3Down is not null)
                yield return MouseButton.Mouse3;
        }

        public IReadOnlyCollection<Key> PressedKeys => CurrentlyPressedKeys.ToArray();

        public Coordinate? MouseCoordinates => LastMousePosition.HasValue ? plot.GetCoordinate(LastMousePosition.Value) : null;

        private MouseDownEventArgs? GetMouseInteractionForButton(MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Mouse1:
                    return CurrentMouse1Down;
                case MouseButton.Mouse2:
                    return CurrentMouse2Down;
                case MouseButton.Mouse3:
                    return CurrentMouse3Down;
                default:
                    throw new ArgumentException($"Unknown button: {button}");
            }
        }

        private void SetMouseInteractionForButton(MouseButton button, MouseDownEventArgs? value)
        {
            switch (button)
            {
                case MouseButton.Mouse1:
                    CurrentMouse1Down = value;
                    break;
                case MouseButton.Mouse2:
                    CurrentMouse2Down = value;
                    break;
                case MouseButton.Mouse3:
                    CurrentMouse3Down = value;
                    break;
                default:
                    throw new ArgumentException($"Unknown button: {button}");
            }
        }

        public void TriggerMouseDown(Pixel position, MouseButton button)
        {
            var interaction = new MouseDownEventArgs(position, button, plot.GetAxisLimits(), PressedKeys);
            SetMouseInteractionForButton(button, interaction);
            MouseDown?.Invoke(EventSender, interaction);
        }

        public void TriggerDoubleClick(Pixel position, MouseButton button)
        {
            DoubleClick?.Invoke(EventSender, new(position, button, plot.GetAxisLimits(), PressedKeys));
        }


        public void TriggerMouseUp(Pixel position, MouseButton button)
        {
            bool cancelledDrag = false;
            var interaction = GetMouseInteractionForButton(button);
            if (interaction is not null && IsDrag(interaction.Position, position))
            {
                TriggerMouseDragEnd(interaction, position, button);
                cancelledDrag = true;
            }

            SetMouseInteractionForButton(button, null);
            MouseUp?.Invoke(EventSender, new(position, button, plot.GetAxisLimits(), cancelledDrag));
        }

        public void TriggerMouseMove(Pixel position)
        {
            LastMousePosition = position;
            MouseMove?.Invoke(EventSender, new(position, PressedKeys));

            for (MouseButton button = MouseButton.Mouse1; button <= MouseButton.Mouse3; button++)
            {
                var interaction = GetMouseInteractionForButton(button);
                if (interaction is not null)
                {
                    var lastMouseDown = interaction;
                    if (IsDrag(lastMouseDown.Position, position))
                    {
                        TriggerMouseDrag(lastMouseDown, position, button);
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

        public static bool IsDrag(Pixel from, Pixel to)
        {
            const int minDragDistance = 5;
            Pixel difference = from - to;

            return difference.X * difference.X + difference.Y * difference.Y > minDragDistance * minDragDistance;
        }
    }
}
