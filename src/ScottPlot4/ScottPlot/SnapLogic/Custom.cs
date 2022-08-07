using System;

namespace ScottPlot.SnapLogic;

/// <summary>
/// Create a snap object from a custom function
/// </summary>
public class Custom : ISnap
{
    private readonly Func<double, double> SnapFunction;

    public Custom(Func<double, double> snapFunction)
    {
        SnapFunction = snapFunction;
    }

    public double Snap(double value)
    {
        return SnapFunction.Invoke(value);
    }
}
