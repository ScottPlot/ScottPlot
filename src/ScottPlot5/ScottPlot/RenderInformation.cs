using System.Diagnostics;

namespace ScottPlot;

/// <summary>
/// Stores information about a render for debugging or later retrieval.
/// </summary>
public class RenderInformation
{
    public TimeSpan Elapsed => Stopwatch.Elapsed;
    public PixelRect FigureRect;
    public PixelRect DataRect;
    public bool IsFinished;

    public readonly Stopwatch Stopwatch = Stopwatch.StartNew();

    public void Finished()
    {
        Stopwatch.Stop();
        IsFinished = true;
    }
}
