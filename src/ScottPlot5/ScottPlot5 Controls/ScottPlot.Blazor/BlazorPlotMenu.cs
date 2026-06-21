namespace ScottPlot.Blazor;

public class BlazorPlotMenu : IPlotMenu
{
    public List<ContextMenuItem> ContextMenuItems { get; } = new();

    public void ShowContextMenu(Pixel pixel)
    {
        //throw new NotImplementedException();
    }

    public void Reset()
    {
    }

    public void Clear()
    {
    }

    public void Add(string Label, Action<Plot> action)
    {
    }

    public void AddSeparator()
    {
    }
}
