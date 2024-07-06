namespace ScottPlot;

public interface IRadialAxis : IPolarAxis
{
    Spoke[] Spokes { get; }
}
