namespace ScottPlot.Axis.AxisTranslation;

public interface IYAxisTranslator : IAxisTranslator
{
    public double Bottom { get; set; }
    public double Top { get; set; }
    public double Height { get; }
}
