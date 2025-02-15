
namespace ScottPlot
{
    /// <summary>
    /// This interface is used by plottables that can locate a <see cref="DataPoint"> within a specified range of a coordinate on the plot.
    /// </summary>
    public interface IGetNearest
    {
        /// <summary>
        /// Return the point nearest a specific location given the X/Y pixel scaling information from a previous render.
        /// Will return <see cref="DataPoint.None"/> if the nearest point is greater than <paramref name="maxDistance"/> pixels away.
        /// </summary>
        DataPoint GetNearest(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance);

        /// <summary>
        /// Return the point nearest a specific X location given the X/Y pixel scaling information from a previous render.
        /// Will return <see cref="DataPoint.None"/> if the nearest point is greater than <paramref name="maxDistance"/> pixels away.
        /// </summary>
        DataPoint GetNearestX(Coordinates mouseLocation, RenderDetails renderInfo, float maxDistance);
    }
}
