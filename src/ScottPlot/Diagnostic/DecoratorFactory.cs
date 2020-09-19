namespace ScottPlot.Diagnostic
{
    public class DecoratorFactory
    {
        public Plottable WrapWithAll(Plottable plottable)
        {
            Plottable result = plottable;
            result = new CheckFieldsForInfinityDecorator(result);
            result = new CheckFieldsForNANDecorator(result);
            result = new CheckFiledsOrderedAccendingDecorator(result);

            return result;
        }
    }
}
