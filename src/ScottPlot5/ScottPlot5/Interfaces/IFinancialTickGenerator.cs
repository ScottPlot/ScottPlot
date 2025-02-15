namespace ScottPlot;

public interface IFinancialTickGenerator
{
    List<(int, string)> GetTicks(DateTime[] DateTimes, int minIndexInView, int maxIndexInView);
}
