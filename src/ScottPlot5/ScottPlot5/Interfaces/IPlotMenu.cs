namespace ScottPlot;

public interface IPlotMenu
{
    public List<ContextMenuItem> ContextMenuItems { get; }

    public void Reset();

    public void Clear();

    public void Add(string Label, Action<Plot> action);

    public void AddSeparator();


    void ShowContextMenu(Pixel pixel);
}
