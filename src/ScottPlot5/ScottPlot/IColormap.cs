namespace ScottPlot;

public interface IColormap
{
    /// <summary>
    /// Full name for this colormap
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Returns a color for the given <paramref name="intensity"/> and <paramref name="intensityRange"/>. 
    /// Returns <see cref="Colors.Transparent"/> for an intensity of <see cref="double.NaN"/>.
    /// </summary>
    /// <param name="intensity">Intensity fraction from 0 to 1 (unless <paramref name="intensityRange"/> is specified)</param>
    /// <param name="intensityRange">If defined, <paramref name="intensity"/> is evaluated as a fraction along this range.</param>
    Color GetColor(double intensity, Range? intensityRange);
}
