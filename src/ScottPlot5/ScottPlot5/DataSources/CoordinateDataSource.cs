namespace ScottPlot.DataSources
{
    /// <summary>
    /// Helper class used when a source (such as <see cref="IScatterSource"/>) does not implement <see cref="IDataSource"/>
    /// </summary>
    public class CoordinateDataSource(IReadOnlyList<Coordinates> readOnlyList) : IDataSource
    {
        private readonly IReadOnlyList<Coordinates> coordinates = readOnlyList;

        public double XOffset { get; set; }
        public double YOffset { get; set; }
        public double XScale { get; set; } = 1;
        public double YScale { get; set; } = 1;

        public bool PreferCoordinates => true;
        public int Length => coordinates.Count;
        public int MinRenderIndex { get; set; } = 0;
        public int MaxRenderIndex { get; set; } = readOnlyList.Count - 1;

        public Coordinates GetCoordinate(int index) => coordinates[index];
        public Coordinates GetCoordinateScaled(int index) => DataSourceUtilities.ScaleCoordinate(coordinates[index], XScale, XOffset, YScale, YOffset);
        public double GetX(int index) => coordinates[index].X;
        public int GetXClosestIndex(Coordinates mouseLocation)
            => DataSourceUtilities.GetClosestIndex(
                coordinates is Coordinates[] ca ? ca : [.. coordinates],
                DataSourceUtilities.UnScaleCoordinate(mouseLocation, XScale, XOffset, YScale, YOffset),
                this.GetRenderIndexRange()
                );
        public double GetXScaled(int index) => DataSourceUtilities.ScaleXY(coordinates[index].X, XScale, XOffset);
        public double GetY(int index) => coordinates[index].Y;
        public double GetYScaled(int index) => DataSourceUtilities.ScaleXY(coordinates[index].Y, YScale, YOffset);
        public bool IsSorted() => coordinates.IsAscending(BinarySearchComparer.Instance);

    }
}
