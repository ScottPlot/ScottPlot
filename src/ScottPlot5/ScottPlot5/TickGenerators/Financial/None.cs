namespace ScottPlot.TickGenerators.Financial;

public class None : IFinancialTickGenerator
{
    public List<(int, string)> GetTicks(DateTime[] DateTimes, int minIndexInView, int maxIndexInView)
    {
        return [];
    }
}
