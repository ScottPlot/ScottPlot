namespace ScottPlot;

public class LinkedAxisRule(IAxis sourceAxis, IAxis targetAxis, Plot targetPlot)
{
    public IAxis SourceAxis { get; } = sourceAxis;
    public IAxis TargetAxis { get; } = targetAxis;
    public Plot TargetPlot { get; } = targetPlot;
}
