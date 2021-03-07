namespace ScottPlot.Control.EventProcess.Events
{
    /// <summary>
    /// This event toggles visibility of the benchmark.
    /// This event is typically called after double-clicking the plot.
    /// </summary>
    public class BenchmarkToggleEvent : IUIEvent
    {
        private Plot plt;
        public RenderType RenderOrder { get; set; } = RenderType.HQOnly;

        public BenchmarkToggleEvent(Plot plt)
        {
            this.plt = plt;
        }

        public void ProcessEvent()
        {
            plt.Benchmark(!plt.Benchmark(null));
        }
    }
}
