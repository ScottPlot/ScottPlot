namespace ScottPlot.Plottables.Interactive.SubplotPreRenderActions;

/// <summary>
///     Demonstrates how to link multiple InteractiveVerticalLine objects so they move together.
/// </summary>
public class SharedVerticalLine : IMultiplotPreRenderAction
{
    private readonly List<InteractiveVerticalLine> _lines;
    private double[] _positions = [];
    private void UpdateCurrentPositions()
    => _positions = _lines.Select(l => l.X).ToArray();

    public SharedVerticalLine(params InteractiveVerticalLine[] lines)
    {
        _lines = lines.ToList();
        UpdateCurrentPositions();
    }

    public void Invoke()
    {
        //figure which, if any, have moved 
        int wanted = -1;
        for (int i = 0; i < _lines.Count; i++)
            if (_positions[i] != _lines[i].X)
                wanted = i;

        if (wanted == -1) return;

        //update all of them to match the one that moved
        double newX = _lines[wanted].X;

        for (int i = 0; i < _lines.Count; i++)
        {
            if (i != wanted)
                _lines[i].X = newX;
        }
        UpdateCurrentPositions();
    }
}
