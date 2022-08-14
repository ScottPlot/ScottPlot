namespace ScottPlot.Axis.AxisTranslation;

public interface IXAxisTranslator : IAxisTranslator
{
    public double Left { get; set; }
    public double Right { get; set; }
    public double Width { get; }
}
