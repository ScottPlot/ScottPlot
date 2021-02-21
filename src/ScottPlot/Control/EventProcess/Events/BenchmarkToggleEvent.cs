namespace ScottPlot.Control.EventProcess.Events
{
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
