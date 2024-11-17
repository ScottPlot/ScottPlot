namespace ScottPlot.Reporting;

public class Report
{
    public List<IComponent> ComponentList { get; } = [];

    public void AddHeading(string text, int level = 1)
    {
        ComponentList.Add(new Components.HeadingComponent(text, level));
    }

    public void AddPlot(Plot plot)
    {
        ComponentList.Add(new Components.PlotComponent(plot));
    }

    public void AddText(string text)
    {
        ComponentList.Add(new Components.TextComponent(text));
    }

    public void AddPageBreak()
    {
        ComponentList.Add(new Components.PageBreakComponent());
    }
}
