using System;

namespace ScottPlot
{
    /// <summary>
    /// Internal interface used for Utility Functions within <see cref="DataSourceUtilities"/>
    /// </summary>
    public interface IDataSource
    {
        /// <summary>
        /// When set true, <see cref="DataSourceUtilities"/> should prefer paths that utilize <see cref="GetCoordinates"/>
        /// </summary>
        bool PreferCoordinates { get; }

        /// <summary> The length of the collection </summary>
        int Length { get; }
        int MinRenderIndex { get; }
        int MaxRenderIndex { get; }

        /// <summary> Gets the X-Y coordinate from the data source at the specified index </summary>
        Coordinates GetCoordinate(int index);

        /// <summary> Gets the X-Y coordinate from the data source at the specified index with any offsets and scaling applied </summary>
        Coordinates GetCoordinateScaled(int index);

        /// <summary> Gets the X value from the data source at the specified index </summary>
        double GetX(int index);

        /// <summary> Gets the X value from the data source at the specified index with any offsets and scaling applied </summary>
        double GetXScaled(int index);

        /// <summary> Gets the Y value from the data source at the specified index </summary>
        double GetY(int index);

        /// <summary> Gets the Y value from the data source at the specified index with any offsets and scaling applied </summary>
        double GetYScaled(int index);

    }
}
