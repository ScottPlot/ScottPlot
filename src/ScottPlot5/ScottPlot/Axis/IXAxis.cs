namespace ScottPlot.Axis;

public interface IXAxis : IAxis
{
    public AxisTranslation.IXAxisTranslator XTranslator { get; }
}
