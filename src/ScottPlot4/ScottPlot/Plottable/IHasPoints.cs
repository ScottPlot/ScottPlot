namespace ScottPlot.Plottable
{
    /// <summary>
    /// Indicates a plottable has data distributed along both axes
    /// and can return the X/Y location of the point nearest a given X/Y location.
    /// </summary>
    public interface IHasPoints : IHasPointsGeneric<double, double>
    {
    }

    /// <summary>
    /// Indicates a plottable has data distributed along the horizontal axis 
    /// and can return the X/Y location of the point nearest a given X value.
    /// </summary>
    public interface IHasPointsGenericX<TX, TY>
    {
        (TX x, TY y, int index) GetPointNearestX(TX x);
        (TY yMin, TY yMax) GetYDataRange(TX xMin, TX xMax);
    }

    /// <summary>
    /// Indicates a plottable has data distributed along the vertical axis 
    /// and can return the X/Y location of the point nearest a given Y value.
    /// </summary>
    public interface IHasPointsGenericY<TX, TY>
    {
        (TX x, TY y, int index) GetPointNearestY(TY y);
    }

    /// <summary>
    /// Indicates a plottable has data distributed along both axes
    /// and can return the X/Y location of the point nearest a given X/Y location.
    /// </summary>
    public interface IHasPointsGeneric<TX, TY> : IHasPointsGenericX<TX, TY>, IHasPointsGenericY<TX, TY>
    {
        (TX x, TY y, int index) GetPointNearest(TX x, TY y, TX xyRatio);
    }
}
