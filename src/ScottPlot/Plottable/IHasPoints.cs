namespace ScottPlot.Plottable
{
    public interface IHasPointsXCalculatable<TX, TY>
    {
        (TX x, TY y, int index) GetPointNearestX(TX x);
    }

    public interface IHasPointsYCalculatable<TX, TY>
    {
        (TX x, TY y, int index) GetPointNearestY(TY y);
    }

    public interface IHasPointsGeneric<TX, TY> : IHasPointsXCalculatable<TX, TY>, IHasPointsYCalculatable<TX, TY>
    {
        (TX x, TY y, int index) GetPointNearest(TX x, TY y, TX xyRatio);
    }

    public interface IHasPoints : IHasPointsGeneric<double, double>
    {
    }
}
