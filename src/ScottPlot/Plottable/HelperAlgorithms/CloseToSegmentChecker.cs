namespace ScottPlot.Plottable.HelperAlgorithms
{
    /// <summary>
    /// Check if point close to segment
    /// This algorithm is not completely fair, and cannot be used separately;
    /// a separate preliminary check for proximity to the ends of the segment (AABBChecker)
    /// makes the results of this algorithm reliable.
    /// </summary>
    public class CloseToSegmentChecker
    {
        public double SnapX { get; set; }
        public double SnapY { get; set; }
        public double CoordinateX { get; set; }
        public double CoordinateY { get; set; }

        private (double x, double y)[][] _boundaryPolys;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="coordinateX">point x coordinate</param>
        /// <param name="coordinateY">point y coordinate</param>
        /// <param name="snapX">permissible deviation for x</param>
        /// <param name="snapY">permissible deviation for y</param>
        public CloseToSegmentChecker(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            CoordinateX = coordinateX;
            CoordinateY = coordinateY;
            SnapX = snapX;
            SnapY = snapY;

            _boundaryPolys = new (double x, double y)[][]
                {
                    new (double x, double y)[]
                    {
                        (0, 0),
                        (0, 0),
                        (0, 0),
                        (0, 0),
                    },
                    new (double x, double y)[]
                    {
                        (0, 0),
                        (0, 0),
                        (0, 0),
                        (0, 0),
                    }
                };
        }

        /// <summary>
        /// Check for point close to segment
        /// </summary>
        /// <param name="x">segment first point x coordinate</param>
        /// <param name="y">segment first point y coordinate</param>
        /// <param name="x1">segment second point x coordinate</param>
        /// <param name="y1">segment second point y coordinate</param>
        /// <returns>true if point close to segment</returns>
        public bool IsClose(double x, double y, double x1, double y1)
        {
            // fix first poly
            _boundaryPolys[0][0].x = x;
            _boundaryPolys[0][0].y = y + SnapY;

            _boundaryPolys[0][1].x = x;
            _boundaryPolys[0][1].y = y - SnapY;

            _boundaryPolys[0][2].x = x1;
            _boundaryPolys[0][2].y = y1 - SnapY;

            _boundaryPolys[0][3].x = x1;
            _boundaryPolys[0][3].y = y1 + SnapY;

            // fix second poly
            _boundaryPolys[1][0].x = x - SnapX;
            _boundaryPolys[1][0].y = y;

            _boundaryPolys[1][1].x = x + SnapX;
            _boundaryPolys[1][1].y = y;

            _boundaryPolys[1][2].x = x1 + SnapX;
            _boundaryPolys[1][2].y = y1;

            _boundaryPolys[1][3].x = x1 - SnapX;
            _boundaryPolys[1][3].y = y1;


            for (int k = 0; k < _boundaryPolys.Length; k++)
            {
                bool inside = false;
                for (int j = 0, j1 = _boundaryPolys[k].Length - 1; j < _boundaryPolys[k].Length; j1 = j++)
                {
                    if ((_boundaryPolys[k][j].y > CoordinateY) != (_boundaryPolys[k][j1].y > CoordinateY) &&
                        (CoordinateX < (_boundaryPolys[k][j1].x - _boundaryPolys[k][j].x) * (CoordinateY - _boundaryPolys[k][j].y) / (_boundaryPolys[k][j1].y - _boundaryPolys[k][j].y) + _boundaryPolys[k][j].x))
                    {
                        inside = !inside;
                    }
                }
                if (inside)
                    return true;
            }

            return false;
        }
    }
}
