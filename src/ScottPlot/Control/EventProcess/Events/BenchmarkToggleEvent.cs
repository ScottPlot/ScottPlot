namespace ScottPlot.Control.EventProcess.Events
{
    /// <summary>
    /// This event toggles visibility of the benchmark.
    /// This event is typically called after double-clicking the plot.
    /// </summary>
    public class BenchmarkToggleEvent : IUIEvent
    {
        private readonly Plot Plot;
        private readonly Configuration Configuration;
        public RenderType RenderType => Configuration.QualityConfiguration.BenchmarkToggle;

        public BenchmarkToggleEvent(Plot plt, Configuration config)
        {
            Plot = plt;
            Configuration = config;
        }

        public void ProcessEvent()
        {
            Plot.Benchmark(!Plot.Benchmark(null));
        }
    }
}
