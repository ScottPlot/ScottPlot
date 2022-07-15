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

    public readonly Stopwatch Stopwatch = new();

    public RenderInformation(bool start = true)
    {
        if (start)
            Stopwatch.Start();
    }

    public void Finished()
    {
        Stopwatch.Stop();
        IsFinished = true;
    }
}
