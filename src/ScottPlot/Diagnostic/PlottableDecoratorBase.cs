using ScottPlot.Config;

namespace ScottPlot.Diagnostic
{
    public class PlottableDecoratorBase : Plottable
    {
        protected Plottable sourcePlottable = null;
        protected Plottable plottable;
        public PlottableDecoratorBase(Plottable plottable)
        {
            this.plottable = plottable;
            if (!(plottable is PlottableDecoratorBase))
                sourcePlottable = plottable;
            else
                sourcePlottable = (plottable as PlottableDecoratorBase).sourcePlottable;
        }

        protected virtual void BeforeRenderCheck()
        {

        }

        protected virtual void AfterRenderCheck()
        {

        }

        public override LegendItem[] GetLegendItems()
        {
            return plottable.GetLegendItems();
        }

        public override AxisLimits2D GetLimits()
        {
            return plottable.GetLimits();
        }

        public override int GetPointCount()
        {
            return plottable.GetPointCount();
        }

        public override void Render(Settings settings)
        {
            BeforeRenderCheck();
            plottable.Render(settings);
            AfterRenderCheck();
        }

        public override string ToString()
        {
            return plottable.ToString() + " (Decorator)";
        }
    }
}
