using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ScottPlot.Control.EventProcess
{
    /// <summary>
    /// Watches the queue for mouse events and processes them as they come in using low quality rendering.
    /// After a certain amount of time without new events, perform a high quality render and exit.
    /// </summary>
    public class EventsProcessor
    {
        private Queue<IUIEvent> queue = new Queue<IUIEvent>();
        private Stopwatch delayTimer = new Stopwatch();
        private bool AnyEventProcessed = false;
        private bool busy = false;
        public int HQRenderDelay { get; set; }
        public Action<bool> Render { get; set; }

        public EventsProcessor(Action<bool> Render, int hqRenderDelay)
        {
            this.Render = Render;
            this.HQRenderDelay = hqRenderDelay;
        }

        public async Task Process(IUIEvent uiEvent)
        {
            queue.Enqueue(uiEvent);
            if (!busy)
                await RunProcessor();
        }

        private void EventPostProcess(IUIEvent uiEvent)
        {
            if (uiEvent.RenderOrder == RenderType.HQAfterLQDelayed)
            {
                if (delayTimer.IsRunning)
                    delayTimer.Restart();
            }
            else
            {
                if (delayTimer.IsRunning)
                    delayTimer.Stop();
            }
            if (uiEvent.RenderOrder != RenderType.None)
                AnyEventProcessed = true;
        }

        private bool RenderPostProcess(IUIEvent lastEvent)
        {
            switch (lastEvent.RenderOrder)
            {
                case RenderType.LQOnly:
                    Debug.WriteLine($"hq Render scipped");
                    return true;
                case RenderType.HQOnly:
                    Render(true);
                    Debug.WriteLine($"hq Render call");
                    return true;
                case RenderType.HQAfterLQImmediately:
                    Render(true);
                    Debug.WriteLine($"Immediately hq Render call");
                    return true;
                case RenderType.HQAfterLQDelayed:
                    {
                        if (!delayTimer.IsRunning)
                            delayTimer.Restart();

                        if (delayTimer.ElapsedMilliseconds > 500)
                        {
                            Render(true);
                            Debug.WriteLine($"Delayed hq Render call");
                            return true;
                        }
                        return false;
                    }
                default:
                    throw new ArgumentException(message: $"Unexpected RenderType value: {lastEvent.RenderOrder}");
            }
        }

        private async Task RunProcessor()
        {
            bool RequestExit = false;
            busy = true;
            IUIEvent lastEvent = null;
            do
            {
                AnyEventProcessed = false;
                while (queue.Count > 0)
                {
                    var uiEvent = queue.Dequeue();
                    uiEvent.ProcessEvent();
                    EventPostProcess(uiEvent);
                    if (uiEvent.RenderOrder != RenderType.None)
                        lastEvent = uiEvent;
                    Debug.WriteLine($"eventProcessed {lastEvent}, elapsed:{queue.Count}");
                }

                if (AnyEventProcessed && lastEvent.RenderOrder != RenderType.HQOnly)
                {
                    Render(false);
                    Debug.WriteLine("lqRender run");
                }
                await Task.Delay(10);
                Debug.WriteLine($"{queue.Count} events in queue");

                RequestExit = false;
                if (queue.Count == 0) // avoid HQ rendering if new requests arrived after lq Render
                    RequestExit = RenderPostProcess(lastEvent);

            } while (!RequestExit || queue.Count > 0);
            busy = false;
        }
    }
}
