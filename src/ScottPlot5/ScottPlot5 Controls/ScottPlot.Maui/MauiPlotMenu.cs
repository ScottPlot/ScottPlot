namespace ScottPlot.Maui;

public class MauiPlotMenu(MauiPlot mp) : IPlotMenu
{
    private readonly MauiPlot MauiPlot = mp;

    public void Add(string Label, Action<IPlotControl> action)
    {
        throw new NotImplementedException();
    }

    public void AddSeparator()
    {
        throw new NotImplementedException();
    }

    public void Clear()
    {
        throw new NotImplementedException();
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void ShowContextMenu(Pixel pixel)
    {
        throw new NotImplementedException();
    }
}
