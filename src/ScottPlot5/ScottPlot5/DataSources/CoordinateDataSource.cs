namespace ScottPlot.DataSources
{
    /// <summary>
    /// Helper class used when a source (such as <see cref="IScatterSource"/>) does not implement <see cref="IDataSource"/>
    /// <br/> This copies the collection to an array, and sorts it during construction.
    /// </summary>
    public class CoordinateDataSource : IDataSource
    {
        private readonly IReadOnlyList<Coordinates> coordinates;

        public double XOffset = 0;
        public double YOffset = 0;
        public double XScale = 1;
        public double YScale = 1;

        public CoordinateDataSource(IReadOnlyList<Coordinates> readOnlyList)
        {
            coordinates = readOnlyList;
        }

        public bool PreferCoordinates => true;
        public bool IsSorted => coordinates.IsAscending(BinarySearchComparer.Instance);
        public int Length => coordinates.Count;
        public int MinRenderIndex => 0;
        public int MaxRenderIndex => coordinates.Count - 1;
        public Coordinates GetCoordinate(int index) => coordinates[index];
        public Coordinates GetCoordinateScaled(int index) => DataSourceUtilities.ScaleCoordinate(coordinates[index], XScale, XOffset, YScale, YOffset);
        public double GetX(int index) => coordinates[index].X;
        public int GetXClosestIndex(Coordinates mouseLocation) => DataSourceUtilities.GetClosestIndex(coordinates is Coordinates[] ca? ca : [.. coordinates], mouseLocation, new IndexRange(MinRenderIndex, MaxRenderIndex));
        public double GetXScaled(int index) => DataSourceUtilities.ScaleXY(coordinates[index].X, XScale, XOffset);
        public double GetY(int index) => coordinates[index].Y;
        public double GetYScaled(int index) => DataSourceUtilities.ScaleXY(coordinates[index].Y, XScale, XOffset);
    }
}
