using System;

namespace ScottPlot.Plottable.HelperAlgorithms
{
    /// <summary>
    /// Check for point inside axis-aligned bounding box (AABB)
    /// </summary>
    public class AABBChecker
    {
        public double SnapX { get; set; }
        public double SnapY { get; set; }
        public double CenterX { get; set; }
        public double CenterY { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="centerX">center x coordinate</param>
        /// <param name="centerY">center y coordinate</param>
        /// <param name="snapX">half width of AABB</param>
        /// <param name="snapY">half height of AABB</param>
        public AABBChecker(double centerX, double centerY, double snapX, double snapY)
        {
            CenterX = centerX;
            CenterY = centerY;
            SnapX = snapX;
            SnapY = snapY;
        }

        /// <summary>
        /// Check for point inside axis-aligned bounding box
        /// </summary>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        /// <returns>True if point inside AABB</returns>
        public bool CheckInsideAABB(double x, double y)
        {
            return (x >= CenterX - SnapX && x <= CenterX + SnapX
                && y >= CenterY - SnapY && y <= CenterY + SnapY);
        }
    }
}
