using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ScottPlot.Control.EventProcess
{
    /// <summary>
    /// This UI event processor uses a queue to process incoming events.
    /// After processing events, this processor may initiate a render.
    /// This processor also contains logic to initiate delayed renders.
    /// </summary>
    public class EventsProcessor
    {
        private readonly Queue<IUIEvent> Queue = new Queue<IUIEvent>();
        private bool QueueHasUnprocessedEvents => Queue.Count > 0;
        private bool QueueIsEmpty => Queue.Count == 0;

        /// <summary>
        /// This timer is used for delayed rendering.
        /// It is restarted whenever an event is processed which requests a delayed render.
        /// </summary>
        private readonly Stopwatch RenderDelayTimer = new Stopwatch();

        /// <summary>
        /// This is true while the processor is processing events and/or waiting for a delayed render.
        /// </summary>
        private bool QueueProcessorIsRunning = false;

        /// <summary>
        /// Time to wait after a low-quality render to invoke a high quality render
        /// </summary>
        public int RenderDelayMilliseconds { get; set; }

        /// <summary>
        /// When a render is required this Action will be invoked.
        /// Its argument indicates whether low quality should be used.
        /// </summary>
        public Action<bool> RenderAction { get; set; }

        /// <summary>
        /// Create a processor to invoke renders in response to incoming events
        /// </summary>
        /// <param name="renderAction">Action to invoke to perform a render. Bool argument is LowQuality.</param>
        /// <param name="renderDelay">Milliseconds after low-quality render to re-render using high quality.</param>
        public EventsProcessor(Action<bool> renderAction, int renderDelay)
        {
            RenderAction = renderAction;
            RenderDelayMilliseconds = renderDelay;
        }

        /// <summary>
        /// Perform a high quality render.
        /// Call this instead of the action itself because this has better-documented arguments.
        /// </summary>
        private void RenderHighQuality() => RenderAction.Invoke(false);

        /// <summary>
        /// Perform a low quality render.
        /// Call this instead of the action itself because this has better-documented arguments.
        /// </summary>
        private void RenderLowQuality() => RenderAction.Invoke(true);

        /// <summary>
        /// Add an event to the queue and process it when it is ready
        /// </summary>
        public async Task Process(IUIEvent uiEvent)
        {
            Queue.Enqueue(uiEvent);

            // start a new processor only if one is not already running
            if (!QueueProcessorIsRunning)
                await RunQueueProcessor();
        }

        /// <summary>
        /// This is called just after a UI event is processed.
        /// If a UI event calls for a delayed render, this will restart the timer.
        /// </summary>
        private void EventPostProcess(IUIEvent uiEvent)
        {
            bool delayedRenderIsRequired = uiEvent.RenderOrder == RenderType.HQAfterLQDelayed;

            // TODO: is this the best place for this?
            if (delayedRenderIsRequired)
            {
                if (RenderDelayTimer.IsRunning)
                    RenderDelayTimer.Restart();
            }
            else
            {
                if (RenderDelayTimer.IsRunning)
                    RenderDelayTimer.Stop();
            }
        }

        /// <summary>
        /// An initial low quality render may have been recently executed already.
        /// Given the render type of the last event, perform a HQ render if needed.
        /// If a HQ render is needed after a delay, this method will arrange it.
        /// </summary>
        /// <param name="lastRenderQuality">Quality of the last render used to determine if a HQ render is needed.</param>
        /// <returns>True if no further action is required. False if a delayed render is still needed.</returns>
        private bool RenderHighQualityIfNeeded(RenderType lastRenderQuality)
        {
            switch (lastRenderQuality)
            {
                case RenderType.LQOnly:
                    // we don't need a HQ render if the type is LQ only
                    return true;

                case RenderType.HQOnly:
                    // A HQ version is always needed
                    RenderHighQuality();
                    return true;

                case RenderType.HQAfterLQImmediately:
                    // A LQ version has been rendered, but we need a HQ version now
                    RenderHighQuality();
                    return true;

                case RenderType.HQAfterLQDelayed:
                    // A LQ version has been rendered, but we need a HQ version after a delay

                    if (RenderDelayTimer.IsRunning == false)
                        RenderDelayTimer.Restart();

                    if (RenderDelayTimer.ElapsedMilliseconds > RenderDelayMilliseconds)
                    {
                        RenderHighQuality();
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                default:
                    throw new ArgumentException($"Unknown quality: {lastRenderQuality}");
            }
        }

        /// <summary>
        /// Process every event in the queue.
        /// A render will be executed after each event is processed.
        /// A slight delay will be added between queue checks.
        /// </summary>
        private async Task RunQueueProcessor()
        {
            bool RequestExit = false;
            QueueProcessorIsRunning = true;
            IUIEvent lastEvent = null;

            while (RequestExit == false || QueueHasUnprocessedEvents)
            {
                // process all events in the queue
                bool eventWasProcessed = false;
                bool hqOnly = false;
                while (QueueHasUnprocessedEvents)
                {
                    var uiEvent = Queue.Dequeue();
                    uiEvent.ProcessEvent();
                    EventPostProcess(uiEvent);
                    hqOnly |= uiEvent.RenderOrder == RenderType.HQOnly;
                    if (uiEvent.RenderOrder != RenderType.None)
                    {
                        lastEvent = uiEvent;
                        eventWasProcessed |= true;
                    }
                }

                if (eventWasProcessed && hqOnly == false)
                    RenderLowQuality();

                await Task.Delay(1); // TODO: is this required?

                if (QueueIsEmpty)
                {
                    // avoid HQ rendering if new requests arrived after lq Render
                    bool finalEventProcessed = RenderHighQualityIfNeeded(lastEvent.RenderOrder);
                    if (finalEventProcessed)
                    {
                        QueueProcessorIsRunning = false;
                        return;
                    }
                }
            };

            // all events have been processed and no delayed renders are needed
            QueueProcessorIsRunning = false;
            return;
        }
    }
}
