namespace ScottPlot.Axis;

public interface IYAxis : IAxis
{
    public AxisTranslation.IYAxisTranslator YTranslator { get; }
}
