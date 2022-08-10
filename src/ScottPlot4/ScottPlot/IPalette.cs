using System.Collections.Generic;

namespace ScottPlot;

/// <summary>
/// A palette is a collection of colors
/// </summary>
public interface IPalette : IEnumerable<System.Drawing.Color>
{
    /// <summary>
    /// Name of this palette
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Name of this palette
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Colors in this palette
    /// </summary>
    System.Drawing.Color[] Colors { get; }

    /// <summary>
    /// Get the color at the specified index (with rollover)
    /// </summary>
    System.Drawing.Color GetColor(int index);

    /// <summary>
    /// Get the color at the specified index (with rollover) with alpha (0 = transparent, 1 = opaque)
    /// </summary>
    System.Drawing.Color GetColor(int index, double alpha = 1);

    /// <summary>
    /// Get the first several colors
    /// </summary>
    System.Drawing.Color[] GetColors(int count, int offset = 0, double alpha = 1);

    /// <summary>
    /// Get the color at the specified index (with rollover)
    /// </summary>
    (byte r, byte g, byte b) GetRGB(int index); // TODO: stop using this and/or add RGBA

    /// <summary>
    /// Returns the total number of colors in this palette
    /// </summary>
    int Count();
}
